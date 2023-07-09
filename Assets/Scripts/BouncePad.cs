using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float force = 5f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Rigidbody2D target;
            target = col.gameObject.GetComponent<Rigidbody2D>();
            if (target != null)
                target.velocity = transform.up * force;
        }
    }
}
