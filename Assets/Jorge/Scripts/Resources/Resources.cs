using UnityEngine;

public class Resource : MonoBehaviour
{
    private Transform mothership;
    private float baseFallSpeed;
    private float maxFallSpeed;
    private float attractionRadius;
    private float resourceCharge;
    private bool isFalling = false;

    public void StartFalling(Transform mothership, float baseFallSpeed, float maxFallSpeed, float attractionRadius, float resourceCharge)
    {
        this.mothership = mothership;
        this.baseFallSpeed = baseFallSpeed;
        this.maxFallSpeed = maxFallSpeed;
        this.attractionRadius = attractionRadius;
        this.resourceCharge = resourceCharge;
        isFalling = true;
    }

    void Update()
    {
        if (isFalling && mothership != null)
        {
            float distance = Vector3.Distance(transform.position, mothership.position);
            float fallSpeed = Mathf.Lerp(maxFallSpeed, baseFallSpeed, distance / attractionRadius);
            transform.position = Vector3.MoveTowards(transform.position, mothership.position, fallSpeed * Time.deltaTime);

            if (distance < 0.5f) // Si el recurso llega a la nave nodriza
            {
                ShieldController.THIS.shieldCharge += resourceCharge;
                ShieldController.THIS.shieldCharge = Mathf.Min(ShieldController.THIS.shieldCharge, ShieldController.THIS.maxShieldCharge);
                ShieldController.THIS.UpdateShieldSize();
                Destroy(gameObject); // Eliminar el recurso
            }
        }
    }
}