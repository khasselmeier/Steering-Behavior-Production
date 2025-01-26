using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : Seek
{
    public GameObject[] path; // Array of waypoints
    private int currentPathIndex = 0; // Current waypoint index
    public float targetRadius = 0.5f; // Distance to consider the waypoint reached

    // Calculates steering output for following the path
    public override SteeringOutput getSteering()
    {
        // If no path is defined or has no waypoints, return null
        if (path == null || path.Length == 0)
        {
            Debug.LogWarning("Path is not defined or empty.");
            return null;
        }

        // Find the nearest waypoint as the initial target if no target is set
        if (target == null)
        {
            target = FindNearestWaypoint();
        }

        // Check if the character has reached the current target waypoint
        if (IsTargetReached())
        {
            // Move to the next waypoint in the path
            currentPathIndex = (currentPathIndex + 1) % path.Length; // Loop back to start
            target = path[currentPathIndex];
        }

        // Delegate the steering to Seek
        return base.getSteering();
    }

    // Finds the nearest waypoint to the character
    private GameObject FindNearestWaypoint()
    {
        int nearestIndex = 0;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < path.Length; i++)
        {
            float distance = Vector3.Distance(character.transform.position, path[i].transform.position);
            if (distance < nearestDistance)
            {
                nearestIndex = i;
                nearestDistance = distance;
            }
        }

        currentPathIndex = nearestIndex; // Update the path index to the nearest waypoint
        return path[nearestIndex];
    }

    // Checks if the character has reached the current target
    private bool IsTargetReached()
    {
        float distanceToTarget = Vector3.Distance(character.transform.position, target.transform.position);
        return distanceToTarget < targetRadius;
    }
}