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
    private PlayerAttack playerAttack;
    private InputAction attackAction;
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;
    

    [SerializeField] string direction;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private float attackDuration = 1f;
    [SerializeField] private float attackDelay = 1f;


    public GameObject swordHitbox;
    Collider2D swordCollider;

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();

        swordCollider = swordHitbox.GetComponent<Collider2D>();

        attackAction = new InputAction("Attack", InputActionType.Button, "<Keyboard>/space");
        attackAction.Enable();
    }

    private void Update()
    {
        //IsInput();
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     LockMovement();
        // }

        // if (Input.GetKeyDown(KeyCode.G))
        // {
        //     UnlockMovement();
        // }

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
        playerAnimation.attackAnimation();
        //playerAttack.Attack();

        // for (int i = 0; i < attackDelay; i++)
        // {
        //     yield return new WaitForEndOfFrame();
        //     yield return new WaitForSeconds(1);
        // }

        for (int i = 0; i < attackDelay; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        
        UnlockMovement();

    }

    public void LockMovement()
    {
        canMove = false;
        //Debug.Log("LockMovement");
    }

    public void UnlockMovement() 
    {
        canMove = true;
        //Debug.Log("UnlockMovement");
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

    bool IsInput() //debug
    {
        if (Input.anyKey)
        {
            //Debug.Log("Input Detected");
            return true;
        }
        else return false;
    }

}
