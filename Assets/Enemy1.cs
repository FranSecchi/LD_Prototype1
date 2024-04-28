using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private Transform hitbox;
    [Header("--LanternBot Variables--")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float coneAngle = 45f;
    [SerializeField] private float tontoTime;
    [SerializeField] private LayerMask playerLayer;
    [Header("Patrol point gizmos")]
    [SerializeField] private Color color;
    [SerializeField] private float gizmosRadius;
    private CollisionManager cm;
    private Rigidbody rb;
    private Transform currentTarget;
    private bool isRotating = false;
    private bool isChasing = false;
    private bool isTonto = false;
    private int targetIndex = 0;
    private int indexsign = -1;
    #region -Gizmos-
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        DrawCone(transform.position, transform.forward, detectionRadius, coneAngle, Color.yellow);
        if (patrolPoints.Length == 0)
            return;
        Gizmos.color = color;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(patrolPoints[i].position, gizmosRadius);
        }
    }

    private void DrawCone(Vector3 position, Vector3 direction, float radius, float angle, Color color)
    {
        Vector3 coneVertex = position + direction.normalized * radius;
        Quaternion coneRotation = Quaternion.LookRotation(direction);
        Vector3 coneSide1 = coneRotation * Quaternion.Euler(0, angle / 2f, 0) * Vector3.forward * radius;
        Vector3 coneSide2 = coneRotation * Quaternion.Euler(0, -angle / 2f, 0) * Vector3.forward * radius;

        Gizmos.color = color;
        Gizmos.DrawLine(position, coneVertex);
        Gizmos.DrawLine(position, position + coneSide1);
        Gizmos.DrawLine(position, position + coneSide2);
        Gizmos.DrawLine(coneVertex, position + coneSide1);
        Gizmos.DrawLine(coneVertex, position + coneSide2);
    }
    #endregion


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cm = GetComponent<CollisionManager>();
        if (patrolPoints.Length > 0)
            currentTarget = patrolPoints[0];
    }
    private void Start()
    {
        SetRotation(Quaternion.LookRotation((currentTarget.position - transform.position).normalized), rotationSpeed);
    }
    private void FixedUpdate()
    {
        if (isTonto) return;
        Debug.Log(cm.IsGrounded());
        DetectPlayer();
        if (!cm.IsGrounded())
        {
            isChasing = false;
            currentTarget = patrolPoints[targetIndex];
        }
        if (isChasing)
        {
            Chase();
        }
        else
        {
            Patrol();
        }

    }

    // Patrols using transforms as points
    private void Patrol()
    {
        float distance = Vector3.Distance(transform.position, currentTarget.position);
        if (!isRotating && distance < 0.1f)
        {
            if ((targetIndex == patrolPoints.Length - 1 && indexsign > 0) || (targetIndex == 0 && indexsign < 0)) indexsign *= -1;
            targetIndex += indexsign;
            currentTarget = patrolPoints[targetIndex];
            rb.velocity = Vector3.zero;
            isRotating = true;
        }
        if (isRotating)
            SetRotation(Quaternion.LookRotation((currentTarget.position - transform.position).normalized), rotationSpeed);
        else
            rb.velocity = (currentTarget.position - transform.position).normalized * patrolSpeed;
    }
    private void Chase()
    {
        if (!isRotating)
        {
            rb.velocity = Vector3.ProjectOnPlane((currentTarget.position - transform.position),Vector3.up).normalized * chaseSpeed;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
    }
    private void DetectPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        if (colliders.Length > 0)
        {
            Transform player = colliders[0].transform;
            Vector3 direction = player.position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, direction);
            if (angleToPlayer <= coneAngle / 2f)
            {
                if (CheckInPoV(player, direction))
                {
                    // Apply animation!
                    currentTarget = player;
                    isRotating = true;
                    SetRotation(Quaternion.LookRotation(Vector3.ProjectOnPlane((currentTarget.position - transform.position), Vector3.up)), rotationSpeed);
                    isChasing = true;
                }
                else
                {
                    isChasing = false;
                    isRotating = true;
                    currentTarget = patrolPoints[targetIndex];
                }
            }
        }
    }

    private bool CheckInPoV(Transform player, Vector3 direction)
    {
        RaycastHit info;
        Physics.Raycast(transform.position, direction, out info, direction.magnitude);
        if (info.transform.Equals(player))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetRotation(Quaternion targetRotation, float speed)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed);
        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane((currentTarget.position - transform.position), Vector3.up))) < 5f)
        {
            isRotating = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        IReceiveDamage damagable = go.GetComponent<IReceiveDamage>();
        if (damagable != null)
        {
            damagable.TakeDamage(gameObject);
            currentTarget = patrolPoints[targetIndex];
            isChasing = false;
            isRotating = true;
        }
    }
    public void Atontar()
    {
        isChasing = false;
        isRotating = true;
        StartCoroutine(Tonto());
    }
    private IEnumerator Tonto()
    {
        hitbox.gameObject.SetActive(false);
           Color color = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.black;
        isTonto = true;
        yield return new WaitForSeconds(tontoTime);
        hitbox.gameObject.SetActive(true);
        isTonto = false;
        GetComponent<Renderer>().material.color = color;
    }

    internal void SetPointsInPosition()
    {
        if (patrolPoints.Length == 0)
            return;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Vector3 pos = patrolPoints[i].position;
            pos.y = transform.position.y;
            patrolPoints[i].position = pos;

        }

    }
}


[CustomEditor(typeof(Enemy1))]
public class LanternbotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Enemy1 myScript = (Enemy1)target;
        if (GUILayout.Button("Set Patrol Points Height"))
        {
            myScript.SetPointsInPosition();
        }
    }
}
