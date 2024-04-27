using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinx : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        IReceiveDamage damagable = go.GetComponent<IReceiveDamage>();
        if (damagable != null)
        {
            damagable.TakeDamage(gameObject);
        }
    }
}
