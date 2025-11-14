using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private Transform player;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waypointReachDistance = 0.5f;
    [SerializeField] private float waitTimeAtWaypoint = 2f;

    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    private CharacterController characterController;
    private Vector3 moveDirection;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // If no character controller, add one
        if (characterController == null)
        {
            characterController = gameObject.AddComponent<CharacterController>();
        }

        // Auto-find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if player is in detection range
            if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
            {
                ChasePlayer();
            }
            else if (distanceToPlayer <= attackRange)
            {
                // Stop and attack (you can add attack logic here)
            }
            else
            {
                // Return to patrol
                Patrol();
            }
        }
        else
        {
            // Just patrol if no player found
            Patrol();
        }

        // Apply gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y -= 9.81f * Time.deltaTime;
        }
        else
        {
            moveDirection.y = -2f; // Keep grounded
        }

        // Move the monster
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void ChasePlayer()
    {
        isWaiting = false;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep movement horizontal

        moveDirection = direction * chaseSpeed;

        // Rotate towards player
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            // If no patrol points, just stay still or wander
            moveDirection = Vector3.zero;
            return;
        }

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            moveDirection = Vector3.zero;

            if (waitTimer <= 0)
            {
                isWaiting = false;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
            return;
        }

        Transform targetWaypoint = patrolPoints[currentPatrolIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        direction.y = 0;

        moveDirection = direction * moveSpeed;

        // Rotate towards waypoint
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Check if reached waypoint
        float distanceToWaypoint = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                                    new Vector3(targetWaypoint.position.x, 0, targetWaypoint.position.z));

        if (distanceToWaypoint <= waypointReachDistance)
        {
            isWaiting = true;
            waitTimer = waitTimeAtWaypoint;
        }
    }

    // Visualize detection ranges in editor
    void OnDrawGizmosSelected()
    {
        // Detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Patrol points
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    Gizmos.DrawSphere(patrolPoints[i].position, 0.3f);

                    // Draw lines between patrol points
                    if (i < patrolPoints.Length - 1 && patrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                    }
                }
            }
            // Connect last to first
            if (patrolPoints[patrolPoints.Length - 1] != null && patrolPoints[0] != null)
            {
                Gizmos.DrawLine(patrolPoints[patrolPoints.Length - 1].position, patrolPoints[0].position);
            }
        }
    }
}