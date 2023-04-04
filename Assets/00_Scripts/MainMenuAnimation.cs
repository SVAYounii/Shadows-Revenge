using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        
    }

    public void StandUp()
    {
        print("Standing");
        anim.SetBool("StandingUp", true);
    }
}
