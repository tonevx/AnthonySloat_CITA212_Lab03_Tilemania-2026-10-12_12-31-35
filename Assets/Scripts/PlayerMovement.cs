using System;
using NUnit.Framework;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;

    [SerializeField] AudioSource audioSource;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpHeight = 25f;
    [SerializeField] float climbSpeed = 7f;
    Animator myAnimator;
    CapsuleCollider2D capsuleCollider2D;
    BoxCollider2D footCollider2D;

    CinemachineImpulseSource impulseSource;

    float gravityScaleAtStart;

    bool isAlive = true;

    void Start()
    {
      myRigidbody = GetComponent<Rigidbody2D>();  
      audioSource = GetComponent<AudioSource>();
      myAnimator = GetComponent<Animator>();
      capsuleCollider2D = GetComponent<CapsuleCollider2D>();
      gravityScaleAtStart = myRigidbody.gravityScale;
      footCollider2D = GetComponent<BoxCollider2D>();
      impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Death();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive){return;}
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(!footCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if(value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpHeight);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.linearVelocity.y);
        myRigidbody.linearVelocity = playerVelocity;
        bool isRunning = Mathf.Abs(myRigidbody.linearVelocity.x) > 0.1f;
        if (isRunning && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!isRunning && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", hasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        if(hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if(!footCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        { 
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return; 
        }
        myRigidbody.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(myRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
        myRigidbody.linearVelocity = climbVelocity;
        bool hasVerticality = Mathf.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", hasVerticality);
    }

    void Death()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) || footCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            if (impulseSource != null)
            {
                impulseSource.GenerateImpulse();
            }
        }
    }
}
