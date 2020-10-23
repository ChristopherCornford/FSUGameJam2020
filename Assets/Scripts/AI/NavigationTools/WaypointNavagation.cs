using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointNavagation : MonoBehaviour
{
    public Task MyTask { get; private set; }

    [Header("AI Behaviour Information")]
    [SerializeField] private Task myTask;


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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum Task
{
    None = 0,
    Explore,
    Patrol,
    Guard
}
