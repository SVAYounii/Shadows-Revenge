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
    public bool AbleToWalk = true;


    [Header("Vision Settings")]
    public bool AbleToSee;
    

    bool _isWalking;
    float _nextTime;
    Transform Mesh;

    [Header("Vision Settings")]
    public float radius;
    [Range(0, 180)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;

    [HideInInspector]
    public GameObject Player;

    ThirdPersonMovement tpm;


    // Start is called before the first frame update
    void Start()
    {
        // StartWalking();
        Player = GameObject.FindGameObjectWithTag("Player");
        Mesh = transform.Find(NPCName[Random.Range(0, NPCName.Count)]);
        Mesh.gameObject.SetActive(true);
        Mesh.gameObject.GetComponent<SkinnedMeshRenderer>().material = Materials[Random.Range(0, Materials.Count)];
        tpm = Player.GetComponent<ThirdPersonMovement>();

        if (AbleToSee)
            StartCoroutine(FOVRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        if (!AbleToWalk)
            return;

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

    public IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            if (!tpm.sneakAbility.isSneaking)
                FieldOfViewCheck();
        }
    }
    public void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

}
