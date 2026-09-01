using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 1f;
    public Animator animator;
    Rigidbody _rb;

    private int facingDirection = 1;
    private bool lastVerticalDirectionDown = true;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        _rb = GetComponent<Rigidbody>();

        animator.SetBool("Down", true);
        animator.SetBool("Move", false);
    }

    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        Vector3 movement = (transform.right * h + transform.forward * v).normalized;
        _rb.velocity = new Vector3(movement.x * speed, _rb.velocity.y, movement.z * speed);

        if (v > 0)
        {
            lastVerticalDirectionDown = false;
        }
        else if (v < 0)
        {
            lastVerticalDirectionDown = true;
        }

        bool isMoving = h != 0 || v != 0;
        animator.SetBool("Move", isMoving);
        animator.SetBool("Down", lastVerticalDirectionDown);
    }
}