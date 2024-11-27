using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CEHealth
{

    private float maxHealth;
    public float health = 100.0f;
    public UnityEvent OnDeath = new UnityEvent();

    public UnityEvent<CEHealth> OnHealthChanged = new(); 

    public CEHealth(float health)
    {
        this.health = health;
        this.maxHealth = health;
    }
    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            OnHealthChanged.Invoke(this);
            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }
    }

    public void UpdateMaxHealth(float newMaxHealth){
        maxHealth = newMaxHealth;
        health = maxHealth;
        OnHealthChanged.Invoke(this);
    }

    public void ResetHealth(){
        health = maxHealth;
        OnHealthChanged.Invoke(this);
    }

    public float NormalizedHealth { get { return health / maxHealth; } }

    private void Die()
    {
        OnDeath.Invoke();
    }
}