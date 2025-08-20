using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMovementSimple : MonoBehaviour
{
    float jumpPower = 5f;
    float horizontalInput;
    public float moveSpeed = 5f; // Speed of the player movement
    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Animator animator;
    bool isGrounded = false;
    bool isFacingRight = true;
    int dashcount = 1;
    private float dashSpeed = 5f;
    private bool hasLanded = true;







    void Start()
    {                    
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        isGrounded = Mathf.Abs(rb.linearVelocity.y) < 0.1f;
        
        // Flip sprite based on movement direction
        FlipSprite();
       
        // Handle Jump
        Jump();
        Dash();
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);


        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);

    }

    

    void FlipSprite()
    {
        // Check if we need to flip the sprite
        if ((isFacingRight && horizontalInput < 0) || (!isFacingRight && horizontalInput > 0))
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

   
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetTrigger("isJumping");


        }

        if (Input.GetKeyUp(KeyCode.Space) && isGrounded)
        {
            animator.SetBool("isJumping", false);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower); // Apply the jump force
            
        
        }
    }
    private void Dash()
    {
        // Reset dash count only once when grounded
        if (isGrounded && hasLanded)
        {
            dashcount = 1; // Reset dash count
            hasLanded = false; // Prevent further resets until airborne
            animator.SetBool("isDash", false);
        }
        else if (!isGrounded)
        {
            hasLanded = true; // Allow reset when the player lands again
        }

        // Perform dash only if not grounded and a dash is available
        if (!isGrounded && dashcount > 0)
        {
            Vector2 dashDirection = Vector2.zero;

            // Check for dash input
            if (Input.GetKeyDown(KeyCode.W)) // Dash upward
            {
                dashDirection = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.A)) // Dash to the left
            {
                dashDirection = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.S)) // Dash downward
            {
                dashDirection = Vector2.down;
            }
            else if (Input.GetKeyDown(KeyCode.D)) // Dash to the right
            {
                dashDirection = Vector2.right;
            }

            // Apply dash force and trigger animation if a direction was chosen
            if (dashDirection != Vector2.zero && dashcount > 0)
            {
                rb.linearVelocity = dashDirection.normalized * dashSpeed; // Set velocity in the dash direction
                dashcount--; // Decrease dash count
                animator.SetBool("isDash", true); // Trigger dash animation only in air
            }
        }
    }




}
