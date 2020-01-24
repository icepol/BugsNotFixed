using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float delayBeforeStart = 0.5f;
    
    [SerializeField] private float speed;
    [SerializeField][Range(0f, 10f)] private float speedVariance = 0;
    
    [SerializeField] private float pauseOnRotation = 0;
    [SerializeField][Range(0f, 10f)] private float pauseVariance = 0;
    
    [SerializeField] private bool randomDirection;

    [SerializeField] private Transform detection;
    [SerializeField] private LayerMask edgeDetectionLayerMask;

    private Enemy _enemy;
    private float _speed;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        StartCoroutine(StartWalking());
    }

    void Update()
    {
        Move();
    }

    IEnumerator StartWalking()
    {
        yield return new WaitForSeconds(delayBeforeStart);
        
        _enemy.IsWalking = true;
        _speed = speed + Random.Range(0f, speedVariance);
        
        if (randomDirection)
            _enemy.IsFacingRight = Random.Range(0, 1) == 1;
    }

    void Move()
    {
        if (!_enemy.IsWalking)
            return;

        // move
        transform.Translate(Time.deltaTime * _speed * (_enemy.IsFacingRight ? Vector2.right : Vector2.left));
        
        // detect the wall
        Vector2 end = detection.position;

        RaycastHit2D hit;
        
        Debug.DrawLine(transform.position, end, Color.red, 1);
        hit = Physics2D.Linecast(transform.position, end, edgeDetectionLayerMask);
        if (hit.collider)
        {
            // there is obstacle in front of enemy
            Rotate();
            return;
        }
        
        // detect ground
        Vector2 start = detection.position;
        end = new Vector2(start.x, start.y - 0.5f);
        
        Debug.DrawLine(start, end, Color.red, 1);
        hit = Physics2D.Linecast(start, end, edgeDetectionLayerMask);
        if (!hit.collider)
        {
            // there is no ground in front of player
            Rotate();
        }
    }

    void Rotate()
    {
        _enemy.IsWalking = false;
        _speed = speed + Random.Range(0f, speedVariance);
        
        StartCoroutine(WaitAndRotate());
    }
    
    IEnumerator WaitAndRotate()
    {
        yield return new WaitForSeconds(pauseOnRotation + Random.Range(0f, pauseVariance));
        
        _enemy.IsFacingRight = !_enemy.IsFacingRight;
        _enemy.IsWalking = true;
    }
}
