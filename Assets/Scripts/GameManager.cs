using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChange;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    private enum State
    {
        WaitingToStart,
        CountDownTostart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float _WaitingToStartTimer = 1f;
    private float _countDownToStartTimer = 3f;
    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 120f;
    private bool isGamePaused = false;


    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }
    private void Start()
    {
        InputManager.Instance.OnPauseAction += InputManager_OnPauseAction;
    }

    private void InputManager_OnPauseAction(object sender, EventArgs e)
    {
        
        PauseGame();
    }

    private void Update()
    {
        switch(state)
        {
            case State.WaitingToStart:
                _WaitingToStartTimer -= Time.deltaTime;
                if( _WaitingToStartTimer < 0f )
                {
                    state = State.CountDownTostart;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                    

                break;
            case State.CountDownTostart:
                _countDownToStartTimer -= Time.deltaTime;
                if (_countDownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    _gamePlayingTimer = _gamePlayingTimerMax;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }                   
                break;
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }                   
                break;
            case State.GameOver:
                break;
        }
        Debug.Log(state);

    }
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }
    public bool IsCountDownToStartActive()
    {
        return state == State.CountDownTostart;
    }

    public float GetCountDownTostartTimer()
    {
        return _countDownToStartTimer;
    }

    public bool IsGameOver()
    { 
        return state == State.GameOver;
    }

    public float GetPlayPlayingNormalized()
    {
        return 1 - (_gamePlayingTimer / _gamePlayingTimerMax);
    }

    public void PauseGame()
    {
        isGamePaused = !isGamePaused;
        if(isGamePaused)
        {
            Time.timeScale = 0;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
        

    }
}
