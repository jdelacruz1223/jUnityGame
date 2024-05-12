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
    [SerializeField] private InputActionReference attack;
    [SerializeField] LayerMask layerMask = 9;
    [SerializeField] private int damage = 10;
    private RaycastHit2D hit;
    private Vector2 direction;
    private Vector2 prevDir;
    private float distance = 1f;
    private PlayerAnimation animChild;

    void Start()
    {
        animChild = GetComponent<PlayerAnimation>();
        direction = Vector2.down;
    }
    
    void FixedUpdate()
    {    
        Vector2 origin = transform.position;

        RayDirection();
        DrawRay(origin);

    }

    void DrawRay(Vector2 origin)
    {
        hit = Physics2D.Raycast(origin, direction, distance, layerMask);
        Debug.DrawRay(origin, direction * distance, Color.green);
    }

    public void RayDirection()
    {
        switch(DataManager.me.faceDir)
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
    }

    public void Attack()
    {
        Debug.Log("Attack!");

        Collider2D target = hit.collider;
        
        animChild.animationUpdate();
        try
        {
            if(target != null)
            {
                Health health = target.gameObject.GetComponent<Health>();
                health.Damage(damage);
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
        finally
        {
            DataManager.me.canMove = true;
        }
        
        
        
    }
}
