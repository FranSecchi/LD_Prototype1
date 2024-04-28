using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    public void Drop()
    {
        player.JumpOnEnemy();
        Destroy(gameObject);
    }
}
