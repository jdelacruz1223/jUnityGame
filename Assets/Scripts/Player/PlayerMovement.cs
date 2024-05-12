using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAnimation animChild;
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float collisionOffset = 0.05f;
    public Vector2 moveInput;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animChild = GetComponent<PlayerAnimation>();
    }

    public void movement()
    {
        if(DataManager.me.canMove)
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
        }
        
        recordDirection();
        animChild.animationUpdate();
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

    private void recordDirection()
    {
        if (moveInput.y > 0f) //up
        {
            DataManager.me.moveDir = "up";
            DataManager.me.faceDir = DataManager.me.moveDir;
            DataManager.me.isMoving = true;
        }
        else if (moveInput.y < 0f) //down
        {
            DataManager.me.moveDir = "down";
            DataManager.me.faceDir = DataManager.me.moveDir;
            DataManager.me.isMoving = true;
        }
        else if (moveInput.x < 0f) //left
        {
            DataManager.me.moveDir = "left";
            DataManager.me.faceDir = DataManager.me.moveDir;
            DataManager.me.isMoving = true;
        }
        else if (moveInput.x > 0f) //right
        {
            DataManager.me.moveDir = "right";
            DataManager.me.faceDir = DataManager.me.moveDir;
            DataManager.me.isMoving = true;
        }
        else
        {
            DataManager.me.moveDir = "default";
            DataManager.me.isMoving = false;
        }
    }

    public void LockMovement()
    {
        DataManager.me.canMove = false;
    }

    public void UnlockMovement() 
    {
        DataManager.me.canMove = true;
    }
}
