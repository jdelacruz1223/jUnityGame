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
    [SerializeField] private bool isMoving;
    private Vector2 posA;
    private Vector2 posB;
    [SerializeField] public string moveDir;
    [SerializeField] public string faceDir;
    [SerializeField] private float moveSpeed = 5f;

    //player combat
    private PlayerAttack attackParent;
    private InputAction attackAction;

    //animation
    public enum MovementState 
    {
        idle,
        run,
        attack
    }
    private Animator anim;
    private SpriteRenderer sprite;
    



    void Start()
    {
        attackParent = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        isMoving = false;

        attackAction = new InputAction("Attack", InputActionType.Button, "<Keyboard>/space");
        attackAction.Enable();
        attackAction.performed += ctx => attackParent.Attack();
    }
    
    void FixedUpdate()
    {
        Vector2 move = moveInput.normalized * moveSpeed * Time.fixedDeltaTime;
        posA = transform.position;
        posB = rb.position + move;
        
        // if(!isMoving)
        // {
        //     isMoving = true;
        //     rb.MovePosition(posB);
        //     isMoving = false;
        // }

        rb.MovePosition(posB);

        if(moveInput.y > 0f) //up
        {
            moveDir = "up";
            faceDir = moveDir;
        }
        else if(moveInput.y < 0f) //down
        {
            moveDir = "down";
            faceDir = moveDir;
        }
        else if(moveInput.x < 0f) //left
        {
            moveDir = "left";
            faceDir = moveDir;
        }
        else if(moveInput.x > 0f) //right
        {
            moveDir = "right";
            faceDir = moveDir;
        }
        else
        {
            moveDir = "default";
        }
        
        AnimationUpdate();
    }

    void AnimationUpdate()
    {
        MovementState state;

        switch (moveDir)
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

   
    //         case "up":
    //         state = MovementState.attack;
    //         anim.SetInteger("state", 6);
    //         break;
            
    //         case "down":
    //         state = MovementState.attack;
    //         anim.SetInteger("state", 5);
    //         break;

    //         case "left":
    //         state = MovementState.attack;
    //         anim.SetInteger("state", 8);
    //         sprite.flipX = true;
    //         break;

    //         case "right":
    //         state = MovementState.attack;
    //         anim.SetInteger("state", 7);
    //         sprite.flipX = false;
    //         break;

    //         case "default":
    //         state = MovementState.idle;
    //         anim.SetInteger("state", (int)state);
    //         break;

    void OnDestroy()
    {
        attackAction.Disable();
    }

    void OnMove(InputValue moveValue)
    {
        moveInput = moveValue.Get<Vector2>();
    }
}
