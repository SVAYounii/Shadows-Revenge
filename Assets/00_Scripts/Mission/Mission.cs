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
    public List<(string, Vector3)> Checkpoint = new List<(string, Vector3)>();
    public bool _startCheckpoint;
    public bool Completed;

    private void Start()
    {
        LastCheckpoint = CurrentCheckpoint;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
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

}
