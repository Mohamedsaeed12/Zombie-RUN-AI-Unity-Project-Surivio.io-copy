using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float laserRange = 100f;
    public float shootingCooldown = 0.5f;
    public int maxHealth = 100;
    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }
    private GameManager gameManager;
    private float lastShootTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        FindObjectOfType<GameManager>().UpdateHealthDisplay(currentHealth);
        gameManager = FindObjectOfType<GameManager>();
        // Any other start logic if necessary
    }

    void Update()
    {
        if (Time.timeScale == 0) return; // Don't execute Update if the game is frozen

        if (Time.time > lastShootTime + shootingCooldown)
        {
            ShootLaser();
        }
    }

    void ShootLaser()
    {
        lastShootTime = Time.time;

        GameObject nearestZombie = FindNearestZombie();
        if (nearestZombie != null)
        {
            Vector3 directionToTarget = nearestZombie.transform.position - transform.position;
            RaycastHit hit;
            // Check if there is a clear line of sight to the zombie
            if (Physics.Raycast(transform.position, directionToTarget.normalized, out hit, laserRange))
            {
                // Check if the raycast hit the zombie and not another object (like a wall)
                if (hit.collider.gameObject == nearestZombie)
                {
                    // Draw the laser from the player to the zombie
                    lineRenderer.SetPositions(new Vector3[] { transform.position, hit.point });
                    lineRenderer.enabled = true;

                    // Damage the zombie
                    nearestZombie.GetComponent<ZombieController>().TakeDamage(1);

                    // Optionally, hide the laser after a short duration
                    StartCoroutine(DisableLaserAfterTime(0.1f));
                }
            }
        }
        else
        {
            // No zombie in range, disable the laser
            lineRenderer.enabled = false;
        }
    }

    GameObject FindNearestZombie()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        GameObject nearestZombie = null;
        float closestDistance = laserRange;

        foreach (GameObject zombie in zombies)
        {
            float distance = Vector3.Distance(transform.position, zombie.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestZombie = zombie;
            }
        }

        return nearestZombie;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        gameManager.UpdateHealthDisplay(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, 100);
        gameManager.UpdateHealthDisplay(currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        gameManager.PlayerDied();
        // Freeze the game
        Time.timeScale = 0;

    }


    IEnumerator DisableLaserAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        lineRenderer.enabled = false;
    }
}
