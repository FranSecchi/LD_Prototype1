using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [Header("Variables")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 0.15f;
    private InputAction moveAction;
    public Rigidbody rb;
    private CharacterController ch;
    private Vector2 movementInput = Vector2.zero;
    private Vector3 minusInput = Vector3.zero;
    private void Awake()
    {
        moveAction = GetComponent<PlayerInput>().actions["Movement"];
        rb = GetComponent<Rigidbody>();
        ch = GetComponent<CharacterController>();
    }
    public void Move()
    {
        movementInput = moveAction.ReadValue<Vector2>();
        // Set input direction to world direction
        Vector3 horizontalInput = _camera.transform.right * movementInput.x;
        Vector3 verticalInput = _camera.transform.forward * movementInput.y;
        Vector3 movementDirection = verticalInput + horizontalInput;
        movementDirection.y = 0f;
        if (movementDirection.magnitude > 1f)
            movementDirection.Normalize();
        movementDirection *= moveSpeed;
        movementDirection += minusInput * moveSpeed;
        if(movementInput != Vector2.zero)
            ch.Move(Time.fixedDeltaTime * movementDirection);
        if (movementDirection != Vector3.zero) // Rotate towards input
        {
            SetRotation(movementDirection);
        }
    }

    // Handles Rotation of the player
    public void SetRotation(Vector3 movementDirection)
    {
        Quaternion targetRotation;
            targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
    }
    // If player on wall, avoid movement onto it
    internal void SubstractInput(Vector3 wallDirection)
    {
        minusInput = wallDirection;
    }
}
