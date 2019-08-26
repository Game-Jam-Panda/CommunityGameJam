using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Utils
{
    public class WaypointContainer : MonoBehaviour
	{
		private void OnDrawGizmos() {

            //Waypoints
            Vector3 firstPosition = transform.GetChild(0).position;
			Vector3 previousPosition = firstPosition;
	
			foreach (Transform waypoint in transform)
			{
                Gizmos.color = Color.yellow;
    	        Gizmos.DrawSphere(waypoint.position, 0.2f);                                 // Sphere representing the waypoints
                Gizmos.color = Color.white;
				Gizmos.DrawLine(previousPosition, waypoint.position);						// Lines between waypoints
				previousPosition = waypoint.position; //Update to new previous waypoint
			}
            Gizmos.DrawLine(previousPosition, firstPosition);  								// Line between last position and first position

		}
	}
}