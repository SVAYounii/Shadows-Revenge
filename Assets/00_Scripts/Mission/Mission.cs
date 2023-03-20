using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    public string MissionTitle;
    public int CurrentCheckpoint;
    protected int LastCheckpoint;
    public List<(string, Vector3)> Checkpoint = new List<(string, Vector3)>();
    public bool _startCheckpoint;

    private void Start()
    {
        LastCheckpoint = CurrentCheckpoint;
    }

    private void Update()
    {
        
    }

   

}
