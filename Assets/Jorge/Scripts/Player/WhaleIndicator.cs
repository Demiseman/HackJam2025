using UnityEngine;

public class WhaleIndicator : MonoBehaviour
{
    public Transform player;          // Referencia al jugador
    public Transform mothership;      // Referencia a la nave nodriza
    public GameObject indicator;      // Objeto del indicador
    public float activationDistance = 10f; // Distancia a la que se activa el indicador

    void Update()
    {
        if (player == null || mothership == null || indicator == null)
            return;

        // Calcular la distancia en el plano X-Z
        Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
        Vector3 mothershipPos = new Vector3(mothership.position.x, 0, mothership.position.z);
        float distance = Vector3.Distance(playerPos, mothershipPos);

        // Mostrar u ocultar el indicador según la distancia
        indicator.SetActive(distance >= activationDistance);

        // Hacer que el indicador apunte a la nave nodriza
        if (indicator.activeSelf)
        {
            Vector3 direction = mothershipPos - playerPos;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                indicator.transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            }
        }
    }
}