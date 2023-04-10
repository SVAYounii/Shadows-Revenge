using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Quest_01 : Mission
{
    public GameObject Master;
    public GameObject EnemiesList;

    public Camera CutsceneCamera;

    public GameObject MissionFailed;
    Image MissionFailedImage;
    public TextMeshProUGUI MissionText;
    public TextMeshProUGUI ObjectiveText;
    public Image Fade;
    public GameObject Canvas;
    int _amountOfEnemy;

    public PlayableDirector _directorMainMenu;
    public PlayableDirector _directorVillage;
    public PlayableDirector _directorVillageEnd;
    public PlayableDirector _directorEnd;
    public AudioSource MainMenuMusic;

    public Transform CameraPos_01End;
    float dist;

    PlayerInfo PlayerInfo;

    public List<Vector3> SpawnPoint = new List<Vector3>();
    public GameObject Enemy;
    public Transform Parent;

    private float fixedDeltaTime;

    bool _missionFailed;
    bool fadeout;
    float _nextTime;

    private void Awake()
    {
        //_director = GetComponent<PlayableDirector>();
        _directorVillage.played += CutsceneHasStarted;
        _directorVillage.stopped += CutsceneHasStopped;

        _directorEnd.played += CutsceneHasStarted;
        _directorEnd.stopped += CutsceneHasStopped;

        _directorMainMenu.played += CutsceneHasStarted;
        _directorMainMenu.stopped += CutsceneHasStopped;

        _directorVillageEnd.played += CutsceneHasStarted;
        _directorVillageEnd.stopped += CutsceneHasStopped;


    }


    // Start is called before the first frame update
    void Start()
    {


        MissionTitle = "Reclaiming the village!";
        Checkpoint.Add(("Go to your Master ", Master.transform.position, false));
        Checkpoint.Add(("Go to the village ", new Vector3(144.858002f, 3.49000001f, 479.731995f), true));
        Checkpoint.Add(("Go to the village ", new Vector3(130.936996f, 3.49000001f, 452.756989f), true));
        Checkpoint.Add(("Go to the village ", new Vector3(105.382004f, 1.37f, 419.69101f), true));
        Checkpoint.Add(("Go to the village ", new Vector3(55.2799988f, 0.200000003f, 358.619995f), true));
        Checkpoint.Add(("", Vector3.zero, true));
        Checkpoint.Add(("Kill the Enemies in the village ", Vector3.zero, true));
        Checkpoint.Add(("Return to the master ", Master.transform.position, true));
        Checkpoint.Add(("", Vector3.zero, true));
        MissionText.text = MissionTitle;
        PlayerInfo = Player.GetComponent<PlayerInfo>();
        MissionFailedImage = MissionFailed.GetComponent<Image>();

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

        if (_missionFailed)
        {
            Failed();
        }
        if (fadeout)
        {
            MainMenuMusic.volume -= Time.deltaTime * 0.5f;
            if (MainMenuMusic.volume <= 0)
            {
                fadeout = false;
            }
        }
        switch (CurrentCheckpoint)
        {
            case 0:
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
                //if close to village start cutscene
                if (dist < 3)
                {
                    CurrentCheckpoint++;
                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
            case 3:
                //if close to village start cutscene
                if (dist < 3)
                {
                    CurrentCheckpoint++;
                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
            case 4:
                //if close to village start cutscene
                if (!_startCheckpoint)
                {
                    _startCheckpoint = true;

                    for (int i = 0; i < SpawnPoint.Count; i++)
                    {
                        Instantiate(Enemy, SpawnPoint[i], Parent.rotation, Parent);

                    }
                }
                if (dist < 3)
                {
                    CurrentCheckpoint++;
                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
            case 5:
                if (!_startCheckpoint)
                {
                    _startCheckpoint = true;
                    StartCutscene(_directorVillage);
                }


                break;
            case 6:
                if (!_startCheckpoint)
                {
                    print("called");
                    _startCheckpoint = true;
                    //StartCutsceneCamera(false);
                    _amountOfEnemy = EnemiesList.gameObject.transform.childCount;

                }
                if (PlayerInfo.Health <= 0 && !_missionFailed)
                {
                    _nextTime = Time.time + 2.5f;
                    _missionFailed = true;
                }
                int killed = _amountOfEnemy - EnemiesList.gameObject.transform.childCount;
                if (killed == _amountOfEnemy)
                {
                    StartCutscene(_directorVillageEnd);

                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1 + " (" + killed.ToString() + "/" + _amountOfEnemy.ToString() + ")";

                break;
            case 7:
                if (!_startCheckpoint)
                {
                    _startCheckpoint = true;
                    Master.GetComponent<NavMeshAgent>().enabled = false;
                    Master.transform.position = new Vector3(152.490997f, 2.66899991f, 486.756989f);
                    Master.transform.LookAt(new Vector3(150.388f, 3.31399989f, 486.812988f));
                    Checkpoint[CurrentCheckpoint] = (Checkpoint[CurrentCheckpoint].Item1, Master.transform.position, true);
                }
                if (dist < 3)
                {
                    StartCutscene(_directorEnd);
                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
            case 8:
                if (!_startCheckpoint)
                {
                    _startCheckpoint = false;
                    NextMission("Mission_01", "Mission_02");
                }
                break;
        }
    }
    void Failed()
    {
        MissionFailed.SetActive(true);
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        var tempColor = MissionFailedImage.color;
        if (tempColor.a < 1)
        {
            tempColor.a += Time.deltaTime * 0.5f;
            MissionFailedImage.color = tempColor;
        }

        if (Time.time > _nextTime)
        {
            ResetMission();
        }
    }
    void ResetMission()
    {
        MissionFailed.SetActive(false);

        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<ThirdPersonMovement>().enabled = false;
        Player.transform.position = Checkpoint[4].Item2;
        Player.GetComponent<CharacterController>().enabled = true;
        Player.GetComponent<ThirdPersonMovement>().enabled = true;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02F;

        var tempColor = MissionFailedImage.color;
        tempColor.a = 0;
        MissionFailedImage.color = tempColor;
        _missionFailed = false;
        PlayerInfo.Health = PlayerInfo.BaseHealth;
        PlayerInfo.IsDead = false;
        LastCheckpoint = CurrentCheckpoint;
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            Parent.transform.GetChild(i).gameObject.GetComponent<SwordEnemy>().EnemyState = SwordEnemy.State.Roaming;
            Parent.transform.GetChild(i).gameObject.GetComponent<SwordEnemy>().canSeePlayer = false;
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        if (CurrentCheckpoint == 0)
        {
            fadeout = true;
        }
        //StartCutsceneCamera(true);
        //Canvas.SetActive(false);
    }
}
