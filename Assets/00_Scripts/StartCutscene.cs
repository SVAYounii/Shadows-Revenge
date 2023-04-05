using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StartCutscene : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Animator anim;
    public GameObject WalkTo;

    public bool once = true;

    float nextTime;

    private void Update()
    {
        if (!once && Time.time >= nextTime)
        {
            WalkToLocation();

        }

    }


    public void WalkToLocation()
    {
        Agent.SetDestination(WalkTo.transform.position);
        print(Agent.remainingDistance);
        if (Agent.remainingDistance > Agent.stoppingDistance)
        {
            anim.SetInteger("State", 1);
        }
        else
        {
            anim.SetInteger("State", 0);
            once = true;
        }
    }
    public void StartCut()
    {
        once = false;
        nextTime = Time.time + 7f;

    }
}
