using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] public int MAX_HEALTH = 100;
    [SerializeField] public int health;
    [SerializeField] private GameObject ItemToSpawn;
    [SerializeField] public float dropChance = 50f;
    private BasicEnemy enemy;
    

    void Start()
    {
        enemy = GetComponent<BasicEnemy>();
        health = MAX_HEALTH;
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
        if(gameObject.tag == "Player")
        {
            restartLevel();
        }  
        else
        {
            dropBomb();
            Destroy(gameObject);
        }
            
    }

    private void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void dropBomb()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= dropChance)
        {
            Instantiate
            (
                ItemToSpawn,
                enemy.transform.position,
                Quaternion.identity
            );
        }
    }

    
}
