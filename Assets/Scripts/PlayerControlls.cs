using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControlls : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float maxJumpCount = 2;
    [SerializeField] private float enemyAttackJumpForce = 50;
    [SerializeField] private float fallingGravityScale = 1f; 

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float groundCheckSideOffset = 0.25f;
    
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private SoundEffects _soundEffects;
    
    private bool _isJumpRequested;
    private bool _canStopJump;
    private int _jumpCount;
    private bool _isGrounded;
    private Player _player;
    private float _gravity;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _soundEffects = GetComponent<SoundEffects>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _player.IsFacingRight = true;
        _gravity = _rigidbody2D.gravityScale;

        EventManager.AddListener("AttackEnemy", OnAttackEnemy);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener("AttackEnemy", OnAttackEnemy);
    }

    // Update is called once per frame
    void Update()
    {
        HandleJumping();
        HandleMove();
        HandleFire();
        
        CheckIsGrounded();
    }

    private void FixedUpdate()
    {
        if (_isJumpRequested)
            DoJump();
    }

    void HandleFire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EventManager.TriggerEvent(Events.PLAYER_SHOOT);
    }

    void HandleMove()
    {
        float xAxe = Input.GetAxisRaw("Horizontal");
        
        Vector2 velocity = new Vector2(xAxe * moveSpeed * Time.deltaTime, _rigidbody2D.velocity.y);
        
        _rigidbody2D.velocity = velocity;
        
        _animator.SetFloat("VelocityX", _rigidbody2D.velocity.x);
        _animator.SetFloat("VelocityY", _rigidbody2D.velocity.y);

        if (_rigidbody2D.velocity.x > 0.1f)
            _player.IsFacingRight = true;
        else if (_rigidbody2D.velocity.x < -0.1f)
            _player.IsFacingRight = false;
    }

    void HandleJumping()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && _jumpCount < maxJumpCount)
        {
            // player will jump
            _isJumpRequested = true;
        }
        else if (_jumpCount > 0 && (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)))
        {
            // player is jumping
            StopJump();
        }

        //_rigidbody2D.gravityScale = _rigidbody2D.velocity.y < 0 ? _gravity * fallingGravityScale : _gravity;
    }

    void DoJump()
    {
        Vector2 velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        _rigidbody2D.velocity = velocity;
        
        _rigidbody2D.AddForce(Vector2.up * jumpForce);
        
        _canStopJump = true;
        _isJumpRequested = false;
        _jumpCount++;
        
        if (_jumpCount == 1)
            EventManager.TriggerEvent(Events.PLAYER_JUMP);
        
        _soundEffects.PlayOnJump();
    }

    void StopJump()
    {
        if (!_canStopJump)
            return;

        Vector2 velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        _rigidbody2D.velocity = velocity;
        
        _canStopJump = false;
    }

    void SetGrounded()
    {
        if (!_isGrounded)
            EventManager.TriggerEvent(Events.PLAYER_LANDED);
        
        _isGrounded = true;
        _jumpCount = 0;
        
        _animator.SetBool("IsGrounded", true);
    }

    void CheckIsGrounded()
    {
        Vector2 startPosition = transform.position;
        Vector2 endPosition = groundCheck.position;

        startPosition.x -= groundCheckSideOffset;
        endPosition.x -= groundCheckSideOffset;
        
        Debug.DrawLine(startPosition, endPosition, Color.red);
        RaycastHit2D hit2D = Physics2D.Linecast(startPosition, endPosition, groundLayerMask);

        if (hit2D.collider)
        {
            SetGrounded();
            return;
        }
        
        startPosition.x += groundCheckSideOffset * 2;
        endPosition.x += groundCheckSideOffset * 2;
        
        Debug.DrawLine(startPosition, endPosition, Color.red);
        hit2D = Physics2D.Linecast(startPosition, endPosition, groundLayerMask);

        if (hit2D.collider)
        {
            SetGrounded();
            return;
        }

        _isGrounded = false;
        
        _animator.SetBool("IsGrounded", false);
    }
    
    void OnAttackEnemy()
    {
        Vector2 velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        _rigidbody2D.velocity = velocity;
        
        _rigidbody2D.AddForce(Vector2.up * enemyAttackJumpForce);
        _jumpCount = 0;
    }
}
