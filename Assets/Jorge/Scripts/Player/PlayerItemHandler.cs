using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    public Transform holdPoint; // Punto donde se sujetarán los objetos
    public float detectionRadius = 3f;
    public float dropForce = 5f;
    public int maxCarriedItems = 5; // Límite de objetos que se pueden llevar

    private List<ResourceItem> carriedItems = new List<ResourceItem>(); // Lista de objetos recogidos
    private PlayerInputHandler playerInputHandler; // Referencia al script principal

    void Start()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>(); // Obtiene referencia al script de movimiento
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Recoger objeto
        {
            TryPickUp();
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // Soltar el objeto más reciente
        {
            Drop();
        }
    }

    void TryPickUp()
    {
        if (carriedItems.Count >= maxCarriedItems) return; // Si está lleno, no recoge más

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider col in colliders)
        {
            ResourceItem resource = col.GetComponent<ResourceItem>();
            if (resource != null && !carriedItems.Contains(resource))
            {
                carriedItems.Add(resource);
                resource.PickUp(GetNextHoldPoint());
                playerInputHandler.AddWeight(resource.GetWeight()); // 🏋️‍♂️ Añadir peso
                break;
            }
        }
    }

    void Drop()
    {
        if (carriedItems.Count == 0) return; // Si no hay objetos, no hace nada

        ResourceItem itemToDrop = carriedItems[carriedItems.Count - 1]; // Suelta el último recogido
        carriedItems.RemoveAt(carriedItems.Count - 1);
        playerInputHandler.ReduceWeight(itemToDrop.GetWeight()); // 🏋️‍♂️ Reducir peso
        itemToDrop.Drop(transform.forward * dropForce);
    }

    Transform GetNextHoldPoint()
    {
        // 🛠 Aquí podrías mejorar para distribuir objetos en la nave (como en una cuadrícula)
        return holdPoint;
    }
}
