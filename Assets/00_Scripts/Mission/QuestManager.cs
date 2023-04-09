using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene("Mission_01", LoadSceneMode.Additive);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextMission(string currSceneName, string newSceneName)
    {
        SceneManager.UnloadSceneAsync(currSceneName);
        SceneManager.LoadScene(newSceneName, LoadSceneMode.Additive);
    }
}
