using System.Collections;
using UnityEngine;

public class Probability : MonoBehaviour
{
    [SerializeField] private TextMesh text;
    
    private int _probability = 0;

    private Animator _animator;
    private Enemy _enemy;
    private Coroutine _coroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponentInParent<Enemy>();
        
        _probability = Random.Range(0, 4) * 20;
    }

    private void Start()
    {
        text.text = _probability + "%";
    }

    private void Update()
    {
        if (_enemy)
            transform.localScale = new Vector3(_enemy.IsFacingRight ? 1 : -1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            _animator.SetTrigger("Show");

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            _coroutine = StartCoroutine(Hide());
        }
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(1f);
        
        _animator.SetTrigger("Hide");
    }

    public int GetProbability()
    {
        return _probability;
    }
}
