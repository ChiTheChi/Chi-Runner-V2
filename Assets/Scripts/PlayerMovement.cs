using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D body;
    private float horizontalInput;
    private bool jumpInput;
    private bool walled;
    private float currentSpeed;
    private bool isWallJumping = false;
    private bool isWallSliding;
    private bool grounded;

    private bool IsFalling;
    private float lastJumpTime;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        walled = false;
    }
    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleFlipping();
        HandleGroundBool();
    }
    private void HandleInput()
    {
        if (walled) 
        {
            horizontalInput = 0f;
        }
        else 
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        jumpInput = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
    }

    private void HandleMovement()
    {
        if (horizontalInput != 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, speed * horizontalInput, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        if (!isWallJumping && !walled)
        {
            body.velocity = new Vector2(currentSpeed, body.velocity.y);
        }

        if (jumpInput && grounded && Time.time - lastJumpTime >= 0.5f)
        {
            //JumpSound.Play();
            Jump();
            Debug.Log("Input Jump Ground Value: " + grounded);
            lastJumpTime = Time.time;
        }
    }

    private void HandleFlipping()
    {
        // Flip the player character based on movement direction
        if (!walled && !isWallSliding && Mathf.Abs(horizontalInput) > 0.01f)
        {
            if (horizontalInput > 0)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else if (horizontalInput < 0)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }
    }

    private void HandleGroundBool()
    {
        grounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        if (grounded)
        {
            Debug.Log("Grounded is true");
            isWallSliding = false;
            walled = false;
            horizontalInput = Input.GetAxis("Horizontal");
           // anim.SetBool("Walling", false);
            IsFalling = false;
        }
        else if (!grounded && !walled && !isWallSliding && IsFalling == false)
        {
            //   anim.SetTrigger("Falling");
            Debug.Log("Grounded is false");
            IsFalling = true;
        }

    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
//        StartCoroutine(DelayedJumpAnimation());
    }


}
