using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharacterBehaviour : MonoBehaviour {

    public static CharacterBehaviour instance;

    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField]
    private bool grounded;
    private Vector2 groundedRaycastOrientation;
    private Vector2 groundedRaycastOrientationRev;
    private Vector3 groundRaycastOffsetRev;

    [Tooltip("Debug mode allows you to control the character with keyboard inputs.")]
    public bool debugMode;

    [Header("Character Controls")]
    [Range(-1, 1)]
    public int moveDir;
    public bool doJumpFrame;
    [Header("Movement Parameters")]
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float walkForce;
    [SerializeField]
    private float horizontalDrag;

    [Header("Ground Detection Parameters")]
    [SerializeField]
    private Vector3 groundRaycastOffset;
    [SerializeField]
    private float groundRaycastTilt;
    [SerializeField]
    private float groundRaycastDistance;
    [SerializeField]
    private LayerMask groundRaycastMask;

    // Start is called before the first frame update
    void Start() {

        if (!Application.isPlaying)
            return;

        instance = this;

        if (debugMode)
            Debug.LogWarning("Debug mode enabled!", this);

        UpdateGroundedRaycastOrientationVector();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

    }

    private void FixedUpdate() {

        if (!Application.isPlaying)
            return;

        grounded = Physics2D.Raycast(transform.position + groundRaycastOffset, groundedRaycastOrientation, groundRaycastDistance, groundRaycastMask) || Physics2D.Raycast(transform.position + groundRaycastOffsetRev, groundedRaycastOrientationRev, groundRaycastDistance, groundRaycastMask);
        rb.AddForce(Vector2.right * moveDir * walkForce * Time.deltaTime);

        if (grounded || (!grounded && moveDir != 0)) {
            //Apply drag on x-axis
            rb.velocity = new Vector2(rb.velocity.x / (1 + (horizontalDrag * Time.deltaTime)), rb.velocity.y);
        }

    }

    // Update is called once per frame
    void Update() {

        Debug.DrawRay(transform.position + groundRaycastOffset, groundedRaycastOrientation * groundRaycastDistance, Color.red);
        Debug.DrawRay(transform.position + groundRaycastOffsetRev, groundedRaycastOrientationRev * groundRaycastDistance, Color.red);

        if (!Application.isPlaying) {
            UpdateGroundedRaycastOrientationVector();
            return;
        }

        if (debugMode) {
            moveDir = Mathf.RoundToInt (Input.GetAxis("Horizontal"));
            doJumpFrame = Input.GetButtonDown("Jump");
        }

        anim.SetBool("Walking", moveDir != 0);
        anim.SetBool("Grounded", grounded);
        if (moveDir != 0)
            anim.transform.localScale = new Vector3(moveDir, 1, 1);

        if (doJumpFrame && grounded ) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        doJumpFrame = false;

    }

    private void UpdateGroundedRaycastOrientationVector() {

        groundedRaycastOrientation.x = Mathf.Cos(groundRaycastTilt * Mathf.Deg2Rad);
        groundedRaycastOrientation.y = Mathf.Sin(groundRaycastTilt * Mathf.Deg2Rad);
        groundedRaycastOrientationRev.x = -groundedRaycastOrientation.x;
        groundedRaycastOrientationRev.y = groundedRaycastOrientation.y;
        groundRaycastOffsetRev.x = -groundRaycastOffset.x;
        groundRaycastOffsetRev.y = groundRaycastOffset.y;

    }

}