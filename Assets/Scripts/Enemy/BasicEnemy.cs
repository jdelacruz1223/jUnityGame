using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif
using UnityEngine;
using UnityEngine.Jobs;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] public float noticeDistance = 4f;
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
    public enum MovementState 
    {
        None,
        Chasing,
        Attacking,
        Patrolling
    }

    [SerializeField] public MovementState currentState;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        attackCollider = attackHitbox.GetComponent<Collider2D>(); 
            
        currentState = MovementState.Patrolling;
    }

    void Update()
    {
        DebugRay();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
        }

        if(currentState != MovementState.Attacking)
        {
            direction = GetDirection();
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < noticeDistance && distance >= attackRange)
        {
            currentState = MovementState.Chasing;
        }
        else if (distance <= attackRange)
        {
            currentState = MovementState.Attacking;
        }
        else 
        {
            currentState = MovementState.Patrolling;
        }

        HandleState();
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case MovementState.Chasing:
                ChasePlayer();
                break;
            case MovementState.Attacking:
                if(!isAttacking)
                {
                    StartCoroutine(AttackCoroutine());
                }
                break;
            case MovementState.Patrolling:
                FollowWaypoint();
                break;
            default:
                Debug.Log("Unknown State");
                break;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        Debug.Log("AttackCoroutine");
        AttackLunge();

        yield return new WaitForSeconds(attackDelay);
        rb.velocity = Vector2.zero;
        DisableCollider();

        isAttacking = false;
    }

    private void DebugRay()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);

        Debug.DrawRay(ray.origin, ray.direction * attackRange, Color.red);
    }

    private void AttackLunge()
    {
        Vector2 posA = transform.position;
        Vector2 posB = player.transform.position;
        Vector2 targetVector = (posB - posA).normalized;

        RaycastHit2D hit = Physics2D.Raycast(posA, targetVector, attackRange);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("hit");
                rb.velocity = Vector2.zero;
                rb.AddForce(targetVector * lungeForce, ForceMode2D.Impulse);

                EnableCollider();
            }
        }
    }

    private void ChasePlayer()
    {
        pursuingPlayer = true;
        speed = 1.5f;
        noticeDistance = 7f;
        
        transform.position = Vector2.MoveTowards
        (
            this.transform.position, 
            player.transform.position, 
            speed * Time.deltaTime
        );
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
    public void OnHit(float damage) 
    {
        Debug.Log(gameObject.name + " hit for " + damage);
        health.Damage((int)damage);
    }

    private string GetDirection()
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
            speed = 1f;
            noticeDistance = 4f;
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


}
