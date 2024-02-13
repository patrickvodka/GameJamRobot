using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float playerSpeed;
    public float jumpForce;
    private Vector3 velocity= Vector2.zero;
    private float horisontalMovemant;
    public float distanceCheckGrounded = 0.5f;
    [SerializeField] private LayerMask ground;
    [SerializeField] [NotNull] public Transform groundCheck;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }


    private void Move()
    {
        horisontalMovemant=playerSpeed * Input.GetAxis("Horizontal");
         Vector3 tagetVelocity = new Vector2(horisontalMovemant, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity,tagetVelocity,ref velocity,.5f);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 1f, ground);
    }
}
