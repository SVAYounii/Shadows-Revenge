using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour
{
    GameObject Player;
    Animator anim;
    ThirdPersonMovement movement;
    public Transform AttackPoint;
    public float AttackRange = 1f;
    public LayerMask EnemyLayer;

    bool _hasAttacked = false;
    float nextTime;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = Player.GetComponentInChildren<Animator>();
        movement = Player.GetComponent<ThirdPersonMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        bool currAnimAttack = anim.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Slash");
        if (currAnimAttack)
        {
            if (!_hasAttacked)
            {
                _hasAttacked = true;
                nextTime = Time.time + (anim.GetCurrentAnimatorStateInfo(0).length / 2);

            }
            else if (nextTime > Time.time)
            {

                Collider[] enemies = Physics.OverlapSphere(AttackPoint.position, AttackRange, EnemyLayer);
                foreach (var enemie in enemies)
                {
                    enemie.gameObject.GetComponent<IHitAble>().Hit(80);

                }
                movement.trueSpeed = movement.CrouchSpeed;

            }

        }
        if (_hasAttacked && !currAnimAttack)
        {
            _hasAttacked = false;
        }
    }


    private void OnDrawGizmos()
    {
        if (AttackPoint == null)
            return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
