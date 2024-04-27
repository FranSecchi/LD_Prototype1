using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Info")]
    [Tooltip("Height of the jump")]
    public float JumpHeight = 10f;
    [Tooltip("Time to reach the peak of the jump")]
    public float TimeToPeak = 0.5f;
    [Tooltip("Fall speed multiplier")]
    public float fallSpeedMultiplier = 2f;
    [Tooltip("Info of the last jump time")]
    public float timepeaking = 0f;
    private CharacterController ch;
    private bool canJump = true;
    private float gravity;
    private float speed;
    private void Start()
    {
        ch = GetComponent<CharacterController>();
        gravity = 2 * JumpHeight / (Mathf.Pow(TimeToPeak, 2));
    }
    private void FixedUpdate()
    {
        
    }
    // Apply initial speed if player can jump
    public void Jump()
    {
        speed = Mathf.Sqrt(2 * gravity * JumpHeight);
        canJump = false;
        timepeaking = 0f;
    }

    public void DoJump()
    {
        if (speed > 0f)
        {
            timepeaking += Time.fixedDeltaTime;
            ch.Move(speed * Time.fixedDeltaTime * Vector3.up);
        }
        else
            ch.Move(speed * fallSpeedMultiplier * Time.fixedDeltaTime * Vector3.up);
        speed -= gravity * Time.fixedDeltaTime;
    }

    // Land on ground
    public void Landed()
    {
        canJump = true;
    }
    public void Fall()
    {
        canJump = false;
        speed = 0f;
    }


    public void JumpOff()
    {
        canJump = false;
    }
    public void JumpOn()
    {
        canJump = true;
    }
}
