using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Vector3 gizmosSize;
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 1f, 0.5f);
        Gizmos.DrawCube(transform.position, gizmosSize);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, gizmosSize);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem hs = other.gameObject.GetComponent<HealthSystem>();
            Destroy(hs.CheckPoint.gameObject);
            hs.CheckPoint = transform;
            Destroy(this);
        }
    }
}
