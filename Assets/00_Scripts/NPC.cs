using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public NavMeshAgent Agent;
    public List<Vector3> WalkPosition = new List<Vector3>();
    public Animator animator;

    public List<string> NPCName = new List<string>();
    public List<Material> Materials = new List<Material>();
    public float StandingDelay;

    bool _isWalking;
    float _nextTime;
    Transform Mesh;

    // Start is called before the first frame update
    void Start()
    {
        // StartWalking();
        Mesh = transform.Find(NPCName[Random.Range(0, NPCName.Count)]);
        Mesh.gameObject.SetActive(true);
        Mesh.gameObject.GetComponent<SkinnedMeshRenderer>().material = Materials[Random.Range(0, Materials.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        Roaming();
        if (Agent.remainingDistance != 0 && Agent.remainingDistance > Agent.stoppingDistance + 0.5f)
        {
            animator.SetInteger("State", 1);
        }
    }

    void Roaming()
    {
        if (AgentHasArrived())
        {
            if (_isWalking)
            {
                animator.SetInteger("State", 0);
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
        animator.SetInteger("State", 1);

        _isWalking = true;

        Agent.SetDestination(WalkPosition[index]);
    }

    bool AgentHasArrived()
    {
        if (!Agent.hasPath)
            return true;

        float dist = Agent.remainingDistance;

        if (dist != Mathf.Infinity && dist <= Agent.stoppingDistance + 0.5f)
        {
            return true;
        }
        return false;
    }

}
