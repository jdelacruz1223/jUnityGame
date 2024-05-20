using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public GameObject player;
    public float speed;
    [SerializeField] public float attractDistance = 6f;
    private float distance;

    [SerializeField] public float knockbackForce = 10f;

    void Start()
    {

    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance < attractDistance)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }

        
    }

    void OnHit(float damage) 
    {
        Debug.Log("Enemy hit for " + damage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerHitbox"))
        {
            Health playerHealth = other.gameObject.GetComponent<Health>();
            playerHealth.Damage(20);
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Debug.Log("Knockback");
                // Calculate knockback direction
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

                // Apply knockback force
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            } 
        }

        
    }
    
}
