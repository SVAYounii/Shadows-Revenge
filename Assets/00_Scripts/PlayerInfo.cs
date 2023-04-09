using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : HitAble
{
    public Animator PlayerAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <=0 && !IsDead)
        {
            PlayerAnim.SetTrigger("IsDead");
            IsDead = true;
        }
    }
}
