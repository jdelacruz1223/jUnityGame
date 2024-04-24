using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

//using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerAttack : MonoBehaviour
{
    //attack logic
    [SerializeField] private InputActionReference attack;
    [SerializeField] LayerMask layerMask = 9;
    [SerializeField] private int damage = 10;
    RaycastHit2D hit;
    Vector2 direction;
    Vector2 prevDir;
    float distance = 1f;
    string playerDirection;
    private PlayerController playerParent;

    //animation
    //private PlayerController.MovementState state;
    private Animator anim;
    private SpriteRenderer sprite;

    void Start()
    {
        //state = GetComponent<PlayerController.MovementState>();
        playerParent = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        playerParent.canReadInput = true;
    }
    
    void Update()
    {    
        playerDirection = GetComponent<PlayerController>().direction;
        Vector2 origin = transform.position;

        switch(playerDirection)
        {
            case "up":
            direction = Vector2.up;
            break;
            case "down":
            direction = Vector2.down;
            break;
            case "left":
            direction = Vector2.left;
            break;
            case "right":
            direction = Vector2.right;
            break;
            case "default":
            direction = prevDir;
            break;
        }

        prevDir = direction;

        hit = Physics2D.Raycast(origin, direction, distance, layerMask);
        Debug.DrawRay(origin, direction * distance, Color.green);

    }
    
    public void Attack()
    {
        Debug.Log("Attack!");
        if (playerParent.canReadInput)
        StartCoroutine(playerParent.AttackCoroutine());

        if(hit.collider != null)
        {
            Debug.Log("Hit Object: " + hit.collider.gameObject.name);
            Health health = hit.collider.gameObject.GetComponent<Health>();
            health.Damage(damage);
        }
        else
        {
            throw new NullReferenceException("No hit");
            //Debug.Log("No Hit");
        }
        
    }
}
