using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float collisionOffset = 0.05f;
    private Rigidbody2D rb;
    public Vector2 moveInput;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    string currentDirection;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move()
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

    public string getDirection()
    {
        if (moveInput.y > 0f) //up
        {
            return currentDirection = "up";
        }
        else if (moveInput.y < 0f) //down
        {
            return currentDirection = "down";
        }
        else if (moveInput.x < 0f) //left
        {
            return currentDirection = "left";
        }
        else if (moveInput.x > 0f) //right
        {
            return currentDirection = "right";
        }
        else
        {
            return currentDirection;
        }
    }
}
