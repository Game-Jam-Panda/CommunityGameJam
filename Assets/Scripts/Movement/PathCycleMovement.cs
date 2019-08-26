using System.Collections;
using System.Collections.Generic;
using CGJ.Utils;
using UnityEngine;

namespace CGJ.Movement
{
    public class PathCycleMovement : MonoBehaviour
    {
        [SerializeField] WaypointContainer pathCycle = null;
        [SerializeField] float movementSpeed = 2.0f;
        [SerializeField] float stoppingDistance = 0.1f;
        [SerializeField] float pauseTimeBetweenSteps = 0.0f;

        bool isCycling = false;
        int nextWaypointIndex; //Keeps track of the index of the next point in the cycle        

        void Start()
        {
            if(!isCycling)
            {
                isCycling = true;
                StartCoroutine(ProcessMovementCycle());
            }
        }

        IEnumerator ProcessMovementCycle()
        {
            while(pathCycle != null)
            {
                Vector3 nextWaypointPos = GetWaypointPosition(nextWaypointIndex);
                MoveTo(nextWaypointPos, movementSpeed);
                CycleWaypointsWhenClose(nextWaypointPos);
                yield return new WaitForSeconds(pauseTimeBetweenSteps);  //Pause between waypoints
            }
        }

        // Physically move
        void MoveTo(Vector3 destination, float speedFraction)
        {
            // Move our position a step closer to the target.
            float step =  movementSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
        }

        // Calculate waypoint values
        private Vector3 GetWaypointPosition(int waypointIndex)
        {
            Vector3 waypointPos = pathCycle.transform.GetChild(waypointIndex).position;
            return waypointPos;
        }
        private bool AtWaypoint(Vector3 waypointPosition)
        {
            float distanceToNextWaypoint = Vector3.Distance(transform.position, waypointPosition);
            return distanceToNextWaypoint <= stoppingDistance;
        }
        private void CycleWaypointsWhenClose(Vector3 nextWaypointPosition)
        {
           if(AtWaypoint(nextWaypointPosition))
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % pathCycle.transform.childCount;      //Update the next waypoint when close enough to current one
            }
        }
    }
}