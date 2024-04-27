using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinxos : MonoBehaviour
{
    [SerializeField] private Transform pinxo;
    [SerializeField] private float timeToUp;
    [SerializeField] private float timeToDown;
    private float elapsed = 0f;
    private bool up = false;
    private void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed >= timeToUp)
        {
            if (!up)
            {
                up = true;
                pinxo.GetComponent<Animator>().SetTrigger("Up");
            }
            else if(elapsed - timeToUp > timeToDown)
            {
                pinxo.GetComponent<Animator>().SetTrigger("Up");
                up = false;
                elapsed = 0f;
            }
        }
    }
}
