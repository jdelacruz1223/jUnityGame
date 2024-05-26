using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;

//using System.Numerics;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] public float noticeDistance = 6f;
    [SerializeField] private float waypointTolerance = 1f;
    [SerializeField] public float knockbackForce = 10f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] public int attackDelay = 1;
    [SerializeField] public float lungeForce = 1f;
    [SerializeField] private float attackRecovery = 1f;
    Health health;
    private SpriteRenderer sprite;
    public Rigidbody2D rb;
    private int currentWaypointIndex = 0;
    private float oldX = 0;
    public GameObject player;
    public float speed;
    public bool pursuingPlayer = false;
    public string currentDirection;
    public Vector2 posB;
    public string direction;
    Collider2D attackCollider;
    public GameObject attackHitbox;
    public bool isAttacking = false;
    

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        attackCollider = attackHitbox.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!isAttacking)
        {
            direction = getDirection();

            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            
            if (distanceTo(player) < noticeDistance)
            {
                ChasePlayer();
            }
            else
            {
                FollowWaypoint();
            }

            
        }
        
    }

    IEnumerator ReadyAttackCoroutine()
    {
        Debug.Log("1 sec");
        yield return new WaitForSeconds(attackDelay);
    }

    IEnumerator AttackCoroutine()
    {
        //lock movement here
        EnableCollider();
        UpdateHitboxPosition(direction);
        AttackLunge();
        
        //animation call here

        for (int i = 0; i < attackRecovery; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        DisableCollider();
        //unlock movement here
    } 

    void AttackLunge()
    {
        Vector2 posA = transform.position;
        Vector2 posB = player.transform.position;
        Vector2 targetVector = (posB - posA).normalized;

        rb.AddForce(targetVector * lungeForce, ForceMode2D.Impulse);
    }

    private void ChasePlayer()
    {
        pursuingPlayer = true;

        if (speed != 1.5)
        {
            ChangeSpeed(pursuingPlayer);
        }
        
        if(distanceTo(player) <= attackRange)
        {
            Debug.Log("Enemy Attack!");
            isAttacking = true;
            StartCoroutine(ReadyAttackCoroutine());
            StartCoroutine(AttackCoroutine());
            isAttacking = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards
            (
                this.transform.position, 
                player.transform.position, 
                speed * Time.deltaTime
            );
        }
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
                //Debug.Log("Knockback");
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

    public string getDirection()
    {
        Vector2 posA = transform.position;
        Vector2 directionVector =  posA - posB;

        posB = posA;
        
        if (directionVector.y > 0f) //up
        {
            currentDirection = "up";
        }
        else if (directionVector.y < 0f) //down
        {
            currentDirection = "down";
        }
        else if (directionVector.x < 0f) //left
        {
            currentDirection = "left";
        }
        else if (directionVector.x > 0f) //right
        {
            currentDirection = "right";
        }
        else
        {
            currentDirection = "stationary";
        }
        
        return currentDirection;
    }

    void UpdateHitboxPosition(string direction)
    {
        Quaternion rotation = Quaternion.identity;

        switch(direction)
        {
            case "up":
            rotation = Quaternion.Euler(0,0,180);
            break;
            case "down":
            rotation = Quaternion.Euler(0,0,0);
            break;
            case "left":
            rotation = Quaternion.Euler(0,0,-90);
            break;
            case "right":
            rotation = Quaternion.Euler(0,0,90);
            break;
            case "default":
            Debug.LogWarning("Invalid Direction");
            return;
        }

        attackCollider.transform.rotation = rotation;
    }

    public void EnableCollider()
    {
        if(attackCollider != null)
        {
            attackCollider.enabled = true;
        }
    }

    public void DisableCollider()
    {
        if(attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    private void FollowWaypoint()
    {
        //If the aggressor gets closer to the waypoint than the waypointTolerance then switch to the next waypoint
        try
        {
            pursuingPlayer = false;
            if (
                Vector2.Distance
                    (
                    waypoints[currentWaypointIndex].transform.position,
                    transform.position
                    ) 
                    < waypointTolerance
                )
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }
            transform.position = Vector2.MoveTowards
                (
                this.transform.position,
                waypoints[currentWaypointIndex].transform.position,
                speed * Time.deltaTime
                );
        }
        catch (IndexOutOfRangeException e)
        {

        }
    }

    private void FacePlayer()
    {
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

    private void ChangeSpeed(bool pursuingPlayer)
    {
        if(pursuingPlayer)
        {
            speed = 1.5f;
        }
        else
        {
            speed = 1f;
        }
    }


}
