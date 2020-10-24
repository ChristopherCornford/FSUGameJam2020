using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    protected float debugDrawRadius = 1.0f;
    [SerializeField]
    protected float connectivityRadius = 50f;

    public List<Waypoint> connections;
    
    public void OnValidate()
    {
        if(this.transform.tag != "Waypoint")
            this.transform.tag = "Waypoint";

        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        connections = new List<Waypoint>();

        for (int i = 0; i < allWaypoints.Length; i++)
        {
            Waypoint nextWaypoint = allWaypoints[i].GetComponent<Waypoint>();

            if (nextWaypoint != null)
            {
                if (Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= connectivityRadius && nextWaypoint != this)
                {

                    connections.Add(nextWaypoint);
                }
            }
        }
    }

    public void OnDrawGizmos()
    {

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, connectivityRadius);
    }
}
