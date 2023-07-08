using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float force = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Rigidbody2D target;
        target = col.gameObject.GetComponent<Rigidbody2D>();
        if(target != null)
        target.velocity = transform.up * force;
    }
}
