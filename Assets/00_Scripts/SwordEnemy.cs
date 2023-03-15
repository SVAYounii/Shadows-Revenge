using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using static UnityEditor.PlayerSettings;

public class SwordEnemy : HitAble
{
    [Header("Walk Settings")]
    public List<Vector3> WalkPlaces = new List<Vector3>();
    public NavMeshAgent Agent;
    public float StandingDelay = 5;

    [Header("Attack Settings")]
    public float AttackDistance;
    public bool HasWeapon;

    [Header("Vision Settings")]
    public float radius;
    [Range(0, 180)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;

    Animator animator;
    bool _hasAttackPerformed;
    bool _isReadyForAttack = true;
    float _nextTimeAttack;

    [Header("UI Settings")]
    public GameObject UI;

    [HideInInspector]
    public GameObject Player;

    float _nextTime;

    int _index = 0;
    bool _isWalking = false;
    bool _playerIsClose;
    bool _once;
    Vector3 randomDirection;
    public enum State
    {
        Roaming,
        Chasing,
        Attacking,
        Fleeing
    }

    public State EnemyState;
    // Start is called before the first frame update
    void Start()
    {
        EnemyState = State.Roaming;
        Player = GameObject.FindGameObjectWithTag("Player");
        animator = this.gameObject.GetComponent<Animator>();
        if (WalkPlaces.Count > 0 && Agent != null)
        {
            StartWalking();
        }
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        switch (EnemyState)
        {
            case State.Roaming:
                if (canSeePlayer && HasWeapon)
                {
                    EnemyState = State.Chasing;
                    break;
                }
                else if (canSeePlayer && !HasWeapon)
                {
                    EnemyState = State.Fleeing;
                }

                Roaming();
                break;
            case State.Chasing:
                Chase();
                break;
            case State.Attacking:
                Attacking();
                break;
            case State.Fleeing:

                Fleeing();
                break;
        }

        if (_isWalking && EnemyState == State.Roaming)
        {
            Agent.isStopped = false;
            animator.SetInteger("State", 1);

        }
        else if (!_isWalking && EnemyState == State.Roaming)
        {
            Agent.isStopped = true;
            animator.SetInteger("State", 0);

        }
        UI.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
     
    }

    void ResetNavMesh()
    {
        if (Agent.hasPath)
        {
            Agent.ResetPath();
        }
    }

    void Fleeing()
    {
        Debug.Log("Running Away");
        Agent.speed = 3.5f;

        if (!_once)
        {
            _once = true;
            ResetNavMesh();
        }

        float distance = Vector3.Distance(transform.position, Player.transform.position);
        if (distance < 20)
        {
            _playerIsClose = true;
        }
        else
        {
            _playerIsClose = false;
        }

        if (_playerIsClose && AgentHasArrived())
        {
            randomDirection = UnityEngine.Random.insideUnitSphere * 50;
            randomDirection += Player.transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 20, 1))
            {
                RunTo(hit.position);
            }
            animator.SetInteger("State", 2);
        }
        else if (!_playerIsClose && AgentHasArrived())
        {
            animator.SetInteger("State", 3);
        }


    }

    void RunTo(Vector3 pos)
    {
        Agent.isStopped = false;
        animator.SetInteger("State", 2);
        Agent.SetDestination(pos);
    }
    void WalkTo(Vector3 pos)
    {
        _isWalking = true;
        Agent.isStopped = false;
        animator.SetInteger("State", 1);
        Agent.SetDestination(pos);
    }

    bool AgentHasArrived()
    {
        if (!Agent.hasPath)
            return true;

        float dist = Agent.remainingDistance;

        if (dist != Mathf.Infinity && Agent.pathStatus == NavMeshPathStatus.PathComplete && dist <= Agent.stoppingDistance +0.5f)
        {
            return true;

        }
        return false;
    }

    void Attacking()
    {

        if (!_hasAttackPerformed)
        {
            //is currently in the attack animation
            animator.SetTrigger("Attack");
            _hasAttackPerformed = true;
            _nextTimeAttack = Time.time + 2.5f;
            Debug.Log("Attacking player");
        }
        else if (Time.time > _nextTimeAttack)
        {
            //after animation go back to chasing state

            EnemyState = State.Chasing;
            _hasAttackPerformed = false;
        }


    }
    void Chase()
    {
        Debug.Log("Chasing player");
        Agent.isStopped = false;
        Agent.speed = 3.5f;

        float dist = Vector3.Distance(this.transform.position, Player.transform.position);

        if (dist > Agent.stoppingDistance)
        {
            animator.SetInteger("State", 2);
            Agent.SetDestination(Player.transform.position);
        }
        else
        {

            if (!_isReadyForAttack)
            {
                //Not yet ready for attack
                animator.SetInteger("State", 0);


                _isReadyForAttack = true;
                _nextTimeAttack = Time.time + 2f;

            }
            else if (Time.time > _nextTimeAttack)
            {
                _isReadyForAttack = false;
                EnemyState = State.Attacking;
            }

        }
    }
    void Roaming()
    {
        Debug.Log("Roaming around");
        Agent.speed = 1.5f;
        animator.SetInteger("State", 1);

        if (AgentHasArrived())
        {
            Debug.Log("Arrived");
            //Agent is at destination
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

        WalkTo(WalkPlaces[_index]);
        if (_index == WalkPlaces.Count - 1)
        {

            _index = 0;
        }
        else
        {
            _index++;
        }

    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck()
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
