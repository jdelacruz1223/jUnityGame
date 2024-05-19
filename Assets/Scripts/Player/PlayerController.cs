using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private InputAction attackAction;
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;

    [SerializeField] string direction;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float attackDuration = 1f;

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();

        attackAction = new InputAction("Attack", InputActionType.Button, "<Keyboard>/space");
        attackAction.Enable();
    }

    void OnDestroy()
    {
        attackAction.Disable();
    }

    private void Update()
    {
        if(canMove) 
        {
            direction = playerMovement.getDirection();
            playerMovement.Move();
        }
        
        playerAnimation.animationUpdate(direction, checkIfMoving());        
    }

    void OnMove(InputValue moveValue)
    {
        playerMovement.moveInput = moveValue.Get<Vector2>();
    }

    void OnAttack()
    {
        playerAnimation.attackAnimation();
        playerAttack.Attack(isAttacking);
    }

    public void LockMovement()
    {
        canMove = false;
        Debug.Log("LockMovement");
    }

    public void UnlockMovement() 
    {
        canMove = true;
        Debug.Log("UnlockMovement");
    }

    private bool checkIfMoving()
    {
        if(playerMovement.moveInput != Vector2.zero)
        {
            return isMoving = true;
        }
        else
        {
            return isMoving = false;
        }
    }

}
