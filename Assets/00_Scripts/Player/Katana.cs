using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour
{
    GameObject Player;
    Animator anim;

    public Transform AttackPoint;
    public float AttackRange = 1f;
    public LayerMask EnemyLayer;

    bool readyToHit = true;
    float nextTime;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = Player.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Slash"))
        {

            //print(anim.GetCurrentAnimatorStateInfo(0).length);
            Collider[] enemies = Physics.OverlapSphere(AttackPoint.position, AttackRange, EnemyLayer);
            foreach (var enemie in enemies)
            {
                enemie.gameObject.GetComponent<IHitAble>().Hit(80);

            }
        }
    }


    private void OnDrawGizmos()
    {
        if (AttackPoint == null)
            return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
