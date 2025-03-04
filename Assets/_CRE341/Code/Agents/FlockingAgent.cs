using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAgent : MonoBehaviour
{
    [Header("Leader Separation")]
    public float leaderSeparationWeight = 3f; // Weight for separation from the leader
    public float leaderSeparationRadius = 3f; // Radius within which to separate from the leader
    
    [Header("Movement")]
    public float baseSpeed = 5f; // Base speed of the agent
    public float speedVariation = 1f; // Maximum variation from the base speed
    private float currentSpeed;
    public float rotationSpeed = 5f;

    [Header("Flocking")]
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float separationWeight = 2f;
    public float followLeaderWeight = 2f; // Weight for following the leader
    public float neighborRadius = 5f;
    public float separationRadius = 2f;
    public float maxSpeed = 5f; // Maximum speed for flocking

    private Vector3 velocity;

    void Start()
    {
        // Add this agent to the list of flockers in the GameManager
        GM_Flocking.Instance.flockers.Add(this.gameObject);
        currentSpeed = baseSpeed + Random.Range(-speedVariation, speedVariation);

        velocity = transform.forward * currentSpeed;
        velocity.y = 0; // Add this line to ensure no initial vertical movement
    }

    void Update()
    {
        // Calculate flocking forces
        Vector3 alignment = CalculateAlignment();
        Vector3 cohesion = CalculateCohesion();
        Vector3 separation = CalculateSeparation();
        Vector3 followLeader = FollowLeader();
        Vector3 leaderSeparation = SeparateFromLeader(); // Calculate separation from leader

         // Combine forces with weights
        velocity += (alignment * alignmentWeight) + (cohesion * cohesionWeight) +
                    (separation * separationWeight) + (followLeader * followLeaderWeight) +
                    (leaderSeparation * leaderSeparationWeight); // Add leader separation force

        // Limit speed
          velocity = Vector3.ClampMagnitude(velocity, currentSpeed);

        // Project the velocity onto the XZ plane to restrict movement to the horizontal plane
        velocity.y = 0;

        // Move the agent
        transform.position += velocity * Time.deltaTime;

        // Rotate the agent to face the direction of movement
        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    Vector3 CalculateAlignment()
    {
        // Alignment: Steer towards the average heading of local flockmates
        Vector3 averageHeading = Vector3.zero;
        int neighborCount = 0;

        foreach (GameObject flocker in GM_Flocking.Instance.flockers)
        {
            if (flocker != this.gameObject && Vector3.Distance(transform.position, flocker.transform.position) < neighborRadius)
            {
                averageHeading += flocker.transform.forward;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            averageHeading /= neighborCount;
            return (averageHeading - transform.forward).normalized;
        }

        return Vector3.zero;
    }

    Vector3 CalculateCohesion()
    {
        // Cohesion: Steer to move toward the average position of local flockmates
        Vector3 averagePosition = Vector3.zero;
        int neighborCount = 0;

        foreach (GameObject flocker in GM_Flocking.Instance.flockers)
        {
            if (flocker != this.gameObject && Vector3.Distance(transform.position, flocker.transform.position) < neighborRadius)
            {
                averagePosition += flocker.transform.position;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            averagePosition /= neighborCount;
            return (averagePosition - transform.position).normalized;
        }

        return Vector3.zero;
    }

    Vector3 CalculateSeparation()
    {
        // Separation: Steer to avoid crowding local flockmates
        Vector3 separationVector = Vector3.zero;
        int neighborCount = 0;

        foreach (GameObject flocker in GM_Flocking.Instance.flockers)
        {
            if (flocker != this.gameObject)
            {
                float distance = Vector3.Distance(transform.position, flocker.transform.position);
                if (distance < separationRadius)
                {
                    separationVector += (transform.position - flocker.transform.position).normalized / Mathf.Max(distance, 0.01f);
                    neighborCount++;
                }
            }
        }

        if (neighborCount > 0)
        {
            separationVector /= neighborCount;
        }

        return separationVector.normalized;
    }

    Vector3 FollowLeader()
    {
        // Follow the leader's position
        if (GM_Flocking.Instance.leader != null)
        {
            return (GM_Flocking.Instance.leader.transform.position - transform.position).normalized;
        }
        return Vector3.zero;
    }

    Vector3 SeparateFromLeader()
    {
        // Calculate a separation force from the leader if within leaderSeparationRadius
        if (GM_Flocking.Instance.leader != null)
        {
            float distance = Vector3.Distance(transform.position, GM_Flocking.Instance.leader.transform.position);
            if (distance < leaderSeparationRadius)
            {
                Vector3 separationDirection = (transform.position - GM_Flocking.Instance.leader.transform.position).normalized;
                return separationDirection / Mathf.Max(distance, 0.01f); // Scale by distance, similar to normal separation
            }
        }
        return Vector3.zero;
    }
}