using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WaypointNavagation : MonoBehaviour
{
    public Task MyTask { get; private set; }

    delegate void Job(Waypoint currentWaypoint);

    private Job myJob;
    public NavMeshAgent navAgent { get { return this.GetComponent<NavMeshAgent>(); } }

    [Header("AI Behaviour Information")]
    [SerializeField] private Task myTask;

    [Header("AI Task Information")]

    [SerializeField] private Waypoint explorationStartingWaypoint;
    [SerializeField] private List<Waypoint> patrolPoints = new List<Waypoint>(1);
    [SerializeField] private Waypoint waypointToGuard = null;

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
            
            switch((int)MyTask)
            {
                case 0:
                    myJob = null;
                    Debug.LogWarning(navAgent.name + " is not assigned a task!");
                    break;

                case 1:
                    myJob = Explore;

                    break;

                case 2:
                    myJob = Patrol;
                    break;

                case 3:
                    myJob = Guard;
                    break;
                    
            }
        }
    }

    private void Start()
    {
        GameManager.Instance.OnStartAINavigations += GameManager_OnStartAINavigation;
        GameManager.Instance.OnStopAINavigation += GameManager_OnStopAINavigation;
    }

    public void Explore(Waypoint currentWaypoint)
    {
        if(currentWaypoint.connections.Count == 1 && currentWaypoint.connections[0] == previousWaypoint)
        {
            destinationWaypoint = previousWaypoint;
            destinationPosition = previousWaypoint.transform.position;

            previousWaypoint = currentWaypoint;

            StartCoroutine(Navigate());

            return;
        }

        List<Waypoint> possibleDestinations = new List<Waypoint>();

        foreach(Waypoint wp in currentWaypoint.connections)
        {
            if(previousWaypoint && wp == previousWaypoint) { continue; }

            else
            {
                possibleDestinations.Add(wp);
            }
        }

        int randIndex = UnityEngine.Random.Range(0, possibleDestinations.Count);

        if (possibleDestinations[randIndex])
        {
            destinationWaypoint = possibleDestinations[randIndex];
            destinationPosition = possibleDestinations[randIndex].transform.position;
        }

        previousWaypoint = currentWaypoint;

        StartCoroutine(Navigate());
    }

    public void Patrol(Waypoint currentWaypoint)
    {
        for (int index = 0; index < patrolPoints.Count; index++)
        {
            if (patrolPoints[index] == currentWaypoint)
            {
                destinationWaypoint = (index + 1 == patrolPoints.Count) ? patrolPoints[0] : patrolPoints[index + 1];
                destinationPosition = destinationWaypoint.transform.position;
            }
        }

        previousWaypoint = currentWaypoint;

        StartCoroutine(Navigate());
    }

    public void Guard(Waypoint currentWaypoint)
    {

    }

    public IEnumerator Navigate()
    {
        navAgent.SetDestination(destinationPosition);

        reachedDestination = false;

        Debug.Log(navAgent.name + " is traveling to: " + destinationPosition.ToString() + ". \n Distance Remaining: " + navAgent.remainingDistance.ToString());

        do
        {
            if (navAgent.remainingDistance <= 1.0f)
                reachedDestination = true;
            else
            {
               // Do Something if need be
            }
                yield return new WaitForEndOfFrame();

        }
        while (!reachedDestination);

        myJob(destinationWaypoint);
    }

    private void GameManager_OnStartAINavigation(object sender, EventArgs e)
    {
        if(destinationWaypoint != null)
        {
            myJob(destinationWaypoint);

            return;
        }

        Waypoint startingWaypoint;

        switch ((int)MyTask)
        {
            case 1:
                startingWaypoint = explorationStartingWaypoint;
                break;
            case 2:
                startingWaypoint = patrolPoints[0];
                break;
            case 3:
                startingWaypoint = waypointToGuard;
                break;

            default:
                return;
        }

        myJob(startingWaypoint);
    }

    private void GameManager_OnStopAINavigation(object sender, EventArgs e)
    {
        this.StopAllCoroutines();
    }
}

public enum Task
{
    None = 0,
    Explore,
    Patrol,
    Guard
}
