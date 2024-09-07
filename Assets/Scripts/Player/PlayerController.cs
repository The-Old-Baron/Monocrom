using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : Entity
{
    public Rigidbody2D rg;
    public Animation anim;
    public BoxCollider2D playerCollider;
    

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
    public bool isChrouching;

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    public float groundCheckRadius;
    public float wallCheckDistance;
    
    private Vector2 crouchSize;
    private Vector2 crouchOffset;
    
    private Vector2 originalSize;
    private Vector2 originalOffset;
    
    
    // Essas 2 variaveis abaixo é só pra ver o cubo "agachando", quando for o sprite mesmo a gente usa o sprite agachando
    private Vector3 originalScale;
    private Vector3 crouchScale;
    
    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        // anim = GetComponent<Animation>();
        playerCollider = GetComponent<BoxCollider2D>();

        originalOffset = playerCollider.offset;
        originalSize = playerCollider.size;
        
        
        crouchSize = new Vector2(originalSize.x, originalSize.y / 2);
        crouchOffset = new Vector2(originalOffset.x, originalSize.y / 2);


        originalScale = transform.localScale;
        crouchScale = new Vector3(originalScale.x, originalScale.y / 2, originalScale.z);
    }

    private void Update()
    {
        
        // isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);    
        
        isWalled = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wallLayer);
        if (isGrounded)
        {
            isDoubleJumping = false;
        }

        if (Input.GetKey(KeyCode.S))
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(JumpDown());
                return;    
            }
            Crouch();
            return;
        }

        if (isChrouching)
        {
            Standup();            
        }
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isOnPlatform))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isJumping && (!isGrounded && !isOnPlatform) && !isDoubleJumping)
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

    private void Crouch()
    {
        // Em tese essas 4 linhas vão funcionar quando tivermos um sprite de agachamento
        // O size reduz o tamanho do colisor e o offset acredito que não vai precisar
        // mas de todo modo vou deixar ele comentado por que pode ser que ajude
        isChrouching = true;
        transform.localScale = crouchScale;
        // playerCollider.size = crouchScale;
        // playerCollider.offset = new Vector2(0,-0.25f);
    }

    private void Standup()
    {
        isChrouching = false;
        transform.localScale = originalScale;
        playerCollider.size = originalSize;
        playerCollider.offset = originalOffset;
    }

    private IEnumerator JumpDown()
    {
        // Se não estiver no chão
        if (!isGrounded) yield break;
        // Se não estiver em cima de uma plataforma
        if(!isOnPlatform) yield break;
        Debug.Log("JUMP DOWN");
        isJumpingDown = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Map"), true);
        yield return new WaitForSeconds(0.5f);
        isJumpingDown = false;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Map"), false);

    }

    private void FixedUpdate()
    {
        if (isGrounded && !isJumping)
        {
            
            rg.gravityScale = 1;
        }
        if (isWalled && !isGrounded && rg.velocity.y < 0)
        {
            rg.gravityScale = 0;
            rg.velocity = new Vector2(rg.velocity.x, -0.5f);
        }
        if (isJumpingDown)
        {
            rg.gravityScale = 3;    
        }

        if (isJumping)
        {
            rg.gravityScale =  1 * fallMultiplier;
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
            isJumping = false;
            isDoubleJumping = false;
            isWallJumping = false;
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = true;
            isJumping = false;
            isDoubleJumping = false;
            isWallJumping = false;
        }
         
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Platform"))
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
