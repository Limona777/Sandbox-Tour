using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCamera : MonoBehaviour
{
    public float rotateTime = 0.2f;
    public float distanceFromPlayer = 5f;
    public float heightOffset = 2f;

    private Transform player;
    private bool isRotating = false;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        offset = new Vector3(0, heightOffset, -distanceFromPlayer);

        transform.position = player.position + offset;
        transform.LookAt(player.position + Vector3.up * heightOffset * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();

        if (!isRotating)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        transform.position = player.position + offset;
        transform.LookAt(player.position + Vector3.up * heightOffset * 0.5f);
    }

    void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isRotating)
        {
            StartCoroutine(RotateAround(-45, rotateTime));
        }
        if (Input.GetKeyDown(KeyCode.E) && !isRotating)
        {
            StartCoroutine(RotateAround(45, rotateTime));
        }
    }

    IEnumerator RotateAround(float angle, float time)
    {
        float number = 60 * time;
        float nextAngle = angle / number;
        isRotating = true;

        for (int i = 0; i < number; i++)
        {
            offset = Quaternion.Euler(0, nextAngle, 0) * offset;
            transform.position = player.position + offset;
            transform.LookAt(player.position + Vector3.up * heightOffset * 0.5f);
            yield return new WaitForFixedUpdate();
        }

        isRotating = false;
    }
}