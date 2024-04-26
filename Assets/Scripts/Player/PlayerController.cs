using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Threading;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    //movement logic
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public bool isMoving = false;
    [SerializeField] public string moveDir;
    [SerializeField] public string faceDir;
    [SerializeField] public float collisionOffset = 0.05f;
    Rigidbody2D rb;
    Vector2 moveInput;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    //player combat
    private PlayerAttack attackChild;
    private InputAction attackAction;
    [SerializeField] private bool canReadInput = true;
    [SerializeField] private bool isAttacking = false;

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
        attackChild = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        attackAction = new InputAction("Attack", InputActionType.Button, "<Keyboard>/space");
        attackAction.Enable();
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
            );

        if(count == 0) 
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        } 
        else
        {
            return false;
        }
    }
    
    private void FixedUpdate()
    {
        if(moveInput != Vector2.zero)
        {
            bool success = TryMove(moveInput);

            if(!success)
            {
                success = TryMove(new Vector2(moveInput.x, 0));
                if(!success)
                {
                    success = TryMove(new Vector2(0, moveInput.y));
                }
            }
        }

        if (moveInput.y > 0f) //up
        {
            moveDir = "up";
            faceDir = moveDir;
            isMoving = true;
        }
        else if (moveInput.y < 0f) //down
        {
            moveDir = "down";
            faceDir = moveDir;
            isMoving = true;
        }
        else if (moveInput.x < 0f) //left
        {
            moveDir = "left";
            faceDir = moveDir;
            isMoving = true;
        }
        else if (moveInput.x > 0f) //right
        {
            moveDir = "right";
            faceDir = moveDir;
            isMoving = true;
        }
        else
        {
            moveDir = "default";
            isMoving = false;
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
            anim.SetBool("isMoving", true);
            anim.SetInteger("state", 2);
            break;
            
            case "down":
            state = MovementState.run;
            anim.SetBool("isMoving", true);
            anim.SetInteger("state", 1);
            break;

            case "left":
            state = MovementState.run;
            anim.SetBool("isMoving", true);
            anim.SetInteger("state", 4);
            sprite.flipX = true;
            break;

            case "right":
            state = MovementState.run;
            anim.SetBool("isMoving", true);
            anim.SetInteger("state", 3);
            sprite.flipX = false;
            break;

            case "default":
            state = MovementState.idle;
            anim.SetBool("isMoving", false);
            anim.SetInteger("state", (int)state);
            break;
        }

    }
   
    void OnDestroy()
    {
        attackAction.Disable();
    }

    void OnMove(InputValue moveValue)
    {
        moveInput = moveValue.Get<Vector2>();
    }

    void OnAttack()
    {
        anim.SetTrigger("down");
        attackChild.Attack();
    }
}
