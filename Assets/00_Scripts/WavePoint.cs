using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePoint : MonoBehaviour
{
    public Mission Mission;
    public GameObject Object;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mission.Checkpoint[Mission.CurrentCheckpoint].Item2 != Vector3.zero && Mission.Checkpoint[Mission.CurrentCheckpoint].Item3)
        {
            Object.SetActive(true);
            Object.transform.position = Mission.Checkpoint[Mission.CurrentCheckpoint].Item2;
            Object.transform.Rotate(new Vector3(.5f, .5f, 0));
        }
        else
        {
            Object.SetActive(false);


        }
    }
}
