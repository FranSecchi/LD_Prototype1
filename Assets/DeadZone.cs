using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 initialPos;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.up * speed * Time.fixedDeltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        IReceiveDamage pl = other.transform.GetComponent<IReceiveDamage>();
        if(pl != null)
        {
            transform.position = initialPos;
            pl.Die();
        }
    }
}
