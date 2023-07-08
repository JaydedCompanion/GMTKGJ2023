using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float force = 5f;

    void OnTriggerEnter2D(Collider2D col)
    {
        Rigidbody2D target;
        print(col.gameObject.name);
        target = col.gameObject.GetComponent<Rigidbody2D>();
        if(target != null)
        target.velocity = transform.up * force;
    }
}
