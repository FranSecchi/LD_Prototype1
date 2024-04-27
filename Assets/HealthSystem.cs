using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IReceiveDamage
{
    [SerializeField] private Transform checkPoint;
    [SerializeField] private int lifes;
    private int hp;
    public Transform CheckPoint { get => checkPoint; set => checkPoint = value; }
    private void Start()
    {
        hp = lifes;
    }

    private void Awake()
    {
        Destroy(checkPoint.GetComponent<CheckPoint>());
    }
    public void Die()
    {
        transform.position = checkPoint.position;
        hp = lifes;
    }

    public void TakeDamage(GameObject actor)
    {
        --hp;
        Debug.Log("Hit by: " + actor.name+" Lifes: " + hp);
        if(hp == 0)
            Die();
    }
}
