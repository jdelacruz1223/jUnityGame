using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool isMoving;
    private float timeToMove = 0.2f;
    private Vector2 posA, posB;
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
                    break;
                case "a":
                    StartCoroutine(MovePlayer(Vector2.left));
                    break;
                case "s":
                    StartCoroutine(MovePlayer(Vector2.down));
                    break;
                case "d":
                    StartCoroutine(MovePlayer(Vector2.right));
                    break;
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
}
