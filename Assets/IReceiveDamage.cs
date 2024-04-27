using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReceiveDamage
{
    public void TakeDamage(GameObject actor);
    public void Die();
}
