using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = 0.2f;

    private enum MovementState {idleUp, idleDown, idleSide, moveDown, moveUp, moveSide}
    private float dirX = 0f;
    private float dirY = 0f;
    private Animator anim;
    private SpriteRenderer sprite;
    private string facing;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxis("Horizontal");
        dirY = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.W) && !isMoving)
            {
                StartCoroutine(MovePlayer(Vector3.up));
                //dirY += 1f;
            }
        else if (Input.GetKey(KeyCode.S) && !isMoving)
            {
                StartCoroutine(MovePlayer(Vector3.down));
                //dirY -= 1f;
            }
        else if (Input.GetKey(KeyCode.A) && !isMoving)
            {
                StartCoroutine(MovePlayer(Vector3.left));
                //dirX -= 1f;
            }
        else if (Input.GetKey(KeyCode.D) && !isMoving)
            {
                StartCoroutine(MovePlayer(Vector3.right));
                //dirX += 1f; 
            }

        UpdateAnimationState();

    }

    private IEnumerator MovePlayer(Vector3 direction) 
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        while(elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }
    
    private void UpdateAnimationState()
    {
        //might switch case this
        MovementState state;
        if(dirY > 0f) //up
        {
            state = MovementState.moveUp;
            anim.SetBool("movingUp", true);
            facing = "up";
        }
        else if (dirY < 0f) //down
        {
            state = MovementState.moveDown;
            anim.SetBool("movingDown", true);
            facing = "down";
        }
        else if (dirX < 0f) //left
        {
            state = MovementState.moveSide;
            sprite.flipX = true;
            anim.SetBool("movingSide", true);
            facing = "left";
        }
        else if (dirX > 0f) //right
        {
            state = MovementState.moveSide;
            sprite.flipX = false;
            anim.SetBool("movingSide", true);
            facing = "right";
        }
        else
        {
            if(facing == "up")
            {
                state = MovementState.idleUp;
                anim.SetBool("movingUp", false);
                anim.SetBool("idleUp", true);
            }
            else if (facing == "right")
            {
                state = MovementState.idleSide;
                sprite.flipX = false;
                anim.SetBool("movingSide", false);
                anim.SetBool("idleSide", true);
            }
            else if (facing == "left")
            {
                state = MovementState.idleSide;
                sprite.flipX = true;
                anim.SetBool("movingSide", false);
                anim.SetBool("idleSide", true);
            }
            else if (facing == "down")
            {
                state = MovementState.idleDown;
                anim.SetBool("movingDown", false);
                anim.SetBool("idleDown", true);
            }
            else
            {
                state = MovementState.idleDown;
            }
        }

        //anim.SetInteger("state", (int)state);

    }
}
