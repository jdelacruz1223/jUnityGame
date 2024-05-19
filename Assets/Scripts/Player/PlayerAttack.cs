using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Callbacks;


//using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerAttack : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackDistance = 1f;

    [SerializeField] private InputActionReference attack;
    [SerializeField] LayerMask layerMask = 9;
    [SerializeField] private int damage = 10;
    private RaycastHit2D hit;
    private Vector2 rayDir;
    private Vector2 prevDir;
    private float distance = 1.5f;
    
    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rayDir = Vector2.down;
    }
    
    void FixedUpdate()
    {    
        Vector2 origin = transform.position;

        RayDirection(playerMovement.getDirection());
        DrawRay(origin);
    }

    void DrawRay(Vector2 origin)
    {
        hit = Physics2D.Raycast(origin, rayDir, distance, layerMask);
        Debug.DrawRay(origin, rayDir * distance, Color.green);
    }

    public void RayDirection(string direction)
    {
        switch(direction)
        {
            case "up":
            rayDir = Vector2.up;
            break;
            case "down":
            rayDir = Vector2.down;
            break;
            case "left":
            rayDir = Vector2.left;
            break;
            case "right":
            rayDir = Vector2.right;
            break;
            case "default":
            rayDir = prevDir;
            break;
        }
        prevDir = rayDir;
    }

    public void Attack(bool isAttacking)
    {
        Collider2D target = hit.collider;
        Debug.Log("Attack!");
        try
        {
            if(target != null)
            {
                Health enemyHealth = target.gameObject.GetComponent<Health>();
                enemyHealth.Damage(damage);
                Debug.Log("Hit " + target.gameObject.name + " for " + damage + " damage!");
            }
            else
            {
                throw new NullReferenceException("No hit");
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogError("Null Reference exception " + e.Message);
        }
    }
}
