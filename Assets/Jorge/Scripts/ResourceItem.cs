using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    public int weight = 10; // Peso del recurso

    private Rigidbody rb;
    private Collider col;
    private Transform originalParent;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        originalParent = transform.parent;
    }

    public void PickUp(Transform attachPoint)
    {
        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(attachPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop(Vector3 dropForce)
    {
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
