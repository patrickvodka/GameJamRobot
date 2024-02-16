using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(GhostTrail))]

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float playerSpeed;
    public float jumpForce;
    private Vector3 velocity= Vector2.zero;
    private float horisontalMovemant;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float CooldownJump = .5f;
    private float TimeSinceJump = .2f;
    [Header("SpawnOfPlayer")]
    private bool firstInput;
    [HideInInspector]public bool thing;
    private bool isRebooting;
    private GhostTrail ghostTrail;
    [SerializeField] [NotNull] public Transform groundCheck;
    private const string HorizontalAxis = "Horizontal";
    private const float InputThreshold = 0.1f;
    private const float RotationZero = 0f;
    private bool blockInput;
    public ParticleSystem explosion;
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
        ChangePlayerOrientation();
    }

    private void Restart()
    {
        if(Input.GetButtonDown("Fire2"))
        LevelManager.RestartLevel();
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
        
            horisontalMovemant=playerSpeed * Input.GetAxis("Horizontal");
            Vector3 tagetVelocity = new Vector2(horisontalMovemant, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity,tagetVelocity,ref velocity,.2f);
        
        
    }
    private void Suicide()
    {
        if (Input.GetButtonUp("Fire1") && firstInput)
        {
            FakeDeath();
        }
        
    }
    private void CheckFirstInput()
    {
        
        if (!firstInput)
        {
            if (Input.GetButtonUp("Jump") || Input.GetAxis("Horizontal") != 0)
            {
               StartCoroutine(SetKinematic());
                firstInput = true;
                ghostTrail.StartRegister();
                if (ghostTrail.currentNbrClones > 0)
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
        return Physics.CheckSphere(groundCheck.position, 0.5f, ground);
        
    }
    private void ChangePlayerOrientation()
    {
        float horizontalInput = Input.GetAxis(HorizontalAxis);
        if (horizontalInput> InputThreshold)
        {
            transform.rotation = Quaternion.Euler(RotationZero, RotationZero, RotationZero);
        }
        else if (horizontalInput < -InputThreshold)
        {
            transform.rotation = Quaternion.Euler(RotationZero,180, RotationZero);
        }
    }
    
    private void FakeDeath()
    {
        blockInput = true;
        ghostTrail.blockWorking= true;
        ghostTrail.canRegister = false;
        Debug.Log($" can register ={ghostTrail.canRegister}");
        var explo = Instantiate(explosion, transform.position, Quaternion.identity);
        explo.Play();
        rb.velocity = new Vector3(0, 0, 0);
        rb.isKinematic = true;
        var iSpawn = GameManager.Instance.SpawnPlayerTimer();
        GameManager.Instance.StartCoroutine(iSpawn);
    }
    public IEnumerator StartMoving()
    {
        ghostTrail.blockWorking = false;
        ghostTrail.ChangeCurrentState();
        yield return new WaitForSeconds(.1f);
        rb.isKinematic = false;
        blockInput = false;
        firstInput = false;
        
    }

    private IEnumerator SetKinematic()
    {
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.3f);
        rb.isKinematic = false;
    }
    

   
}