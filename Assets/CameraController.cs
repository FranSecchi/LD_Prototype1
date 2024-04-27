using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform centerOfTower;
    [SerializeField] private Transform playerTransform;
    [Header("Camera Settings")]
    [SerializeField] private float radiusToTower;
    [SerializeField] private float cameraHeightOffset;
    [SerializeField] private float lookAtAngle;
    [SerializeField] private float lerp;



    // Update is called once per frame
    void Update()
    {
        SetCameraPosition();
        


    }
    private void SetCameraPosition()
    {
        Vector3 directionToPlayer = centerOfTower.position - playerTransform.position;
        directionToPlayer.y = 0f;
        directionToPlayer.Normalize();
        
        

        Vector3 cameraPosition = new Vector3(centerOfTower.position.x, 0f, centerOfTower.position.z) + directionToPlayer * radiusToTower;
        cameraPosition.y = playerTransform.position.y + cameraHeightOffset;
        transform.position = Vector3.Lerp(transform.position,cameraPosition,lerp);

        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y - lookAtAngle,
            playerTransform.position.z));


    }

}
