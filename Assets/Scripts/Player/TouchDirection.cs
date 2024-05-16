using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Tamzid
Helper script designed to determine whether the player character is
1. grounded
2. touching walls
3. touching the ceiling
Script makes use of raycasting and collider properties to validate 
surrounding GameObjects in the scene.
The logic is set up such the specific directional rays from the player character
are used to determine and set properties that indicate the character's location.
*/
public class TouchDirection : MonoBehaviour
{
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;
    public float diagonalDistance = 0.1f;

    // Filter to make sure only colliding with the correct objects/layers
    public ContactFilter2D castFilter;

    // Collider to use for checks
    private Animator animator;
    private CapsuleCollider2D touchingCollider;

    // Ray cast hits
    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] wallHits = new RaycastHit2D[5];
    private RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    private RaycastHit2D[] diagonalHits = new RaycastHit2D[5];

    // Ground check
    [SerializeField]
    private bool _isGrounded;
    public bool IsGrounded
    {
        get => _isGrounded;
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimatorParamStrings.isGrounded, value);
        }
    }

    // Wall check
    [SerializeField]
    private bool _isOnWall;
    public bool IsOnWall
    {
        get => _isOnWall;
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimatorParamStrings.isOnWall, value);
        }
    }

    // Ceiling check
    [SerializeField]
    private bool _isOnCeiling;
    public bool IsOnCeiling
    {
        get => _isOnCeiling;
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimatorParamStrings.isOnCeiling, value);
        }
    }

    // Diagonal checks
    private bool _isOnDiagonal;
    public bool IsOnDiagonal
    {
        get => _isOnDiagonal;
        private set
        {
            _isOnDiagonal = value;
        }
    }

    // Determine which way the wall is for the check to initiate
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    private void Awake()
    {
        touchingCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Casting. Check for the ground, wall, ceiling, and diagonals
    private void FixedUpdate()
    {
        IsGrounded = touchingCollider.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCollider.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCollider.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;

        // Diagonal checks
        IsOnDiagonal =
            touchingCollider.Cast(new Vector2(-1, -1).normalized, castFilter, diagonalHits, diagonalDistance) > 0 || // Bottom-left
            touchingCollider.Cast(new Vector2(1, -1).normalized, castFilter, diagonalHits, diagonalDistance) > 0 ||  // Bottom-right
            touchingCollider.Cast(new Vector2(-1, 1).normalized, castFilter, diagonalHits, diagonalDistance) > 0 ||  // Top-left
            touchingCollider.Cast(new Vector2(1, 1).normalized, castFilter, diagonalHits, diagonalDistance) > 0;     // Top-right
    }
}
