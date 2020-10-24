using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WaypointNavagation : MonoBehaviour
{
    public Task MyTask { get; private set; }
    public NavMeshAgent navAgent { get { return this.GetComponent<NavMeshAgent>(); } }

    [Header("AI Behaviour Information")]
    [SerializeField] private Task myTask;
    [SerializeField] private Waypoint startingWaypoint;

    [Header("Current Pathfinding Debug Info")]
    [SerializeField] private bool reachedDestination;

    [SerializeField] private Vector3 destinationPosition;
    [SerializeField] private Waypoint destinationWaypoint;

    [SerializeField] private Waypoint previousWaypoint;

    private void OnValidate()
    {
        if(myTask == MyTask)
        {
            return;
        }
        else
        {
            MyTask = myTask;
            //Do Stuff
        }
    }

    private void Start()
    {
        AquireNextDestination(startingWaypoint);
    }

    public void AquireNextDestination(Waypoint currentWaypoint)
    {
        List<Waypoint> possibleDestinations = new List<Waypoint>();

        foreach(Waypoint wp in currentWaypoint.connections)
        {
            if(previousWaypoint && wp == previousWaypoint) { continue; }

            else
            {
                possibleDestinations.Add(wp);
            }
        }

        int randIndex = Random.Range(0, possibleDestinations.Count);

        if (possibleDestinations[randIndex])
        {
            destinationWaypoint = possibleDestinations[randIndex];
            destinationPosition = possibleDestinations[randIndex].transform.position;
        }

        previousWaypoint = currentWaypoint;

        StartCoroutine(Navigate());
    }

    public IEnumerator Navigate()
    {
        navAgent.SetDestination(destinationPosition);

        reachedDestination = false;

        do
        {
            if (navAgent.remainingDistance <= 1.0f)
                reachedDestination = true;
            else
            {
                Debug.Log(navAgent.name + " is traveling to: " + destinationPosition.ToString() + ". \n Distance Remaining: " + navAgent.remainingDistance.ToString());
            }
                yield return new WaitForEndOfFrame();

        }
        while (!reachedDestination);

        AquireNextDestination(destinationWaypoint);
    }
}

public enum Task
{
    None = 0,
    Explore,
    Patrol,
    Guard
}
