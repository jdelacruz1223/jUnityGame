using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private float moveX;
    private float moveY;
    private string direction;
    [SerializeField] private float moveSpeed = 5f;


    //animation
    private enum MovementState 
    {
        idleDown,
        runDown
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
        moveX = Input.GetAxisRaw("Horitzontal"); //remove
        moveY = Input.GetAxisRaw("Vertical"); //remove
        //find some other way to record direction, this input throws errors

        if (moveY < 0f)
        {
            direction = "down";
        }

        if(!isMoving)
        {
            isMoving = true;
            rb.MovePosition(rb.position + move);
            isMoving = false;
        }

        AnimationUpdate();
    }

    void AnimationUpdate()
    {
        MovementState state;

        switch (direction)
        {
            case "down":
            state = MovementState.runDown;
            anim.SetBool("runDown", true);
            break;
            case "default":
            break;
        }
        

        
    }
}
