using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{

    public List<Vector3> SpawnPoint = new List<Vector3>();
    public GameObject NPC;
    public Transform Parent;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < SpawnPoint.Count; i++)
        {
            Instantiate(NPC, SpawnPoint[i], Parent.rotation, Parent);

        }
    }

    
}
