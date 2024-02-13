using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float playerSpeed;
    public float jumpForce;
    private Vector3 velocity= Vector2.zero;
    private float horisontalMovemant;
    public float distanceCheckGrounded = 0.5f;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            Vector2 vel = rb.velocity;
            vel.y = jumpForce;
            rb.velocity = vel;
            Debug.Log("jump");
        }
    }


    private void Move()
    {
        horisontalMovemant=playerSpeed * Input.GetAxis("Horizontal");
         Vector3 tagetVelocity = new Vector2(horisontalMovemant, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity,tagetVelocity,ref velocity,.5f);
    }
    
    private bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distanceCheckGrounded, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }
}
