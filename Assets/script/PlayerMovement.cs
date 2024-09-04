using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent Humanoid;
    private Camera cam;

    public float maxSpeed = 10.0f; // The maximum speed of the agent
    public float stoppingDistance = 0.5f; // Minimum distance to target before stopping

    void Start()
    {
        Humanoid = GetComponent<NavMeshAgent>();
        cam = Camera.main;

        // Set the initial speed to zero to prevent the agent from moving immediately
        Humanoid.speed = 0;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Calculate the distance from the agent to the hit point
            float distanceToTarget = Vector3.Distance(hit.point, transform.position);

            // If the agent is within the stopping distance, reduce speed to zero
            if (distanceToTarget < stoppingDistance)
            {
                Humanoid.speed = 0;
            }
            else
            {
                // Otherwise, set the speed based on the distance to the target
                // The closer the agent is to the target, the slower it moves
                Humanoid.speed = Mathf.Lerp(0, maxSpeed, distanceToTarget / maxSpeed);

                // Set the destination of the agent
                Humanoid.SetDestination(hit.point);
            }
        }
    }
}

