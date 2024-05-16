using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
Tamzid
Main controller script for the PolloMan character.
This script handles:
1. Character movement (walking, running, and jumping)
2. Sliding mechanic with a timer and collider adjustments
3. Facing direction and flipping the character sprite
4. Interaction with the TouchDirection helper script for environmental checks
*/

[RequireComponent(typeof(Rigidbody2D), typeof(TouchDirection))]
public class PolloManController : MonoBehaviour
{
    // Movement parameters
    public float walkSpeed = 15f;
    public float runSpeed = 20f;
    public float jumpForce = 10f;
    public float doubledMoveSpeed = 0f;

    // Sliding parameters
    public float slideDuration = 1.0f;
    private bool _isSlidingButtonPressed = false;
    private float _timeSinceLastSlide = 0.0f;
    private float _slideTimer = 0.0f;

    // Movement speed selector
    private float CurrentMoveSpeed => IsMoving && !touchingDirections.IsOnWall ? (doubledMoveSpeed > 0f ? doubledMoveSpeed : walkSpeed) : 0;

    // Animator controller setup
    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get => _isMoving;
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimatorParamStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool _isJumping = false;
    public bool IsJumping
    {
        get => _isJumping;
        private set
        {
            _isJumping = value;
            animator.SetBool(AnimatorParamStrings.isJumping, value);
        }
    }

    // Direction check
    [SerializeField]
    private bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
            {
                // Flip scale to face the opposite direction
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    // Sliding check
    [SerializeField]
    private bool _isSliding = false;
    public bool IsSliding
    {
        get => _isSliding;
        private set
        {
            _isSliding = value;
            animator.SetBool(AnimatorParamStrings.isSliding, value);
        }
    }

    // Movement input
    private Vector2 _moveInput;

    // Components
    private TouchDirection touchingDirections;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 _originalColliderSize;

    // Audio clips
    public AudioClip walkingSound;
    public AudioClip runningSound;
    public AudioClip jumpSound;
    public AudioClip slidingSound;
    private AudioSource audioSource;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchDirection>();
        audioSource = GetComponent<AudioSource>();
        _originalColliderSize = GetComponent<CapsuleCollider2D>().size;
    }
    private void Update()
    {
        HandleSliding();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleSliding()
    {
        if (IsSliding)
        {
            _slideTimer -= Time.deltaTime;
            if (_slideTimer <= 0.0f)
            {
                EndSlide();
            }
        }

        if (_isSlidingButtonPressed && !IsSliding && touchingDirections.IsGrounded)
        {
            StartSlide();
        }

        if (_isSlidingButtonPressed)
        {
            _timeSinceLastSlide += Time.deltaTime;
            if (_timeSinceLastSlide >= 2.0f)
            {
                _timeSinceLastSlide = 0.0f;
                IsSliding = false;
            }
        }
    }

    private void StartSlide()
    {
        IsSliding = true;
        _slideTimer = slideDuration;
        GetComponent<CapsuleCollider2D>().size = new Vector2(_originalColliderSize.x, _originalColliderSize.y * 0.5f); // Reduce collider size for sliding
        PlaySlidingSound();
    }

    private void EndSlide()
    {
        IsSliding = false;
        GetComponent<CapsuleCollider2D>().size = _originalColliderSize; // Reset collider size
        _timeSinceLastSlide = 0.0f;
        StopSlidingSound();
    }

    private void HandleMovement()
    {
        rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        animator.SetFloat(AnimatorParamStrings.yVelocity, rb.velocity.y);
        
        if (IsMoving && !IsJumping && touchingDirections.IsGrounded)
        {
            PlayFootstepSound();
        }
    }

    private void PlayFootstepSound()
    {
        if (CurrentMoveSpeed == runSpeed)
        {
            audioSource.clip = runningSound;
        }
        else
        {
            audioSource.clip = walkingSound;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    private void PlaySlidingSound()
    {
        audioSource.clip = slidingSound;
        audioSource.Play();
    }

    private void StopSlidingSound()
    {
        if (audioSource.clip == slidingSound)
        {
            audioSource.Stop();
        }
    }

    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isSlidingButtonPressed = true;
        }
        else if (context.canceled)
        {
            _isSlidingButtonPressed = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>(); // x and y movement inputs
        IsMoving = _moveInput != Vector2.zero;
        SetFacingDirection(_moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            audioSource.PlayOneShot(jumpSound);
        }
    }
}
