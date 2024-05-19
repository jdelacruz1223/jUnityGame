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
    MovementState state;
    private Animator anim;
    private SpriteRenderer sprite;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    public void animationUpdate(string direction, bool isMoving)
    {
        //MovementState state;

        switch (direction)
        {
            case "up":
                if(isMoving)
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
                if(isMoving)
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
                if(isMoving)
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
                if(isMoving)
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

    public void attackAnimation()
    {
        anim.SetTrigger("Attack");
    }
}
