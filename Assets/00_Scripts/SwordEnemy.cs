using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordEnemy : HitAble
{
    [Header("Walk Settings")]
    public List<Vector3> WalkPlaces = new List<Vector3>();
    public NavMeshAgent Agent;
    public float StandingDelay = 5;

    [Header("Attack Settings")]
    public float AttackDistance;
    Animator animator;

    [Header("UI Settings")]
    public GameObject UI;

    GameObject _player;

    float _nextTime;

    int _index = 0;
    bool _isWalking = false;

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
        _player = GameObject.FindGameObjectWithTag("Player");
        animator = this.gameObject.GetComponent<Animator>();
        if (WalkPlaces.Count > 0 && Agent != null)
        {
            StartWalking();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (EnemyState)
        {
            case State.Roaming:
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
        
        UI.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

    }

    void Fleeing()
    {
        animator.SetInteger("State", 1);
    }

    void Attacking()
    {
        animator.SetInteger("State", 2);

    }
    void Chase()
    {
        animator.SetInteger("State", 1);

        float dist = Vector3.Distance(this.transform.position, _player.transform.position);
        if (dist > AttackDistance)
        {
            Agent.SetDestination(_player.transform.position);
        }
        else
        {
            EnemyState = State.Attacking;

        }
    }
    void Roaming()
    {
        animator.SetInteger("State", 0);
        float dist = Agent.remainingDistance;
        if (dist != Mathf.Infinity && Agent.pathStatus == NavMeshPathStatus.PathComplete && Agent.remainingDistance == 0)
        {
            //Agent is at destination
            if (_isWalking)
            {
                _isWalking = false;
                _nextTime = Time.time + StandingDelay;
            }
            else if (Time.time > _nextTime)
            {
                StartWalking();
            }
        }

    }
    void StartWalking()
    {
        _isWalking = true;

        Agent.SetDestination(WalkPlaces[_index]);
        if (_index == WalkPlaces.Count - 1)
        {

            _index = 0;
        }
        else
        {
            _index++;
        }

    }
    
}
