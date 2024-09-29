using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : Entity
{
    public Rigidbody2D rg;
    public Animator animator;
    public BoxCollider2D playerCollider;
    

    public float walkSpeed;
    [Tooltip("Velocidade aplicada durante o dash.")]
    public float dashSpeed;
    [Tooltip("Duração do dash em segundos.")]
    public float dashDuration;
    [Tooltip("Tempo de recarga em segundos antes que o dash possa ser usado novamente.")]
    public float dashCooldown;
    [Tooltip("Janela de tempo para detectar um duplo toque na mesma tecla.")]
    public float doubleTapTimeWindow = 0.2f;
    public float runSpeed;
    public float jumpForce;
    public float fallMultiplier;
    public float wallJumpForce;
    public bool isOnPlatform;
    public bool isGrounded;
    public bool isWalled;
    public bool isDashing;
    public bool isJumping;
    public bool isJumpingDown;
    public bool isRunning;
    public bool isWallJumping;
    public bool isDoubleJumping;
    public bool isChrouching;
    private ColliderSystem _colliderSystem;

    
    // Variáveis para detectar duplo toque
    private float _lastTapTimeD;
    private float _lastTapTimeA;

    public float dashTime;
    public float dashCooldownTime;
    
    
    

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    public float groundCheckRadius;
    public float wallCheckDistance;
    
    private Vector2 _crouchSize;
    private Vector2 _crouchOffset;
    
    private Vector2 _originalSize;
    private Vector2 _originalOffset;
    
    
    // Essas 2 variaveis abaixo é só pra ver o cubo "agachando", quando for o sprite mesmo a gente usa o sprite agachando
    private Vector3 _originalScale;
    private Vector3 _crouchScale;
    private Vector2 _movement;
    private SpriteRenderer _spriteRenderer;

    public DirectionMove directionMovement;
    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        directionMovement = DirectionMove.Right;
        
        playerCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        _originalOffset = playerCollider.offset;
        _originalSize = playerCollider.size;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        _crouchSize = new Vector2(_originalSize.x, _originalSize.y / 2);
        _crouchOffset = new Vector2(_originalOffset.x, _originalSize.y / 2);

        _colliderSystem = GetComponent<ColliderSystem>();
        _originalScale = transform.localScale;
        _crouchScale = new Vector3(_originalScale.x, _originalScale.y / 2, _originalScale.z);
    }

    private void Update()
    {
        
        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");
        isGrounded = _colliderSystem.IsGrounded(groundCheck);
        
        // Raycast para a esquerda e direita
        
        bool isTouchingWallRight = _colliderSystem.IsTouchingWall(wallCheck, Vector2.right);
        bool isTouchingWallLeft = false;
        // Pra evitar ficar lançando raycast, só vai ver se ele esta tocando em uma parede na esquerda caso não tenha detectado 
        // colisão na direita, ja que acredito eu que vão ter mais colisões pra direita do que pra esquerda
        if (!isTouchingWallRight)
        {
            isTouchingWallLeft = _colliderSystem.IsTouchingWall(wallCheck, Vector2.left);
        }
        isWalled = isTouchingWallLeft || isTouchingWallRight;
        if (isWalled)
        {
            _spriteRenderer.flipX = false;
            switch (directionMovement)
            { 
                    
                case DirectionMove.Right:
                    if(animator.GetCurrentAnimatorStateInfo(0).IsName("WallSlideRight"))
                        return;
                    
                    animator.Play("WallSlideRight");
                    Debug.Log("Wall Slide direita");
                    break;
                case DirectionMove.Left:
                    if(animator.GetCurrentAnimatorStateInfo(0).IsName("WallSlideLeft"))
                        return;
                    animator.Play("WallSlideLeft");
                    Debug.Log("Wall Slide esquerda");
                    break;
            }       
        }
        else
        {
            rg.gravityScale = 1;
            isWalled = false;
        }
        
        if (isGrounded)
        {
            isDoubleJumping = false;
        }

        if (Input.GetKey(KeyCode.S))
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(JumpDown());
            }
            else
            {
                Crouch();    
            }
            return;
        }

        if (isChrouching)
        {
            Standup();            
        }

        DetectDash();

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isOnPlatform || isWalled ))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isJumping && (!isGrounded && !isOnPlatform) && !isDoubleJumping)
        {
            DoubleJump();
        }
        if (Input.GetKey(KeyCode.LeftShift) && _movement.x is > 0 or < 0 )
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void DetectDash()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - _lastTapTimeD < doubleTapTimeWindow && !isDashing && Time.time >= dashCooldownTime)
            {
                Debug.Log("Dashing Right");
                Dash(Vector2.right);
            }
            _lastTapTimeD = Time.time;
        }

        // Detecta duplo toque na tecla "A"
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - _lastTapTimeA < doubleTapTimeWindow && !isDashing && Time.time >= dashCooldownTime)
            {
                Debug.Log("Dashing Left");
                Dash(Vector2.left);
            }
            _lastTapTimeA = Time.time;
        }

        if (isDashing)
        {
            // Se o player apertar na direção oposta ao movimento do dash, cancela o mesmo
            if ((Input.GetKey(KeyCode.A) && rg.velocity.x > 0) || (Input.GetKey(KeyCode.D) && rg.velocity.x < 0) || Time.time >= dashTime)
            {
                EndDash();
            }
        }
    }

    private void Crouch()
    {
        // Em tese essas 4 linhas vão funcionar quando tivermos um sprite de agachamento
        // O size reduz o tamanho do colisor e o offset acredito que não vai precisar
        // mas de todo modo vou deixar ele comentado por que pode ser que ajude
        isChrouching = true;
        transform.localScale = _crouchScale;
        // playerCollider.size = crouchScale;
        // playerCollider.offset = new Vector2(0,-0.25f);
    }

    private void Standup()
    {
        isChrouching = false;
        transform.localScale = _originalScale;
        playerCollider.size = _originalSize;
        playerCollider.offset = _originalOffset;
    }

    private IEnumerator JumpDown()
    {
        // Se não estiver no chão
        if (!isGrounded) yield break;
        // Se não estiver em cima de uma plataforma
        if(!isOnPlatform) yield break;
        Debug.Log("JUMP DOWN");
        isJumpingDown = true;
        // A ideia aqui é desligar e ligar a colisão por alguns milisegundos
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Map"), true);
        yield return new WaitForSeconds(0.2f);
        isJumpingDown = false;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Map"), false);

    }

    private void FixedUpdate()
    { 
        switch (_movement.x)
        {
            case > 0:
                directionMovement = DirectionMove.Right;
                if (!isWalled)
                {
                    _spriteRenderer.flipX = false;                    
                }
                break;
            case < 0:
                directionMovement = DirectionMove.Left;
                if (!isWalled)
                {
                    _spriteRenderer.flipX = true;    
                }
                
                break;
        }

        if (isGrounded && !isJumping)
        {
            rg.gravityScale = 1;
        }
        if (isWalled && !isGrounded && rg.velocity.y < 0)
        {
            rg.gravityScale = 3f;
            rg.velocity = new Vector2(rg.velocity.x, -0.5f);
        }
        if (isJumpingDown)
        {
            rg.gravityScale = 4;    
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
            if (!isDashing)
            {
                Walk();    
            }
        }
    }

    private void Walk()
    {
        
        float move = Input.GetAxis("Horizontal");
        if (move == 0)
        {
            if (isJumping || isDashing || isDoubleJumping || isWalled)
            {
                return;
            }
            Debug.Log("Idling");
            animator.Play("Idle");
            return;
        }
        
        Debug.Log("Walking");
        rg.velocity = new Vector2(move * walkSpeed, rg.velocity.y);
        rg.velocity.Normalize();
        Debug.Log($"Speed: {animator.speed}");
        // Não ativa a animação se o player estiver em algum desses estados
        if (isJumping || isDashing || isDoubleJumping || isWalled)
        {
            return;
        }
        animator.speed = 0.8f;
        animator.Play("Run");
    }

    private void Dash(Vector2 direction)
    {
        isDashing = true;
        dashTime = Time.time + dashDuration;
        dashCooldown = Time.time + dashCooldown;

        rg.velocity = direction * dashSpeed;
    }

    void EndDash()
    {
        isDashing = false;
        rg.velocity = Vector2.zero;

        // Faz com que o jogador continue caminhando caso o player esteja apertando algum botão de movimento
        if (_movement.x != 0)
        {
            rg.velocity = new Vector2(_movement.x * walkSpeed, rg.velocity.y);
        }
    }

    private void Run()
    {
        float move = Input.GetAxis("Horizontal");
        rg.velocity = new Vector2(move * runSpeed, rg.velocity.y);
        Debug.Log("Running");
        animator.speed = 1f;
        animator.Play("Run");
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
            isGrounded = true;
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
            // isGrounded = false;
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
