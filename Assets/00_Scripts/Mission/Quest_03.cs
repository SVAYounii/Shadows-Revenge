using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Quest_03 : Mission
{
    public GameObject EnemiesList;

    public GameObject NPCQuest;

    public GameObject MissionFailed;
    Image MissionFailedImage;
    public TextMeshProUGUI MissionText;
    public TextMeshProUGUI ObjectiveText;
    public GameObject NPC;
    public PlayableDirector Director01;
    public PlayableDirector Director02;
    public PlayableDirector Director03;

    public List<Vector3> EnemySpawn = new List<Vector3>();
    public GameObject Enemy;
    public GameObject EnemyParent;

    PlayerInfo PlayerInfo;
    int _amountOfEnemy;

    float dist;
    private float fixedDeltaTime;

    bool _missionFailed;

    float _nextTime;
    // Start is called before the first frame update
    void Start()
    {
        Checkpoint.Add(("Find the guy at the nearby farm", NPC.transform.position, true));
        Checkpoint.Add(("Go to the house", new Vector3(50.6850014f, 0.598999977f, 55.8339119f), true));
        Checkpoint.Add(("Go to the house", new Vector3(54.7459984f, 2.61999989f, 76.2190018f), true));
        Checkpoint.Add(("Go to the house", new Vector3(48.9000015f, 1.90900004f, 90.2799988f), true));
        Checkpoint.Add(("Defeat the enemies", Vector3.zero, false));
        Checkpoint.Add(("Go back to the guy", NPC.transform.position, true));
        Checkpoint.Add(("", Vector3.zero, false));


        Director01.played += CutsceneHasStarted;
        Director01.stopped += CutsceneHasStopped;

        Director02.played += CutsceneHasStarted;
        Director02.stopped += CutsceneHasStopped;

        Director03.played += CutsceneHasStarted;
        Director03.stopped += CutsceneHasStopped;
        PlayerInfo = Player.GetComponent<PlayerInfo>();
        MissionFailedImage = MissionFailed.GetComponent<Image>();

        this.fixedDeltaTime = Time.fixedDeltaTime;

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

        switch (CurrentCheckpoint)
        {
            case 0:
                if (dist < 3)
                {
                    StartCutscene(Director01);
                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
            case 1:
                IfCloseGoNextCheckpoint();
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
            case 2:
                if (!_startCheckpoint)
                {
                    _startCheckpoint = true;
                    for (int i = 0; i < EnemySpawn.Count; i++)
                    {
                        Instantiate(Enemy, EnemySpawn[i], EnemyParent.transform.rotation, EnemyParent.transform);
                    }
                }
                IfCloseGoNextCheckpoint();
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
            case 3:

                if (dist < 3)
                {
                    _amountOfEnemy = EnemiesList.gameObject.transform.childCount;
                    StartCutscene(Director02);
                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;

                break;
            case 4:

                if (PlayerInfo.Health <= 0 && !_missionFailed)
                {
                    _nextTime = Time.time + 2.5f;
                    _missionFailed = true;
                }
                int killed = _amountOfEnemy - EnemiesList.gameObject.transform.childCount;
                if (killed == _amountOfEnemy)
                {
                    CurrentCheckpoint++;
                }
                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1 + " (" + killed.ToString() + "/" + _amountOfEnemy.ToString() + ")";


                break;
            case 5:
                if (dist < 3)
                {
                    StartCutscene(Director03);
                }

                ObjectiveText.text = Checkpoint[CurrentCheckpoint].Item1;
                break;
            case 6:
                ObjectiveText.text = "";

                break;

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
        Player.transform.position = Checkpoint[2].Item2;
        CurrentCheckpoint = 4;
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

        for (int i = 0; i < EnemyParent.transform.childCount; i++)
        {
            EnemyParent.transform.GetChild(i).gameObject.GetComponent<SwordEnemy>().EnemyState = SwordEnemy.State.Roaming;
            EnemyParent.transform.GetChild(i).gameObject.GetComponent<SwordEnemy>().canSeePlayer = false;
        }

    }

    void IfCloseGoNextCheckpoint()
    {
        if (dist < 3)
        {
            CurrentCheckpoint++;
        }
    }
    void NextCheckpoint()
    {
        LastCheckpoint = CurrentCheckpoint;
        _startCheckpoint = false;
    }
    private void CutsceneHasStopped(PlayableDirector obj)
    {
        LetPlayerMove();
        CurrentCheckpoint++;
    }

    private void CutsceneHasStarted(PlayableDirector obj)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        LetPlayerNotMove();
    }
}
