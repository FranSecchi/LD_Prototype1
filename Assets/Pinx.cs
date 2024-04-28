using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinx : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        GameObject go = other.gameObject;
        IReceiveDamage damagable = go.GetComponent<IReceiveDamage>();
        if (damagable != null)
        {
            damagable.TakeDamage(gameObject);
        }
    }
}
