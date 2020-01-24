using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleDeath;
    [SerializeField] private ParticleSystem particleDust;
    
    private Rigidbody2D _rigidbody2D;
    private Eye[] _eyes;
    private SoundEffects _soundEffects;
    private bool _isFacingRight;
    private Vector2 _lastPosition;

    public bool IsFacingRight
    {
        get => _isFacingRight;
        
        set
        {
            _isFacingRight = value;
            
            transform.localScale = new Vector2(_isFacingRight ? 1 : -1, 1);            
        }
    }
    
    public bool IsFalling => _rigidbody2D.velocity.y < 0;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _eyes = GetComponentsInChildren<Eye>();
        _soundEffects = GetComponent<SoundEffects>();
    }

    void Start()
    {
        EventManager.AddListener(Events.ATTACK_PLAYER, OnAttackPlayer);
        EventManager.AddListener(Events.PLAYER_JUMP, OnPlayerJump);
        EventManager.AddListener(Events.PLAYER_LANDED, OnPlayerLanded);
        EventManager.AddListener(Events.PLAYER_SHOOT, OnPlayerShoot);

        StartCoroutine(EyeBlick());

        _lastPosition = transform.position;
    }

    private void Update()
    {
        if (Mathf.Abs(Vector2.Distance(transform.position, _lastPosition)) > 0.01f)
        {
            _lastPosition = transform.position;
            
            EventManager.TriggerEvent(Events.PLAYER_POSITION_CHANGE, _lastPosition);
        }
    }

    // Update is called once per frame
    void OnDestroy()
    {
        EventManager.RemoveListener(Events.ATTACK_PLAYER, OnAttackPlayer);
        EventManager.RemoveListener(Events.PLAYER_JUMP, OnPlayerJump);
        EventManager.RemoveListener(Events.PLAYER_LANDED, OnPlayerLanded);
        EventManager.RemoveListener(Events.PLAYER_SHOOT, OnPlayerShoot);
    }

    void OnAttackPlayer()
    {
        _soundEffects.PlayOnDie();
        
        Instantiate(particleDeath, transform.position, Quaternion.identity);
        
        EventManager.TriggerEvent(Events.PLAYER_DIED);
        
        Destroy(gameObject);
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

    void OnPlayerJump()
    {
        if (particleDust)
        {
            ParticleSystem particle = Instantiate(particleDust);

            particle.gameObject.transform.position = new Vector2(
                transform.position.x, transform.position.y);
        }
    }

    void OnPlayerLanded()
    {
        if (particleDust)
        {
            ParticleSystem particle = Instantiate(particleDust);

            particle.gameObject.transform.position = new Vector2(
                transform.position.x, transform.position.y);
        }
    }

    void OnPlayerShoot()
    {
        _soundEffects.PlayOnFire();
    }
}
