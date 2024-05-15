using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum MovementState 
    {
        idle,
        run,
        attack
    }
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerMovement moveChild;
    
    void Start()
    {
        moveChild = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void attackAnimation()
    {
        //Debug.Log("attackAnim");
        MovementState state;

        // switch (DataManager.me.faceDir)
        // {
        //     case "up":
        //     state = MovementState.attack;
        //     anim.SetTrigger("Attack");
        //     break;
        //     case "down":
        //     state = MovementState.attack;
        //     anim.SetTrigger("Attack");
        //     break;
        //     case "left":
        //     state = MovementState.attack;
        //     anim.SetTrigger("Attack");
        //     break;
        //     case "right":
        //     state = MovementState.attack;
        //     anim.SetTrigger("Attack");
        //     break;
        //     case "default":
        //     break;
        // }
        state = MovementState.attack;
        anim.SetTrigger("Attack");
    }
    public void animationUpdate()
    {
        MovementState state;

        if(!DataManager.me.canMove) return;

        switch (DataManager.me.faceDir)
        {
            case "up":
                if(DataManager.me.isMoving)
                {   
                    state = MovementState.run;
                    anim.SetBool("isMoving", true);
                    anim.SetInteger("state", 2);
                }
                else
                {
                    state = MovementState.idle;
                    anim.SetBool("isMoving", false);
                    anim.SetInteger("state", 0);
                }
            break;
            
            case "down":
                if(DataManager.me.isMoving)
                {
                    state = MovementState.run;
                    anim.SetBool("isMoving", true);
                    anim.SetInteger("state", 1);
                }
                else
                {
                    state = MovementState.idle;
                    anim.SetBool("isMoving", false);
                    anim.SetInteger("state", 0);
                }
            break;

            case "left":
                sprite.flipX = true;
                if(DataManager.me.isMoving)
                {
                    state = MovementState.run;
                    anim.SetBool("isMoving", true);
                    anim.SetInteger("state", 4);
                }
                else
                {
                    state = MovementState.idle;
                    anim.SetBool("isMoving", false);
                    anim.SetInteger("state", 0);
                }
            break;

            case "right":
                sprite.flipX = false;
                if(DataManager.me.isMoving)
                {
                    state = MovementState.run;
                    anim.SetBool("isMoving", true);
                    anim.SetInteger("state", 3); 
                }
                else 
                {
                    state = MovementState.idle;
                    anim.SetBool("isMoving", false);
                    anim.SetInteger("state", 0);
                }
            break;

            case "default":
            break;
        }
    }
}
