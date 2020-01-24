using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.PLAYER_JUMP, OnPlayerJump);
        EventManager.AddListener(Events.PLAYER_LANDED, OnPlayerLanded);
        EventManager.AddListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.RemoveListener(Events.PLAYER_JUMP, OnPlayerJump);
        EventManager.RemoveListener(Events.PLAYER_LANDED, OnPlayerLanded);
        EventManager.RemoveListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    void OnPlayerDied()
    {
        _animator.SetTrigger("Shake");
    }
    
    void OnEnemyDied()
    {
        _animator.SetTrigger("Shake");
    }

    void OnPlayerJump()
    {
        _animator.SetTrigger("Jump");
    }

    void OnPlayerLanded()
    {
        _animator.SetTrigger("Landed");
    }
}
