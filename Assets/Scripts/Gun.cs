using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private float reloadTime;

    private float _reloading;
    private Player _player;
    private Animator _animator;
    
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _animator = GetComponent<Animator>();
        
        EventManager.AddListener(Events.PLAYER_SHOOT, OnPlayerShoot);
    }

    private void Update()
    {
        if (_reloading >= 0)
            _reloading -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_SHOOT, OnPlayerShoot);
    }

    void OnPlayerShoot()
    {
        if (_reloading > 0)
            return;

        _reloading = reloadTime;
        
        _animator.SetTrigger("Fire");

        Bullet instance = Instantiate(bullet);
        instance.SetDirection(new Vector2(_player.IsFacingRight ? 1 : -1, 0));

        Vector2 position = transform.position;
        position.x += _player.IsFacingRight ? 0.5f : -0.5f;

        instance.transform.position = position;
        instance.transform.localScale = new Vector2(_player.IsFacingRight ? 1 : -1, 1);
    }
}
