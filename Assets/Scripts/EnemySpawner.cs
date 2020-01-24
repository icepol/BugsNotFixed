using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject teleport;
    
    [SerializeField] private Enemy[] enemy;
    [SerializeField] private float minDelay = 0.5f;
    [SerializeField] private float maxDelay = 1f;

    private Enemy _spawnedEnemy;
    private Animator _animator;
    private Vector2 _spawnPosition;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            if (_spawnedEnemy == null)
            {
                _spawnPosition = new Vector2(Random.Range(-2f, 2f), 1.4f);

                teleport.transform.localPosition = _spawnPosition;
                
                _animator.SetTrigger("Spawn");
            }
        }
    }

    public void Spawn()
    {
        // called from animator
        _spawnedEnemy = Instantiate(enemy[Random.Range(0, enemy.Length)], transform);
        _spawnedEnemy.transform.localPosition = new Vector2(_spawnPosition.x, 1.4f);
    }
}
