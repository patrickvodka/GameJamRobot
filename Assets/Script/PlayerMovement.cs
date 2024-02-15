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
    [SerializeField] private LayerMask ground;
    [SerializeField] private float CooldownJump = .5f;
    private float TimeSinceJump = 6.0f;
    [Header("SpawnOfPlayer")]
    public Transform spawnPlayer;
    private bool firstInput;
    [HideInInspector]public bool thing;
    private bool isRebooting;
    private GhostTrail ghostTrail;
    [SerializeField] [NotNull] public Transform groundCheck;
    void Start()
    {
        firstInput = false;
        ghostTrail = GetComponent<GhostTrail>();
        rb = GetComponent<Rigidbody>();
        GameManager.Instance.PlayerStarted(gameObject, transform.position);

    }

    void Update()
    {
        Suicide();
        CheckFirstInput();
        Move();
        Jump();
        TimeSinceJump += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(Input.GetAxis("Horizontal")==0 && IsGrounded() && rb.velocity.y==0)
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
    private void Suicide()
    {
        if (Input.GetButtonUp("Jump")) {
            ghostTrail.canRegister = false;
            isRebooting = true;
            GameManager.Instance.SpawnPlayer(0);
        }
        
    }
    private void CheckFirstInput()
    {
        
        if (!firstInput)
        {
            if (Input.GetButtonUp("Jump") || Input.GetAxis("Horizontal") != 0)
            {
                Debug.Log("input");
                Debug.Log($"current = {ghostTrail.currentNbrClones}");
                firstInput = true;
                ghostTrail.StartRegister();
                if (ghostTrail.currentNbrClones != 0)
                {
                    ghostTrail.StartSpawning();
                }
            }
        }
        else
        {
            return;
        }
    }

    
    
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 1f, ground);
    }

   
}
