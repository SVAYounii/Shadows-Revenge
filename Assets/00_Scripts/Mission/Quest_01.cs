using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Quest_01 : Mission
{
    public GameObject Master;
    public GameObject EnemiesList;

    public Camera CutsceneCamera;
    public GameObject FreeLookCamera;
    Camera MainCamera;

    public TextMeshProUGUI MissionText;
    public TextMeshProUGUI ObjectiveText;
    public Image Fade;
    public GameObject Canvas;
    int _amountOfEnemy;

    public PlayableDirector _directorStart;
    public PlayableDirector _directorVillage;
    public PlayableDirector _directorVillageEnd;
    public PlayableDirector _directorEnd;

    public Transform CameraPos_01End;
    float dist;




    private void Awake()
    {
        //_director = GetComponent<PlayableDirector>();
        _directorVillage.played += CutsceneHasStarted;
        _directorVillage.stopped += CutsceneHasStopped;

        _directorEnd.played += CutsceneHasStarted;
        _directorEnd.stopped += CutsceneHasStopped;

        _directorStart.played += CutsceneHasStarted;
        _directorStart.stopped += CutsceneHasStopped;

        _directorVillageEnd.played += CutsceneHasStarted;
        _directorVillageEnd.stopped += CutsceneHasStopped;

        MainCamera = Camera.main;
        Player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        movement = Player.GetComponent<ThirdPersonMovement>();
    }


    // Start is called before the first frame update
    void Start()
    {
        

        MissionTitle = "Reclaiming the village!";
        Checkpoint.Add(("Go to your Master ", Master.transform.position,true));
        Checkpoint.Add(("Go to the village ", new Vector3(55.2799988f, 0.200000003f, 358.619995f), true));
        Checkpoint.Add(("", Vector3.zero, true));
        Checkpoint.Add(("Kill the Enemies in the village ", Vector3.zero,true));
        Checkpoint.Add(("Return to the master ", Master.transform.position, true));
        Checkpoint.Add(("", Vector3.zero, true));
        MissionText.text = MissionTitle;
        _amountOfEnemy = EnemiesList.gameObject.transform.childCount;

    }

    // Update is called once per frame
    void Update()
    {
        if (Checkpoint[CurrentCheckpoint].Item2 != null)
        {
            dist = Vector3.Distance(Player.transform.position, Checkpoint[CurrentCheckpoint].Item2);
        }

        if (CurrentCheckpoint > LastCheckpoint)
        {
            NextCheckpoint();
        }


        switch (CurrentCheckpoint)
        {
            case 0:
                //if close to master start cutscene
                if (dist < 3)
                {
                    StartCutscene(_directorStart);

                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;
                break;
            case 1:
                //if close to village start cutscene
                if (dist < 3)
                {
                    CurrentCheckpoint++;
                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
            case 2:
                if (!_startCheckpoint)
                {
                    _startCheckpoint = true;
                    StartCutscene(_directorVillage);
                }


                break;
            case 3:
                if (!_startCheckpoint)
                {
                    print("called");
                    _startCheckpoint = true;
                    //StartCutsceneCamera(false);

                }

                int killed = _amountOfEnemy - EnemiesList.gameObject.transform.childCount;
                if (killed == _amountOfEnemy)
                {
                    StartCutscene(_directorVillageEnd);

                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1 + "(" + killed.ToString() + "/" + _amountOfEnemy.ToString() + ")";

                break;
            case 4:
                if (dist < 3)
                {
                    StartCutscene(_directorEnd);
                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
            case 5:
                if(!_startCheckpoint)
                {
                    _startCheckpoint = false;
                    NextMission("Mission_01", "Mission_02");
                }
                break;
        }
    }


    void NextCheckpoint()
    {
        LastCheckpoint = CurrentCheckpoint;
        _startCheckpoint = false;
    }

    //void StartCutscene(PlayableDirector timeline)
    //{

    //    timeline.Play();

    //}
   
    void StartCutsceneCamera(bool value)
    {
        MainCamera.gameObject.SetActive(!value);
        FreeLookCamera.SetActive(!value);
        CutsceneCamera.gameObject.SetActive(value);
    }

    private void CutsceneHasStopped(PlayableDirector obj)
    {
        LetPlayerMove();
        //Canvas.SetActive(true);
        //StartCutsceneCamera(false);

        CurrentCheckpoint++;
    }

    private void CutsceneHasStarted(PlayableDirector obj)
    {
        LetPlayerNotMove();
        //StartCutsceneCamera(true);
        //Canvas.SetActive(false);
    }
}
