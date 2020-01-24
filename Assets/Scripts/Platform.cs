using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Platform : MonoBehaviour
{
    private Probability _probability;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private bool _willHaveABug;
    private bool _bugApplied;

    private void Awake()
    {
        _probability = GetComponentInChildren<Probability>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (_probability)
            _willHaveABug = Random.Range(0, 100) < _probability.GetProbability();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_probability || !_willHaveABug || _bugApplied)
            return;
        
        Player player = other.GetComponent<Player>();
        if (player && player.IsFalling)
            ApplyBug();
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
        
        _spriteRenderer.color = new Color(226f / 256f, 40 / 256f, 65 / 256f);
    }

    void ChangePosition()
    {
        Vector2 position = transform.position;

        position.x += Random.Range(-2f, 2f);
        position.y += Random.Range(-2f, 0);

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
