using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public bool thisWillKillthePlayer = false;
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
        if (col.CompareTag("Player"))
        {
            if(!thisWillKillthePlayer)
            {
                Debug.Log("Game Ends! Next Level");
            }else{
                Debug.Log("Game Ends! Retry!");
            }

        }
    }
}
