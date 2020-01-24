using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Rigidbody2D _targetBody;
    private Transform _target;
    private Vector2 _velocity;

    [SerializeField] float smoothX;
    [SerializeField] float smoothY;
    [SerializeField] float overflow;

    float _xMin;
    float _xMax;

    float _yMin;
    float _yMax;

    private float _width;
    private float _height;
    
    float z;

    private bool _isFollowing;

    private void Awake()
    {
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.LEVEL_START, OnLevelStart);
    }

    private void Start()
    {
        _height = Camera.main.orthographicSize * 2f;
        _width = _height * Camera.main.aspect;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        _target = player.transform;
        _targetBody = player.GetComponent<Rigidbody2D>();

        z = transform.position.z;
        
        // defaults
        _xMin = -_width;
        _xMax = _width;
        _yMin = transform.position.y;
        _yMax = 0;

        _isFollowing = true;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.RemoveListener(Events.LEVEL_START, OnLevelStart);
    }

    private void Update() {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (!_isFollowing)
            return;
        
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = _target.position;

        Vector2 velocity = _targetBody.velocity;

        float targetX = targetPosition.x + velocity.x * 0.5f;
        float targetY = targetPosition.y + velocity.y * 0.5f;

        float x = Mathf.Clamp(
            Mathf.SmoothDamp(currentPosition.x, targetX, ref _velocity.x, smoothX),
            _xMin,
            _xMax
        );

        float y = Mathf.SmoothDamp(currentPosition.y, targetY, ref _velocity.y, smoothY);

        if (y < _yMin)
            y = _yMin;

        transform.position = new Vector3(Round(x), Round(y), z);
    }

    float Round(float value)
    {
        return (int) (value * 50) / 50f;
    }

    void OnLevelStart()
    {
        _isFollowing = true;
    }

    void OnPlayerDied()
    {
        _isFollowing = false;
    }
}