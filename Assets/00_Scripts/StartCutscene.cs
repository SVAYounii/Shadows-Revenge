using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class StartCutscene : MonoBehaviour
{
    public GameObject Player;
    public GameObject Sensei;

    public NavMeshAgent PlayerAgent;
    public NavMeshAgent SenseiAgent;

    public Animator PlayerAnim;
    public Animator SenseiAnim;

    public PlayableDirector Cutscene;
    bool CallOnce;

    float nextTime;
    float nextTimeCut01;
    int CheckPoint = 0;

    float nextTimeRot;

    private void Start()
    {
        Cutscene.played += Cutscene_played;
    }

    private void Cutscene_played(PlayableDirector obj)
    {
        CheckPoint++;
        nextTime = Time.time + 7f;
        nextTimeCut01 = Time.time + 20.15f;
    }

    private void Update()
    {

        switch (CheckPoint)
        {
            case 1:
                if (Time.time >= nextTime)
                {
                    print("isWalkiing");
                    WalkToLocation();

                }
                break;
            case 2:
                if (!CallOnce)
                {
                    CallOnce = true;
                    Sensei.transform.position = new Vector3(145.675003f, 3.00300002f, 500.730011f);
                    Player.transform.position = new Vector3(144.475998f, 3.00300002f, 500.730011f);

                    SenseiAgent.SetDestination(new Vector3(146.58f, 3.00300002f, 483.519989f));
                    PlayerAgent.SetDestination(new Vector3(145.54f, 3.00300002f, 483.519989f));
                }

                ChangeCut();

                break;
            case 3:
                if (!CallOnce)
                {
                    nextTimeRot = Time.time + 1f;
                    CallOnce = true;
                }
                if (nextTimeRot > Time.time)
                {
                    RotateEachOther();
                }
                break;

        }


    }
    void RotateEachOther()
    {
                    Player.transform.LookAt(Sensei.transform);
                    Sensei.transform.LookAt(Player.transform);

    }

    void ChangeCut()
    {

        if (SenseiAgent.remainingDistance > SenseiAgent.stoppingDistance)
        {
            SenseiAnim.SetInteger("State", 1);
            PlayerAnim.SetInteger("State", 2);
        }
        else
        {
            PlayerAnim.SetInteger("State", 1);
            SenseiAnim.SetInteger("State", 0);
            CheckPoint++;
            CallOnce = false;

            //once = true;
        }
    }

    void WalkToLocation()
    {
        SenseiAgent.SetDestination(new Vector3(143.330002f, 2.32999992f, 498.290009f));
        print(SenseiAgent.remainingDistance);
        if (SenseiAgent.remainingDistance > SenseiAgent.stoppingDistance)
        {
            SenseiAnim.SetInteger("State", 1);
        }
        else
        {
            SenseiAnim.SetInteger("State", 0);

            if (Time.time >= nextTimeCut01)
                CheckPoint++;
        }
    }
}
