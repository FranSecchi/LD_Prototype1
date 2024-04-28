using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Coin : MonoBehaviour
{
    [SerializeField] private Transform camara;
    [SerializeField] private bool isGood;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Material x10;
    static int count = 0;
    private void Start()
    {
        if (isGood) GetComponent<Renderer>().material = x10;
        text.text = "Points" + count;
    }
    private void Update()
    {
        transform.LookAt(camara);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isGood)
            count += 10;
        else ++count;
        text.text = "Points: " + count;
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }
}
