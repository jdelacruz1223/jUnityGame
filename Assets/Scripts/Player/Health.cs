using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int MAX_HEALTH = 100;

    void Start()
    {
        Debug.Log(gameObject.name);
    }

    void Update()
    {
        //debug
        if(Input.GetKeyDown(KeyCode.G))
        {
            Damage(10);
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            Heal(10);
        }
        
    }

    public void Damage(int amount)
    {
       if(amount < 0)
       {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
       }
        
        this.health -= amount;
        Debug.Log("Ouch");

        if(health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if(wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
    }

    private void Die()
    {
        Debug.Log("I am Dead!");
        Destroy(gameObject);
    }
}
