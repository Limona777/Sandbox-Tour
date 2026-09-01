using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        Destroy(collision.gameObject);
        FindObjectOfType<ArkanoidPlayerController>().OnBallLost();
    }
}