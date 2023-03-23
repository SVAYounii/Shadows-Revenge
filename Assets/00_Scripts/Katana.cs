using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Katana : MonoBehaviour
{
    GameObject Player;
    Animator anim;
    ThirdPersonMovement movement;
    public Transform AttackPoint;
    public float AttackRange = 1f;
    public LayerMask EnemyLayer;
    public AudioSource AudioSource;
    public AudioClip KatanaSound;
    public AudioClip HitSound;

    bool _hasAttacked = false;
    float nextTime;
    int hit = 0;
    // Start is called before the first frame update
    private void Awake()
    {

    }
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
                AudioSource.PlayOneShot(KatanaSound, 0.5f);
            }
            else if (nextTime > Time.time && _hasAttacked)
            {

                Collider[] enemies = Physics.OverlapSphere(AttackPoint.position, AttackRange, EnemyLayer);
                foreach (var enemie in enemies)
                {
                    enemie.gameObject.GetComponent<IHitAble>().Hit(80);
                    if (hit == 0)
                        hit = 1;
                }
                movement.trueSpeed = movement.CrouchSpeed;
                if (hit == 1)
                {
                    hit = 2;
                    AudioSource.PlayOneShot(HitSound, 0.5f);

                }
            }

        }
        if (_hasAttacked && !currAnimAttack)
        {
            _hasAttacked = false;
            hit = 0;
        }
    }


    private void OnDrawGizmos()
    {
        if (AttackPoint == null)
            return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
