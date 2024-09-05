using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    public Rigidbody2D rg;
    public Animation anim;
    public Collider2D playerCollider;

    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public float fallMultiplier;
    public float wallJumpForce;
    public bool isOnPlatform;
    public bool isGrounded;
    public bool isWalled;
    public bool isJumping;
    public bool isJumpingDown;
    public bool isRunning;
    public bool isWallJumping;
    public bool isDoubleJumping;

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    public float groundCheckRadius;
    public float wallCheckDistance;

    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        // anim = GetComponent<Animation>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isWalled = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wallLayer);
        if (isGrounded)
        {
            isDoubleJumping = false;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            
            StartCoroutine(JumpDown());
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && !isDoubleJumping)
        {
            DoubleJump();
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private IEnumerator JumpDown()
    {
        // Se não estiver no chão
        if (!isGrounded) yield break;
        // Se não estiver em cima de uma plataforma
        if(!isOnPlatform) yield break;
        Debug.Log("JUMP DOWN");
        isJumpingDown = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), true);
        yield return new WaitForSeconds(0.5f);
        isJumpingDown = false;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), false);

    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            isJumping = false;
            isWallJumping = false;
        }
        if (isWalled && !isGrounded && rg.velocity.y < 0)
        {
            rg.gravityScale = 0;
            rg.velocity = new Vector2(rg.velocity.x, -0.5f);
        }
        else
        {
            if (isJumpingDown)
            {
                rg.gravityScale = 3;    
            }
            else
            {
                rg.gravityScale =  1 *  fallMultiplier;    
            }
            
        }
        if (isWalled && !isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            WallJump();
        }
        if (isRunning)
        {
            Run();
        }
        else
        {
            Walk();
        }
    }

    private void Walk()
    {
        float move = Input.GetAxis("Horizontal");
        rg.velocity = new Vector2(move * walkSpeed, rg.velocity.y);
        rg.velocity.Normalize();

        if(move > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (move < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        // anim.Play("Walk");
    }

    private void Run()
    {
        float move = Input.GetAxis("Horizontal");
        rg.velocity = new Vector2(move * runSpeed, rg.velocity.y);

        if (move > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if(move < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        // anim.Play("Run");
    }

    private void Jump()
    {
        rg.velocity = new Vector2(rg.velocity.x, jumpForce);
        isJumping = true;
    }

    private void DoubleJump()
    {
        rg.velocity = new Vector2(rg.velocity.x, jumpForce);
        isDoubleJumping = true;
    }

    private void WallJump()
    {
        rg.velocity = new Vector2(rg.velocity.x, wallJumpForce);
        isWallJumping = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            isOnPlatform = true;
        }
         
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            isOnPlatform = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
