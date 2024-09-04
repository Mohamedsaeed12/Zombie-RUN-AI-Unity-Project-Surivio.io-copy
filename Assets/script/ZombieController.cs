using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public float attackDistance = 2.0f;
    public int attackDamage = 10;
    public float attackCooldown = 1.0f;
    public int maxHealth = 3; // Adding health for the zombie
    private int currentHealth;

    private Transform playerTransform;
    private NavMeshAgent agent;
    private float lastAttackTime;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Move towards the player
        agent.SetDestination(playerTransform.position);

        // Attack the player if within attack distance and cooldown has passed
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackDistance
            && Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            playerTransform.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }

    internal void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        // Notify the spawner that a zombie has died
        FindObjectOfType<ZombieSpawner>().ZombieDied();
      
        gameManager.AddScore(1);  // Assuming each zombie kill is worth 10 points
        gameObject.SetActive(false);  // Or Destroy(gameObject); if you prefer to instantiate new ones instead of reactivating

      
    }

}
