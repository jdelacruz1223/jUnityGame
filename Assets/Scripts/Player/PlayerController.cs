using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool isMoving;
    private float timeToMove = 0.2f;
    private Vector2 posA, posB;
    public string direction;
    private BoxCollider2D collider;
    [SerializeField] float distance = 0.5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private LayerMask Enemy;
    //RaycastHit2D hit;
    //Vector2 currentDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        //isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        var input = Input.inputString;
        while(isMoving)
        return;

        if(!string.IsNullOrEmpty(input))
        {
            Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //Debug.Log("Pressed char: " + Input.inputString);
            //Debug.Log("Pressed char: " + input);
            switch(input)
            {
                case "w":
                    StartCoroutine(MovePlayer(Vector2.up));
                    direction = "up";
                    break;
                case "a":
                    StartCoroutine(MovePlayer(Vector2.left));
                    direction = "left";
                    break;
                case "s":
                    //currentDirection = Vector2.down;
                    StartCoroutine(MovePlayer(Vector2.down));
                    direction = "down";
                    break;
                case "d":
                    StartCoroutine(MovePlayer(Vector2.right));
                    direction = "right";
                    break;
            }

            if(Input.GetKeyDown(KeyCode.Space) && RaycastAttack())
            {
                Debug.Log("Attack Success!");
            }
        }
    }
    private IEnumerator MovePlayer(Vector2 direction)
    {
        isMoving = true;
        float elapsedTime = 0;

        //point A to point B
        posA = transform.position;
        posB = posA + direction;

        while(elapsedTime < timeToMove)
        {
            transform.position = Vector2.Lerp(posA, posB, (elapsedTime / timeToMove) * moveSpeed);
            elapsedTime += Time.deltaTime;
            
            yield return 0;
        }
        transform.position = posB;
        isMoving = false;
    }

    private bool RaycastAttack()
    {
        //Debug.Log("Cast!");
        switch(direction)
        {
            case "down":
            Debug.Log("Casted Down!");
            return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, distance, Enemy);
            default:
            return false;
        }        
    }

    // private void FixedUpdate()
    // {
    //     hit = Physics2D.Raycast(transform.position, currentDirection, distance);
    //     if(hit.collider != null)
    //     {
    //         Debug.DrawRay(transform.position, hit.point, Color.white);
    //         Debug.Log("Enemy hit");
    //     }
    //     else
    //     {
    //         Debug.DrawRay(transform.position, transform.position + transform.right * distance, Color.black);
    //     }
    // }
}
