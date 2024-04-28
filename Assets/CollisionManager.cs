using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [Header("Layer masks")]
    [Tooltip("Layers that player can jump from")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [Tooltip("Height for ground detection")]
    [SerializeField] private float playerHeight = 1.8f;
    [Tooltip("Distance to detect the ground")]
    [SerializeField] private float distanceToGround = 0.1f;

    private bool onWall = false;
    private Vector3 wallDirection;

    public bool OnWall { get => onWall; set => onWall = value; }
    public Vector3 WallDirection { get => wallDirection; set => wallDirection = value; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 raycastOrigin = transform.position;
        Gizmos.DrawLine(raycastOrigin, raycastOrigin - Vector3.up * (playerHeight / 2 + distanceToGround));
    }
    public bool IsGrounded()
    {
        if (transform == null) return false;
        RaycastHit info;
        bool b = Physics.Raycast(transform.position, -Vector3.up, out info, playerHeight / 2 + distanceToGround, groundLayer);
        if (b)
        {
            Plataforma p = info.collider.GetComponent<Plataforma>();
            if (p != null)
                StartCoroutine(p.Drop());
        }
        return b;
    }
    public GameObject HitsEnemy()
    {
        RaycastHit info;
        bool b = Physics.Raycast(transform.position, -Vector3.up, out info, playerHeight / 2 + distanceToGround, enemyLayer);
        GameObject p = null;
            if(b) p = info.collider.gameObject;
        return p;
    }
    private void OnCollisionStay(Collision collision)
    {
        var direction = (transform.position - collision.contacts[0].point).normalized;
        if (Mathf.Abs(direction.y) < 0.85f)
        {
            onWall = true;
            wallDirection = direction;
            wallDirection.y = 0f;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        onWall = false;
    }
}
