using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public int enemyDamage = 5;
    public float baseAcceleration = 10f; // Aceleración base del enemigo
    public float baseMaxSpeed = 20f; // Velocidad máxima base
    public float baseTurnSpeed = 120f; // Velocidad de giro base
    public float baseStability = 2f; // Controla la resistencia a giros descontrolados

    private float acceleration;
    private float maxSpeed;
    private float turnSpeed;
    private float stability;
    
    private Rigidbody rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY; // Evita inclinaciones
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Randomización leve de los parámetros para cada enemigo (±10%)
        acceleration = baseAcceleration * Random.Range(0.85f, 1.15f);
        maxSpeed = baseMaxSpeed * Random.Range(0.85f, 1.15f);
        turnSpeed = baseTurnSpeed * Random.Range(0.85f, 1.15f);
        stability = baseStability * Random.Range(0.85f, 1.15f);
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Dirección hacia el player (solo en X-Z)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        // Rotación gradual hacia el Player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));

        // Aplicar una fuerza en vez de cambiar la velocidad directamente
        Vector3 desiredVelocity = transform.forward * maxSpeed;
        Vector3 force = (desiredVelocity - rb.linearVelocity) * acceleration;
        rb.AddForce(force, ForceMode.Acceleration);

        // Limitar la rotación no deseada
        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, stability * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Shield")){
            ShieldController.THIS.EnemyCollision(enemyDamage);
            Destroy(gameObject);
        }
    }
}

