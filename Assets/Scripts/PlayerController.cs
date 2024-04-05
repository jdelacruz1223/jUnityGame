using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isMoving;
    private Vector2 posA, posB;
    private float timeToMove = 0.2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float inputSpeed = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Debug.Log(move.x + "." + move.y);
        if (move.y > inputSpeed && !isMoving) //W
        {
            StartCoroutine(MovePlayer(Vector2.up));
        }
        if (move.y < -inputSpeed && !isMoving) //S
        {
            StartCoroutine(MovePlayer(Vector2.down));
        }
        if (move.x < -inputSpeed && !isMoving) //A
        {
            StartCoroutine(MovePlayer(Vector2.left));
        }
        if (move.x > inputSpeed && !isMoving) //D
        {
            StartCoroutine(MovePlayer(Vector2.right));
        }
        
        //update animation
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
