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
    public PlayableDirector Director04;


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
        Checkpoint.Add(("Defeat the enemies", new Vector3(48.9000015f, 1.90900004f, 90.2799988f), true));

        Director01.played += CutsceneHasStarted;
        Director01.stopped += CutsceneHasStopped;
        Director02.played += CutsceneHasStarted;
        Director02.stopped += CutsceneHasStopped;
        Director03.played += CutsceneHasStarted;
        Director03.stopped += CutsceneHasStopped;
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
                //Start Cutscene
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
                //StartCutscene
                break;
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
        LetPlayerNotMove();
    }
}
