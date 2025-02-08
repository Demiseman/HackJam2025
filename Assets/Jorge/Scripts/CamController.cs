using UnityEngine;

public class IsoCameraController : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public Transform motherShip; // Referencia a la nave nodriza
    public float minDistance = 20f; // Distancia mínima de la cámara
    public float maxDistance = 80f; // Distancia máxima de la cámara
    public float maxSeparation = 50f; // Máxima distancia del jugador a la nave para ajustar la cámara
    public float smoothSpeed = 5f; // Tiempo de suavizado

    private Vector3 velocity = Vector3.zero; // Velocidad de interpolación
    private Vector3 offset; // Offset inicial de la cámara

    void Start()
    {
        if (player == null || motherShip == null)
        {
            Debug.LogError("Asigna el Player y la MotherShip en el inspector.");
            return;
        }

        offset = transform.position - player.position; // Calcula el offset inicial
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Distancia del jugador a la nave nodriza
        float distanceToMotherShip = Vector3.Distance(player.position, motherShip.position);

        // Ajustar la distancia de la cámara basándose en la separación
        float distanceFactor = Mathf.Pow(Mathf.Clamp01(distanceToMotherShip / maxSeparation), 0.7f);

        float targetDistance = Mathf.Lerp(minDistance, maxDistance, distanceFactor);

        // Calcular la posición deseada
        Vector3 targetPosition = player.position + offset.normalized * targetDistance;

        // Aplicar suavizado con SmoothDamp para evitar tirones
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // Asegurar que la cámara siempre mire al jugador
        transform.LookAt(player);
    }
}
