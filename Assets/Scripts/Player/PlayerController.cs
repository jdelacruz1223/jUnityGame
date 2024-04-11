using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool isMoving;
    private float timeToMove = 0.2f;
    private Vector2 posA, posB;
    public string direction;
    private BoxCollider2D collider;
    [SerializeField] float distance = 0.1f;
    [SerializeField] private float moveSpeed = 2f;
    
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
                    StartCoroutine(MovePlayer(Vector2.down));
                    direction = "down";
                    break;
                case "d":
                    StartCoroutine(MovePlayer(Vector2.right));
                    direction = "right";
                    break;
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                RaycastAttack();
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
        switch(direction)
        {
            case "down":
            return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, distance);
        }
            
        
    }
}
