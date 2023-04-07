using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOpenScreens : MonoBehaviour
{

    public string firstLevel;
    public GameObject optionsScreen; 
    public GameObject gameScreen; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        gameScreen.SetActive(true);
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
