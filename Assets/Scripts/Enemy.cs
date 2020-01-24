using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int lives = 1;
    [SerializeField] private ParticleSystem particleAttack;
    [SerializeField] private ParticleSystem particleDeath;
    [SerializeField] private SpriteRenderer[] bugSpriteRenderer;
    [SerializeField] private float delayAferSpawn = 0.5f;

    private Rigidbody2D _rigidbody;
    private Probability _probability;
    private Animator _animator;
    private Eye[] _eyes;
    private SoundEffects _soundEffects;

    private bool _isWalking;
    private bool _isFacingRight;
    private bool _isDeath;
    private bool _isUnderAttack;
    private bool _willHaveABug;
    private bool _bugApplied;
    private bool _isSpawningFinished;

    public bool IsWalking
    {
        get => _isWalking;
        set
        {
            _isWalking = value;
            
            _animator.SetBool("IsWalking", _isWalking);
        }
    }

    public bool IsFacingRight { 
        get => _isFacingRight;
        set
        {
            _isFacingRight = value;
            
            transform.localScale = new Vector3(_isFacingRight ? 1 : -1, 1, 1);
        }
    }

    private void Awake()
    {
        _probability = GetComponentInChildren<Probability>();
        _animator = GetComponent<Animator>();
        _eyes = GetComponentsInChildren<Eye>();
        _soundEffects = GetComponent<SoundEffects>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (_probability)
            _willHaveABug = Random.Range(0, 100) < _probability.GetProbability();
        
        StartCoroutine(EyeBlick());
        StartCoroutine(FinishSpawning());
    }

    private IEnumerator FinishSpawning()
    {
        yield return new WaitForSeconds(delayAferSpawn);

        _isSpawningFinished = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDeath || _isUnderAttack)
            return;
        
        Player player = other.GetComponent<Player>();
        if (player)
            CollisionWithPlayer(player);

        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet)
            CollisionWithBullet();
    }

    void CollisionWithPlayer(Player player)
    {
        if (_bugApplied)
            return;
        
        if (player.IsFalling)
        {
            if (!_willHaveABug || _bugApplied)
                TakeDamage();
            else
                ApplyBug();
        }
        else if (_isSpawningFinished)
        {
            EventManager.TriggerEvent(Events.ATTACK_PLAYER);
        }
    }
    
    void CollisionWithBullet()
    {
        TakeDamage();
    }

    void TakeDamage()
    {
        lives--;

        if (lives <= 0)
        {
            EventManager.TriggerEvent(Events.INCREASE_SCORE, 10);
            EnemyDied();
        }
        else
        {
            EventManager.TriggerEvent(Events.INCREASE_SCORE, 5);
            EnemyUnderAttack();
        }
        
        EventManager.TriggerEvent(Events.ATTACK_ENEMY);
    }

    IEnumerator EyeBlick()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.5f, 2f));
            
            foreach (Eye eye in _eyes)
                eye.Blick();
        }
    }

    void EnemyUnderAttack()
    {
        _isUnderAttack = true;
        
        Instantiate(particleAttack, transform.position, Quaternion.identity);
        
        if (_soundEffects)
            _soundEffects.PlayUnderAttack();

        StartCoroutine(FinishUnderAttackState());
    }
    
    IEnumerator FinishUnderAttackState()
    {
        yield return new WaitForSeconds(0.2f);
        
        _isUnderAttack = false;
    }
    
    void EnemyDied()
    {
        _isDeath = true;
                
        Instantiate(particleDeath, transform.position, Quaternion.identity);
        
        if (_soundEffects)
            _soundEffects.PlayOnDie();
        
        Destroy(gameObject);
        
        EventManager.TriggerEvent(Events.ENEMY_DIED);
    }
    
    void ApplyBug()
    {
        _bugApplied = true;

        int bug = Random.Range(0, 3);
        switch (bug)
        {
            case 0:
                ChangePosition();
                break;
            case 1:
                ApplyGravity();
                break;
            case 2:
                UpOrDown();
                break;
        }

        foreach (SpriteRenderer spriteRenderer in bugSpriteRenderer)
        {
            spriteRenderer.color = new Color(226f / 256f, 40 / 256f, 65 / 256f);            
        }
    }

    void ChangePosition()
    {
        Vector2 position = transform.position;

        position.x += Random.Range(-2f, 2f);
        position.y += Random.Range(-2f, 2f);

        transform.position = position;
    }

    void ApplyGravity()
    {
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.gravityScale = Random.Range(-1f, 1f);
    }

    void UpOrDown()
    {
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX & RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.gravityScale = Random.Range(-1f, 1f);
    }
}
