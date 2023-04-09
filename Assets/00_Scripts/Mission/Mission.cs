using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public abstract class Mission : MonoBehaviour
{
    public string MissionTitle;
    public int CurrentCheckpoint;
    protected int LastCheckpoint;
    public List<(string, Vector3,bool)> Checkpoint = new List<(string, Vector3, bool)>();
    public bool _startCheckpoint;
    public bool Completed;

    public GameObject Player;
    public ThirdPersonMovement movement;
    private void Awake()
    {
    }
    private void Start()
    {
        LastCheckpoint = CurrentCheckpoint;
    }

    private void Update()
    {
        
    }

    protected void NextMission(string currSceneName, string newSceneName)
    {
        GameObject.FindGameObjectsWithTag("QuestLoader").FirstOrDefault().GetComponent<QuestManager>().NextMission(currSceneName, newSceneName);
    }

    public void StartCutscene(PlayableDirector timeline)
    {

        timeline.Play();

    }

    public void LetPlayerMove()
    {
        movement.enabled = true;
    }
    public void LetPlayerNotMove()
    {
        movement.enabled = false;
    }

}
