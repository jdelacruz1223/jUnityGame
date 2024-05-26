using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    private InputAction attackAction;
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;
    
    [SerializeField] private bool canMove = true;
    [SerializeField] private float attackDelay = 1f;
    public string direction;

    public GameObject swordHitbox;
    Collider2D swordCollider;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        swordCollider = swordHitbox.GetComponent<Collider2D>();

        attackAction = new InputAction("Attack", InputActionType.Button, "<Keyboard>/space");
        attackAction.Enable();
        
    }

    private void Update()
    {
        direction = playerMovement.getDirection();
        playerMovement.Move();
        playerAnimation.animationUpdate(direction, checkIfMoving());
    }

    void OnMove(InputValue moveValue)
    {
        if (!canMove)
        {
            return;
        }
        playerMovement.moveInput = moveValue.Get<Vector2>();
    }

    void OnDestroy()
    {
        attackAction.Disable();
    }

    void OnAttack()
    {
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        LockMovement();
        EnableCollider();
        UpdateHitboxPosition(direction);
        playerAnimation.attackAnimation();

        for (int i = 0; i < attackDelay; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        DisableCollider();
        UnlockMovement();

    }

    void UpdateHitboxPosition(string direction)
    {
        Quaternion rotation = Quaternion.identity;

        switch(direction)
        {
            case "up":
            rotation = Quaternion.Euler(0,0,180);
            break;
            case "down":
            rotation = Quaternion.Euler(0,0,0);
            break;
            case "left":
            rotation = Quaternion.Euler(0,0,-90);
            break;
            case "right":
            rotation = Quaternion.Euler(0,0,90);
            break;
            case "default":
            Debug.LogWarning("Invalid Direction");
            return;
        }

        swordCollider.transform.rotation = rotation;
    }

    private bool checkIfMoving()
    {
        if(playerMovement.moveInput != Vector2.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LockMovement()
    {
        canMove = false;
        playerMovement.moveInput = Vector2.zero;
    }

    public void UnlockMovement() 
    {
        canMove = true;
    }
    
    public void EnableCollider()
    {
        if(swordCollider != null)
        {
            swordCollider.enabled = true;
        }
    }

    public void DisableCollider()
    {
        if(swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }

    
}
