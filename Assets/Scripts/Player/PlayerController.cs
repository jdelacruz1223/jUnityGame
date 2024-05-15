using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Threading;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    private PlayerAttack attackChild;
    private InputAction attackAction;
    private PlayerMovement moveChild;
    private PlayerAnimation animChild;

    void Start()
    {
        attackChild = GetComponent<PlayerAttack>();
        moveChild = GetComponent<PlayerMovement>();

        attackAction = new InputAction("Attack", InputActionType.Button, "<Keyboard>/space");
        attackAction.Enable();

        DataManager.me.canMove = true;
    }
    
    private void FixedUpdate()
    {
        moveChild.movement();
    }

    void OnAttack()
    {
        //DataManager.me.canMove = false;
        attackChild.Attack();
    }

    void OnMove(InputValue moveValue)
    {
        moveChild.moveInput = moveValue.Get<Vector2>();
    }

    void OnDestroy()
    {
        attackAction.Disable();
    }

}
