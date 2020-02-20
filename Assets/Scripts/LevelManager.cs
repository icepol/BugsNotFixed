using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text killedText;
    [SerializeField] private Text heightText;

    private bool _isGameOver;

    private int _score;
    private float _height;
    private int _killed;

    private Animator _scoreAnimator;
    private Animator _heightAnimator;
    private Animator _killedAnimator;
    
    private int Score
    {
        get => _score;
        set
        {
            if (_score != value)
                _scoreAnimator.SetTrigger("Change");
            
            _score = value;

            scoreText.text = (_score + (int)_height).ToString();
        }
    }
    
    private float Height
    {
        get => _height;
        set
        {
            if (_height != value)
                _heightAnimator.SetTrigger("Change");
            
            _height = value;

            heightText.text = ((int)_height).ToString();
        }
    }
    
    private int Killed
    {
        get => _killed;
        set
        {
            if (_killed != value)
                _killedAnimator.SetTrigger("Change");
            
            _killed = value;

            killedText.text = _killed.ToString();
        }
    }

    private void Awake()
    {
        EventManager.AddListener(Events.INCREASE_SCORE, OnIncreaseScore);
        EventManager.AddListener(Events.LEVEL_START, OnLevelStart);
        EventManager.AddListener(Events.LEVEL_FAILED, OnLevelFailed);
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.ENEMY_DIED, OnEnemyDied);
        EventManager.AddListener(Events.PLAYER_POSITION_CHANGE, OnPlayerPositionChange);

        _scoreAnimator = scoreText.GetComponent<Animator>();
        _killedAnimator = killedText.GetComponent<Animator>();
        _heightAnimator = heightText.GetComponent<Animator>();
    }

    void Start()
    {
        Score = 0;
        Killed = 0;
        Height = 0;

        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        
        EventManager.TriggerEvent(Events.LEVEL_START);
    }

    private void Update()
    {
        if (_isGameOver && Input.anyKey)
            OnPlayAgainButtonClick();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.INCREASE_SCORE, OnIncreaseScore);
        EventManager.RemoveListener(Events.LEVEL_START, OnLevelStart);
        EventManager.RemoveListener(Events.LEVEL_FAILED, OnLevelFailed);
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.RemoveListener(Events.ENEMY_DIED, OnEnemyDied);
        EventManager.RemoveListener(Events.PLAYER_POSITION_CHANGE, OnPlayerPositionChange);
    }
    
    public void OnPlayAgainButtonClick()
    {
        SceneManager.LoadScene("Game");
    }

    void OnLevelStart()
    {
        AnalyticsEvent.GameStart();
        
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    void OnLevelFailed()
    {
        _isGameOver = true;
        
        AnalyticsEvent.GameOver();
        
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(true);
    }

    void OnIncreaseScore(int score)
    {
        Score += score;
    }

    void OnPlayerDied()
    {
        StartCoroutine(LevelFailed());
    }

    IEnumerator LevelFailed()
    {
        yield return new WaitForSeconds(0.5f);
        
        EventManager.TriggerEvent(Events.LEVEL_FAILED);
    }

    void OnEnemyDied()
    {
        Killed++;
    }

    void OnPlayerPositionChange(Vector3 playerPosition)
    {
        float currentY = playerPosition.y + 7.5f;

        if (currentY > Height)
        {
            Height = currentY;
            Score += 0;  // will rerender score component
        }
            
    }
}
