using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    public int weight = 10; // Peso del recurso
    private Rigidbody rb;
    private Collider col;
    private Transform originalParent;
    private bool isPickedUp = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        originalParent = transform.parent;
    }

    void Update()
    {
        if (!isPickedUp)
        {
            transform.Rotate(Vector3.up, 30f * Time.deltaTime); // Rotación suave en el eje Y
        }
    }

    public void PickUp(Transform attachPoint)
    {
        isPickedUp = true;
        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(attachPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop(Vector3 dropForce)
    {
        isPickedUp = false;
        transform.SetParent(originalParent);
        rb.isKinematic = false;
        col.enabled = true;
        rb.AddForce(dropForce, ForceMode.Impulse);
    }

    public int GetWeight()
    {
        return weight;
    }
}
