using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public NavMeshAgent Agent;
    public List<Vector3> WalkPosition = new List<Vector3>();

    public float StandingDelay;

    bool _isWalking;
    float _nextTime;


    // Start is called before the first frame update
    void Start()
    {
        StartWalking();
    }

    // Update is called once per frame
    void Update()
    {
        Roaming();
    }

    void Roaming()
    {
        if (AgentHasArrived())
        {
            if (_isWalking)
            {
                _isWalking = false;
                _nextTime = Time.time + StandingDelay;
            }
            else if (Time.time > _nextTime)
            {
                Debug.Log("Walk!");

                StartWalking();
            }
        }
    }
    void StartWalking()
    {

        WalkTo(Random.Range(0, WalkPosition.Count));

    }

    void WalkTo(int index)
    {
        _isWalking = true;
        Agent.SetDestination(WalkPosition[index]);
    }

    bool AgentHasArrived()
    {
        if (!Agent.hasPath)
            return true;

        float dist = Agent.remainingDistance;

        if (dist != Mathf.Infinity && Agent.pathStatus == NavMeshPathStatus.PathComplete && dist <= Agent.stoppingDistance + 0.5f)
        {
            return true;

        }
        return false;
    }

}
