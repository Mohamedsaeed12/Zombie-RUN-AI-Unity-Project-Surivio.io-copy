using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 50; // The amount of health restored to the player
    public float respawnTime = 10f; // The cooldown time before the health pickup respawns

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Heal(healthAmount); // Assuming a Heal method exists on the player
                gameObject.SetActive(false); // Disable the pickup
                Invoke(nameof(Respawn), respawnTime); // Schedule the respawn
            }
        }
    }

    private void Respawn()
    {
        gameObject.SetActive(true); // Reactivate the pickup
    }
}
