using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitAble : MonoBehaviour, IHitAble
{
    public float Health;
    public float BaseHealth;
    float _nextHit;
    float _delay = 1f;
    bool ready = true;
    private void Awake()
    {
        BaseHealth = Health;
    }
    public int Hit(int amount)
    {
        if (!ready)
        {
            _nextHit = Time.time + _delay;
            ready = true;
        }
        else if (Time.time > _nextHit)
        {

            ready = false;

            Health -= amount;
            print("I got hit");
            if (Health <= 0)
            {
                Destroy(this.gameObject);
                return Random.Range(40, 80);
            }
        }
        return 0;

    }
}
