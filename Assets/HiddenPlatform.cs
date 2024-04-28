using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPlatform : MonoBehaviour
{
    [Header("Target a on anira la plataforma")]
    [SerializeField] private float HideMuch;
    [SerializeField] private float timeToHide;
    [SerializeField] private float timeToAppear;
    [SerializeField] private float speed;
    [SerializeField] private bool startHidden;
    private float elapsed = 0f;
    private bool hide = false;
    private bool moving = false;
    private Vector3 initialpos;
    private Vector3 targetpos;
    private void Start()
    {
        if (startHidden)
        {
            initialpos = transform.position - HideMuch * transform.right;
            targetpos = transform.position;
        }
        else
        {
            initialpos = transform.position;
            targetpos = transform.position - HideMuch * transform.right; 
        }
        transform.position = initialpos;
    }
    private void Update()
    {
        if (moving) return;
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
        Vector3 pos = v? targetpos : initialpos;
        while(Vector3.Distance(transform.position, pos) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed);
            yield return null;
        }
        moving = false;
    }
}
