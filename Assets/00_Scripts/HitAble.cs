using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitAble : MonoBehaviour, IHitAble
{
    public int Health;

    public void Hit(int amount)
    {
        Health -= amount;
        if (Health <= 0) Destroy(this.gameObject);

    }
}
