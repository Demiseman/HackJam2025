using UnityEngine;

public class IsoCameraController : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public Transform motherShip; // Referencia a la nave nodriza

    // Distancias de cambio entre los estados (configurables en Inspector)
    public float closeRange = 20f; // Distancia máxima para el rango cercano
    public float midRange = 60f;   // Distancia máxima para el rango medio
    public float maxSeparation = 100f; // Distancia máxima antes del rango lejano

    // Distancias de la cámara para cada rango
    public float closeDistance = 20f; // Distancia de la cámara cuando está cerca
    public float midDistance = 50f;   // Distancia de la cámara en rango medio
    public float farDistance = 80f;   // Distancia de la cámara cuando está lejos

    public float smoothSpeed = 5f; // Factor de suavizado
    public float cameraTiltAngle = 30f; // Ángulo de inclinación de la cámara
    public float lerpDuration = 1f; // Tiempo en segundos que tarda la interpolación

    private Vector3 offset; // Offset inicial de la cámara
    private float currentDistance; // Distancia actual de la cámara
    private float lerpProgress = 0f; // Progreso del Lerp

    private Vector3 lastTargetPosition; // Última posición objetivo de la cámara
    private Quaternion lastTargetRotation; // Última rotación objetivo de la cámara

    void Start()
    {
        if (player == null || motherShip == null)
        {
            Debug.LogError("Asigna el Player y la MotherShip en el inspector.");
            return;
        }

        offset = transform.position - player.position; // Calcula el offset inicial
        currentDistance = closeDistance; // Inicializa la distancia de la cámara
        lastTargetPosition = transform.position;
        lastTargetRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Distancia del jugador a la nave nodriza
        float distanceToMotherShip = Vector3.Distance(player.position, motherShip.position);

        // Determinar el rango actual y la distancia de la cámara
        float targetDistance;

        if (distanceToMotherShip <= closeRange)
        {
            targetDistance = closeDistance; // Rango cercano
        }
        else if (distanceToMotherShip <= midRange)
        {
            targetDistance = midDistance; // Rango medio
        }
        else
        {
            targetDistance = farDistance; // Rango lejano
        }

        // Si cambia el objetivo, reiniciar la interpolación
        if (Mathf.Abs(targetDistance - currentDistance) > 0.1f)
        {
            lerpProgress = 0f;
        }

        // Aplicar interpolación progresiva
        lerpProgress += Time.deltaTime / lerpDuration;
        lerpProgress = Mathf.Clamp01(lerpProgress);

        currentDistance = Mathf.Lerp(currentDistance, targetDistance, lerpProgress);

        // Calcular la nueva posición de la cámara
        Vector3 targetPosition = player.position + offset.normalized * currentDistance;
        lastTargetPosition = Vector3.Lerp(lastTargetPosition, targetPosition, lerpProgress);
        transform.position = lastTargetPosition;

        // 🔹 Ajuste del ángulo de inclinación (picado)
        Quaternion targetRotation = Quaternion.Euler(cameraTiltAngle, transform.eulerAngles.y, 0);
        lastTargetRotation = Quaternion.Slerp(lastTargetRotation, targetRotation, lerpProgress);
        transform.rotation = lastTargetRotation;

        // Apuntar ligeramente hacia el jugador
        transform.LookAt(player.position + Vector3.up * 2f);
    }
}
