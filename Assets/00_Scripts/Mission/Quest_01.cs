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
    GameObject Player;
    public GameObject EnemiesList;
    ThirdPersonMovement movement;

    public Camera CutsceneCamera;
    public GameObject FreeLookCamera;
    Camera MainCamera;

    public TextMeshProUGUI MissionText;
    public TextMeshProUGUI ObjectiveText;
    public Image Fade;
    public GameObject Canvas;
    int _amountOfEnemy;

    public PlayableDirector _director;
    public PlayableDirector _directorEnd;

    public Transform CameraPos_01End;
    float dist;



    private void Awake()
    {
        //_director = GetComponent<PlayableDirector>();
        _director.played += CutsceneHasStarted;
        _director.stopped += CutsceneHasStopped;

        _directorEnd.played += CutsceneHasStarted;
        _directorEnd.stopped += CutsceneHasStopped;

        MainCamera = Camera.main;
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MissionTitle = "Reclaiming the village!";
        Checkpoint.Add(("Go to the village ", new Vector3(55.2799988f, 0.200000003f, 358.619995f)));
        Checkpoint.Add(("", Vector3.zero));
        Checkpoint.Add(("Kill the Enemies in the village ", Vector3.zero));
        Checkpoint.Add(("Return to the master ", Vector3.zero));
        CurrentCheckpoint = 0;
        MissionText.text = MissionTitle;
        _amountOfEnemy = EnemiesList.gameObject.transform.childCount;
        Player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        movement = Player.GetComponent<ThirdPersonMovement>();
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
                if (dist < 3)
                {
                    CurrentCheckpoint++;
                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
            case 1:
                if (!_startCheckpoint)
                {
                    _startCheckpoint = true;
                    StartCutscene(_director);
                    //StartCutsceneCamera(true);
                    //CutsceneCamera.transform.position = Checkpoint[CurrentCheckpoint].Item2;
                    //CutsceneCamera.transform.rotation = CameraPos_01End.rotation;
                }

                //Lerp(CutsceneCamera.transform.position, CameraPos_01End.position, CutsceneCamera.transform);

                break;
            case 2:
                if (!_startCheckpoint)
                {
                    print("called");
                    _startCheckpoint = true;
                    //StartCutsceneCamera(false);

                }

                int killed = _amountOfEnemy - EnemiesList.gameObject.transform.childCount;
                if (killed == _amountOfEnemy)
                {
                    StartCutscene(_directorEnd);

                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1 + "(" + killed.ToString() + "/" + _amountOfEnemy.ToString() + ")";

                break;
            case 3:
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
        }
    }


    void NextCheckpoint()
    {
        LastCheckpoint = CurrentCheckpoint;
        _startCheckpoint = false;
    }
    void Lerp(Vector3 curr, Vector3 end, Transform obj)
    {
        if (Vector3.Distance(obj.position, end) > 0.05f)
        {
            obj.position = Vector3.Slerp(curr, end, 0.01f);
        }
        else
        {
            CurrentCheckpoint++;
        }

    }

    void StartCutscene(PlayableDirector timeline)
    {

        timeline.Play();

    }
    void LetPlayerMove()
    {
        movement.enabled = true;
    }
    void LetPlayerNotMove()
    {
        movement.enabled = false;
    }
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
