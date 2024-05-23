using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public GameObject player;
    public float speed;
    [SerializeField] public float noticeDistance = 6f;
    // private float distance;
    Health health;
    private Rigidbody2D myRigidbody;

    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float waypointTolerance = 1f;
    private int currentWaypointIndex = 0;

    [SerializeField] public float knockbackForce = 10f;

    private SpriteRenderer sprite;
    private float oldX = 0;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        myRigidbody = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (distanceTo(player) < noticeDistance)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            //If the aggressor gets closer to the waypoint than the waypointTolerance then switch to the next waypoint
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < waypointTolerance)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }
            transform.position = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
        }
        
        //Face the aggressor in the direction that it is moving
        if (oldX > transform.position.x)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
        oldX = transform.position.x;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
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

    private float distanceTo(GameObject theObject)
    {
        float xDiff = theObject.transform.position.x - transform.position.x;
        float yDiff = theObject.transform.position.y - transform.position.y;
        float distance = Mathf.Sqrt( (xDiff * xDiff ) + (yDiff * yDiff) );

        return distance;
    }

    //from HitboxController
    void OnHit(float damage) 
    {
        Debug.Log(gameObject.name + " hit for " + damage);
        health.Damage((int)damage);
    }
    
}
