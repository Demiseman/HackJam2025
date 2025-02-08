using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    public int weight = 10; // Peso del recurso
    private bool isCollected = false;
    private Transform attachPoint; // Lugar donde se pegará al Player

    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Collect(Transform playerHand)
    {
        if (isCollected) return;

        isCollected = true;
        attachPoint = playerHand;

        // Desactivar físicas para que no caiga
        rb.isKinematic = true;
        col.enabled = false;

        // Pegar a la mano del Player
        transform.SetParent(attachPoint);
        transform.localPosition = Vector3.zero; // Ajustar la posición en la mano
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        if (!isCollected) return;

        isCollected = false;
        transform.SetParent(null); // Despegar del Player

        // Activar físicas de nuevo
        rb.isKinematic = false;
        col.enabled = true;

        // Lanzarlo un poco hacia adelante
        rb.AddForce(attachPoint.forward * 3f, ForceMode.Impulse);
    }

    public bool IsCollected()
    {
        return isCollected;
    }
}