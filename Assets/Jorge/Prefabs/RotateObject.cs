using UnityEngine;

public class InfiniteRotateObject : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Eje de rotación (X, Y o Z)
    public float rotationSpeed = 100f; // Velocidad de rotación en grados por segundo

    void Update()
    {
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime);
    }
}
