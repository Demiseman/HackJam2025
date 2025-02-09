using UnityEngine;

public class ResourceCollector  : MonoBehaviour
{
    public Transform mothership; // Referencia a la nave nodriza
    public float baseFallSpeed = 10f; // Velocidad mínima de caída
    public float maxFallSpeed = 100f; // Velocidad máxima de caída cerca de la nave
    public float attractionRadius = 5f; // Radio en el cual los recursos empiezan a ser atraídos
    public float resourceCharge = 5f; // Cantidad de energía que aporta cada recurso

    public bool tooLowCharge = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource") && !tooLowCharge) // Asegúrate de que los recursos tengan este tag
        {
            Debug.Log("Recurso detectado");
            Resource resource = other.GetComponent<Resource>();
            if (resource != null)
            {
                resource.StartFalling(mothership, baseFallSpeed, maxFallSpeed, attractionRadius, resourceCharge);
            }
        }
    }
}