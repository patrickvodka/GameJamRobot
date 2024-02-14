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
    [SerializeField] private float airControl=4;
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
        if (IsGrounded())
        {
            horisontalMovemant=playerSpeed * Input.GetAxis("Horizontal");
            Vector3 tagetVelocity = new Vector2(horisontalMovemant, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity,tagetVelocity,ref velocity,.5f);
        }
        else
        {
            horisontalMovemant=airControl * Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horisontalMovemant, rb.velocity.y);
        }
    }

    [SerializeField] bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 1f, ground);
    }

   
}
