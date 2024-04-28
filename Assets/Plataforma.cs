using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma : MonoBehaviour
{
    [SerializeField] private float timeToDrop;
    private bool droping = false;

    public IEnumerator Drop()
    {
        if (droping) yield return null;
        droping = true;
        float elapsed = 0f;
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = Color.yellow;
        while(elapsed < timeToDrop)
        {
            float x = elapsed / timeToDrop;
            if (x >= 0.6f)
                rend.material.color = Color.black;
            else if (x >= 0.3)
                rend.material.color = Color.red;
            elapsed += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Reactive());
    }
    public IEnumerator Reactive()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(4f);
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
    }
}
