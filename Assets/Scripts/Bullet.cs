using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 _direction;

    // Update is called once per frame
    void Update()
    {
        if (_direction != null)
            transform.Translate(_direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
}
