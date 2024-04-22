using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] public int MAX_HEALTH = 100;
    [SerializeField] private int health;

    void Start()
    {
        health = MAX_HEALTH;
    }
    
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     Damage(10);
        // }

        // if (Input.GetKeyDown(KeyCode.H))
        // {
        //     Heal(10);
        // }
    }

    public void Damage(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        this.health -= amount;

        if(health <= 0)
        {
            Die();
        }
    }

    void Heal(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Healing");
        }

        bool healthExcessive = health + amount > MAX_HEALTH;

        if (healthExcessive)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " dead");
        if(gameObject.name == "Player")
            restartLevel();
        else
            Destroy(gameObject);
    }

    private void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
