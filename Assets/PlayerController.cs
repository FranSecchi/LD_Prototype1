using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region -Actions-
    private InputAction jumpAction;
    #endregion
    private CollisionManager cm;
    private PlayerJump myJump;
    private PlayerMovement myMovement;
    private bool isJumping = false;
    private void Awake()
    {
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
    }
    #region -Enable & Disable-
    private void OnEnable()
    {
        jumpAction.started += _ => JumpPress();
    }
    private void OnDisable()
    {
        jumpAction.started -= _ => JumpPress();
    }
    #endregion
    
    void Start()
    {
        cm = GetComponent<CollisionManager>();
        myJump = GetComponent<PlayerJump>();
        myMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(cm.IsGrounded());
        myMovement.Move();
        CheckJump();
        CheckWallCollision();
    }

    
    // Checks if the player is jumping, landing or falling
    private void CheckJump()
    {
        if (isJumping)
        {
            myJump.DoJump();
            if (cm.IsGrounded())
            {
                isJumping = false;
                myJump.Landed();
            }
        }
        else if (!cm.IsGrounded())
        {
            myJump.Fall();
            isJumping = true;
        }
    }
    // Performs jump
    private void JumpPress()
    {
        if (!cm.IsGrounded()) return;
        isJumping = true;
        myJump.Jump();
    }
    public void JumpOnEnemy()
    {
        isJumping = true;
        myJump.Jump();
    }
    // If not grounded and hiting a wall, cancel movement towards it
    private void CheckWallCollision()
    {
        if (cm.OnWall && !cm.IsGrounded())
        {
            myMovement.SubstractInput(cm.WallDirection);
        }
    }
}
