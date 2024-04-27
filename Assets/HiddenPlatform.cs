using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPlatform : MonoBehaviour
{
    [Header("Target a on anira la plataforma")]
    [SerializeField] private Transform target;
    [SerializeField] private float timeToHide;
    [SerializeField] private float timeToAppear;
    [SerializeField] private float speed;
    private float elapsed = 0f;
    private bool hide = false;
    private bool moving = false;
    private Vector3 initialpos;
    private void Start()
    {
        initialpos = transform.position;
    }
    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= timeToHide)
        {
            if (!hide)
            {
                hide = true;
                StartCoroutine(Move(true));
            }
            else if (elapsed - timeToHide > timeToAppear)
            {
                StartCoroutine(Move(false));
                hide = false;
                elapsed = 0f;
            }
        }
    }

    private IEnumerator Move(bool v)
    {
        moving = true;
        Vector3 pos = v?target.position : initialpos;
        while(Vector3.Distance(transform.position, pos) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed);
            yield return null;
        }
        moving = false;
    }
}
