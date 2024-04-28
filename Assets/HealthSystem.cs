using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthSystem : MonoBehaviour, IReceiveDamage
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private int lifes;
    private int hp;
    public Transform CheckPoint { get => checkPoint; set => checkPoint = value; }
    private void Start()
    {
        hp = lifes;
        text.text = "Lifes: " + hp;
    }

    private void Awake()
    {
        Destroy(checkPoint.GetComponent<CheckPoint>());
    }
    public void Die()
    {
        StartCoroutine(Dead());
    }
    public void TakeDamage(GameObject actor)
    {
        --hp;
        text.text = "Lifes: " + hp;
        if(hp == 0)
            Die();
    }
    private IEnumerator Dead()
    {
        yield return new WaitForEndOfFrame();
        transform.position = checkPoint.position;
        hp = lifes;
        text.text = "Lifes: " + hp;
    }
}
