using System;
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
    [SerializeField] private float CooldownJump = 5.0f;
    private float TimeSinceJump = 6.0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
        TimeSinceJump += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(Input.GetAxis("Horizontal")==0 && IsGrounded() && Input.GetButtonUp("Jump"))
        {
            rb.velocity= Vector3.zero;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded() && TimeSinceJump > CooldownJump)
        {
            rb.velocity = Vector2.up * jumpForce;
            TimeSinceJump = 0.0f;
        }
    }


    private void Move()
    {
        if (IsGrounded())
        {
            horisontalMovemant=playerSpeed * Input.GetAxis("Horizontal");
            Vector3 tagetVelocity = new Vector2(horisontalMovemant, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity,tagetVelocity,ref velocity,.2f);
        }
        else
        {
            print("no");
        }
        
    }

    
    
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceCheckGrounded, ground)!= null;
    }

   
}
