using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    //movement logic
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private Vector2 moveInput;
    private bool isMoving;
    private Vector2 posA;
    private Vector2 posB;
    private string direction;
    [SerializeField] private float moveSpeed = 5f;


    //animation
    private enum MovementState 
    {
        idle,
        run
    }
    private Animator anim;
    private SpriteRenderer sprite;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        isMoving = false;
    }

    void OnMove(InputValue moveValue)
    {
        moveInput = moveValue.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector2 move = moveInput.normalized * moveSpeed * Time.fixedDeltaTime;
        posA = transform.position;
        posB = rb.position + move;
        
        if(!isMoving)
        {
            isMoving = true;
            rb.MovePosition(rb.position + move);
            isMoving = false;
        }

        if(moveInput.y > 0f) //up
        {
            direction = "up";
        }
        else if(moveInput.y < 0f) //down
        {
            direction = "down";
        }
        else if(moveInput.x < 0f) //left
        {
            direction = "left";
        }
        else if(moveInput.x > 0f) //right
        {
            direction = "right";
        }
        else
        {
            direction = "default";
        }
        
        AnimationUpdate();
    }

    void AnimationUpdate()
    {
        MovementState state;

        switch (direction)
        {
            case "up":
            state = MovementState.run;
            anim.SetInteger("state", 2);
            break;
            
            case "down":
            state = MovementState.run;
            anim.SetInteger("state", 1);
            break;

            case "left":
            state = MovementState.run;
            anim.SetInteger("state", 4);
            sprite.flipX = true;
            break;

            case "right":
            state = MovementState.run;
            anim.SetInteger("state", 3);
            sprite.flipX = false;
            break;

            case "default":
            state = MovementState.idle;
            anim.SetInteger("state", (int)state);
            break;
        }
        
    }
}
