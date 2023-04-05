using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Quest_02 : Mission
{
    public GameObject EnemiesList;

    public GameObject NPCQuest;

    public GameObject MissionFailed;
    Image MissionFailedImage;
    public TextMeshProUGUI MissionText;
    public TextMeshProUGUI ObjectiveText;
    public TextMeshProUGUI PressFText;
    public NPC Enemy;
    public NPC Enemy1;
    public PlayableDirector Director01;
    public PlayableDirector Director02;
    public PlayableDirector Director03;
    public PlayableDirector Director04;

    public AudioClip Grab;

    GameObject DragonBreath;

    float dist;
    private float fixedDeltaTime;

    bool _missionFailed;

    float _nextTime;


    // Start is called before the first frame update
    void Start()
    {
        DragonBreath = GameObject.FindGameObjectsWithTag("DragonBreath").FirstOrDefault();
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(138.457993f, 5.41369104f, 468.890015f), true));
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(111.25f, 3, 427.369995f), true));
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(56.7843666f, 0.181999996f, 361.622803f), true));
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(32.5900002f, 0.181999996f, 307.649994f), true));
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(-10.9099998f, 2.000005f, 274.670013f), true));
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(12.4099998f, 2.099996f, 203.789993f), true));
        Checkpoint.Add(("Find the guy that knows more about the master", NPCQuest.transform.position, true));
        Checkpoint.Add(("Find the Dragon’s breath", new Vector3(7.31699991f, -2.31699991f, 86.7639999f), true));
        Checkpoint.Add(("Find the Dragon’s breath", new Vector3(6.57999992f, -3.04500008f, 63.5849991f), true));
        Checkpoint.Add(("Find the Dragon’s breath", new Vector3(20.8770008f, -2.02999997f, 47.9510002f), true));
        Checkpoint.Add(("Find the Dragon’s breath", new Vector3(31.9909992f, -1.24699998f, 44.0219994f), true));
        Checkpoint.Add(("Get the Dragon’s breath, without being noticed!", DragonBreath.transform.position, false));
        Checkpoint.Add(("Escape and go back to the stranger", NPCQuest.transform.position, true));
        Checkpoint.Add(("", Vector3.zero, false));

        MissionText.text = MissionTitle;

        this.fixedDeltaTime = Time.fixedDeltaTime;

        MissionFailedImage = MissionFailed.GetComponent<Image>();

        Director01.played += CutsceneHasStarted;
        Director01.stopped += CutsceneHasStopped;
        Director02.played += CutsceneHasStarted;
        Director02.stopped += CutsceneHasStopped;
        Director03.played += CutsceneHasStarted;
        Director03.stopped += CutsceneHasStopped;
        Director04.played += CutsceneHasStarted;
        Director04.stopped += CutsceneHasStopped;

        PressFText.GetComponent<TextMeshProUGUI>().text = "Press <color=#fff700>F</color> to Pickup";
        Enemy.AbleToWalk = false;
    }

    private void CutsceneHasStopped(PlayableDirector obj)
    {
        LetPlayerMove();
        CurrentCheckpoint++;
    }

    private void CutsceneHasStarted(PlayableDirector obj)
    {
        LetPlayerNotMove();
    }

    // Update is called once per frame
    void Update()
    {
        ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

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
        else
            switch (CurrentCheckpoint)
            {
                case 0:
                    IfCloseGoNextCheckpoint();
                    break;
                case 1:
                    IfCloseGoNextCheckpoint();
                    break;
                case 2:
                    IfCloseGoNextCheckpoint();
                    break;
                case 3:
                    IfCloseGoNextCheckpoint();
                    break;
                case 4:
                    IfCloseGoNextCheckpoint();
                    break;
                case 5:
                    if (dist < 3)
                    {
                        StartCutscene(Director01);
                    }
                    break;
                case 6:
                    if (dist < 3)
                    {
                        StartCutscene(Director02);
                    }
                    break;
                case 7:
                    IfCloseGoNextCheckpoint();
                    break;
                case 8:
                    IfCloseGoNextCheckpoint();
                    break;
                case 9:
                    IfCloseGoNextCheckpoint();
                    break;
                case 10:
                    if (dist < 3)
                    {
                        StartCutscene(Director03);
                    }
                    break;
                case 11:
                    if (!_startCheckpoint)
                    {
                        _startCheckpoint = true;
                        Enemy.AbleToWalk = true;

                    }
                    if (Enemy.canSeePlayer || Enemy1.canSeePlayer)
                    {
                        print("Mission Failed!");
                        _nextTime = Time.time + 2.5f;
                        _missionFailed = true;
                    }
                    if (dist < 2)
                    {
                        PressFText.gameObject.SetActive(true);
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            Player.GetComponent<AudioSource>().PlayOneShot(Grab,0.5f);
                            DragonBreath.SetActive(false);
                            CurrentCheckpoint++;
                        }
                    }
                    else
                    {
                        PressFText.gameObject.SetActive(false);
                    }
                    break;
                case 12:
                    if (Enemy.canSeePlayer)
                    {
                        print("Mission Failed!");
                        _nextTime = Time.time + 2.5f;
                        _missionFailed = true;
                    }
                    PressFText.gameObject.SetActive(false);

                    if (dist < 3)
                    {
                        StartCutscene(Director04);
                    }
                    break;
                case 13:
                    Completed = true;
                    break;

            }
    }

    void NextCheckpoint()
    {
        LastCheckpoint = CurrentCheckpoint;
        _startCheckpoint = false;
    }
    void IfCloseGoNextCheckpoint()
    {
        if (dist < 3)
        {
            CurrentCheckpoint++;
        }
    }

    void Failed()
    {
        MissionFailed.SetActive(true);
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
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
        Player.transform.position = Checkpoint[10].Item2;
        CurrentCheckpoint = 11;
        Player.GetComponent<CharacterController>().enabled = true;
        Player.GetComponent<ThirdPersonMovement>().enabled = true;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        DragonBreath.SetActive(true);

        var tempColor = MissionFailedImage.color;
        tempColor.a = 0;
        MissionFailedImage.color = tempColor;
        _missionFailed = false;

    }


}
