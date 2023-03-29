using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Quest_02 : Mission
{
    public GameObject EnemiesList;

    public Camera CutsceneCamera;
    public GameObject NPCQuest;
    Camera MainCamera;

    public TextMeshProUGUI MissionText;
    public TextMeshProUGUI ObjectiveText;
    public Image Fade;
    public GameObject Canvas;
    int _amountOfEnemy;

    public PlayableDirector Director01;
    public PlayableDirector Director02;
    public PlayableDirector Director03;
    public PlayableDirector Director04;

    float dist;
    private void Awake()
    {
    
        MainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(138.457993f, 5.41369104f, 468.890015f)));
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(111.25f, 3, 427.369995f)));
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(56.7843666f, 0.181999996f, 361.622803f)));
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(32.5900002f, 0.181999996f, 307.649994f)));
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(-10.9099998f, 2.000005f, 274.670013f)));
        Checkpoint.Add(("Go to the city of Tsumi", new Vector3(12.4099998f, 2.099996f, 203.789993f)));
        Checkpoint.Add(("Find the guy that know more about the master", NPCQuest.transform.position));

        MissionText.text = MissionTitle;

        Director01.played += CutsceneHasStarted;
        Director01.stopped += CutsceneHasStopped;
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
                if(dist < 3)
                {
                    StartCutscene(Director01);
                }
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


}
