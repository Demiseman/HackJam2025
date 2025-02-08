using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Movimiento")]
    public float baseSpeed = 7f;
    public float maxSpeed = 10f;
    public float minSpeed = 3f;
    public int peso = 0;
    private Vector2 movementInput;
    private Vector3 moveDirection;
    
    [Header("Dash / Rush")]
    public float dashForce = 20f;  // Intensidad del Dash
    public float dashCooldown = 1f; // Tiempo entre dashes
    public float dashLockTime = 0.3f; // Tiempo durante el cual los inputs no afectan al dash
    private bool canDash = true;
    private bool isDashing = false;

    [Header("Apuntado con Right Stick")]
    public Transform aimTarget; // GameObject que rotará con el Right Stick
    public float rotationSpeed = 10f;
    private Vector2 rightStickInput;
    
    private Camera mainCamera;
    public Animator animator;
    private Rigidbody rb;

    [Header("Frenado")]
    public float decelerationTime = 0.2f;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY;
    }

    void FixedUpdate()
    {
        AdjustSpeedBasedOnWeight();

        if (!isDashing) // No permite mover si está en Dash
        {
            MovePlayer();
        }

        RotateAimTarget();
    }

    private void MovePlayer()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDirection = (cameraForward * movementInput.y + cameraRight * movementInput.x).normalized;

        if (movementInput.magnitude > 0.1f)
        {
            rb.linearVelocity = moveDirection * baseSpeed;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime / decelerationTime);
            if (rb.linearVelocity.magnitude < 0.1f)
            {
                rb.linearVelocity = Vector3.zero;
                animator.SetBool("IsWalking", false);
            }
        }
    }

    private void RotateAimTarget()
    {
        if (aimTarget == null) return; // Si no hay objeto asignado, no hace nada

        if (rightStickInput.magnitude > 0.1f)
        {
            if (!aimTarget.transform.GetChild(0).gameObject.activeSelf) aimTarget.transform.GetChild(0).gameObject.SetActive(true);

            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 aimDirection = (cameraForward * rightStickInput.y + cameraRight * rightStickInput.x).normalized;

            if (aimDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
                aimTarget.rotation = Quaternion.Slerp(aimTarget.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            }
        } else {
            if (aimTarget.transform.GetChild(0).gameObject.activeSelf)aimTarget.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void Dash()
    {
        if (!canDash) return;

        canDash = false;
        isDashing = true;
        Vector3 dashDirection;

        // **Prioridad del Right Stick**
        if (rightStickInput.magnitude > 0.1f)
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            dashDirection = (cameraForward * rightStickInput.y + cameraRight * rightStickInput.x).normalized;
        }
        else if (movementInput.magnitude > 0.1f) // Si no hay Right Stick, usa el Move
        {
            dashDirection = moveDirection;
        }
        else
        {
            // **Si no hay input, hacer Dash hacia adelante**
            dashDirection = transform.forward;
        }

        rb.linearVelocity = Vector3.zero; // Evita acumulación de velocidad antes del dash
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
        Debug.Log("DASH ACTIVADO");

        // Bloquear inputs durante `dashLockTime`
        Invoke(nameof(EndDashLock), dashLockTime);

        // Reset del dash después del cooldown
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void EndDashLock()
    {
        isDashing = false;
    }

    private void ResetDash()
    {
        canDash = true;
    }

    private void AdjustSpeedBasedOnWeight()
    {
        baseSpeed = Mathf.Lerp(maxSpeed, minSpeed, peso / 100f);
    }

    public void AddWeight(int amount)
    {
        peso = Mathf.Clamp(peso + amount, 0, 100);
    }

    public void ReduceWeight(int amount)
    {
        peso = Mathf.Clamp(peso - amount, 0, 100);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!isDashing) // Bloquea la entrada de movimiento mientras el Dash está activo
        {
            movementInput = ctx.ReadValue<Vector2>();
        }
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        rightStickInput = ctx.ReadValue<Vector2>();
    }

    

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Dash();
        }
    }
}
