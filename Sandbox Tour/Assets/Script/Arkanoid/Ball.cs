using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D rb;
    float playerV;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerV = FindObjectOfType<ArkanoidPlayerController>().ballShootSpeed;
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        float v = rb.velocity.magnitude;

        if (v > 0 && v != playerV)
        {
            rb.velocity = rb.velocity.normalized * playerV;
        }
    }

}
