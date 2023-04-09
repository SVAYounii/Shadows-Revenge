using UnityEngine;
using UnityEngine.UI;

public abstract class HitAble : MonoBehaviour, IHitAble
{
    public float Health;
    public float BaseHealth;
    public Image HealthUI;
    float _nextHit;
    float _delay = 1f;
    bool ready = true;

    private void Awake()
    {
        BaseHealth = Health;

        float percent = Health / BaseHealth;
        HealthUI.fillAmount = percent;
    }
    public void Hit(int amount)
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
            }
        }

        if (HealthUI != null)
        {
            float percent = Health / BaseHealth;
            HealthUI.fillAmount = percent;
        }


    }
}
