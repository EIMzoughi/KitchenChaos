using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChange;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public event EventHandler OnLocalPlayerReadyChanged;
    private enum State
    {
        WaitingToStart,
        CountDownTostart,
        GamePlaying,
        GameOver,
    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private NetworkVariable<float> _countDownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> _gamePlayingTimer = new NetworkVariable<float>(0f);
    private float _gamePlayingTimerMax = 120f;
    private bool isGamePaused = false;
    private bool isLocalPlayerReady = false;
    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }
    private void Start()
    {
        InputManager.Instance.OnPauseAction += InputManager_OnPauseAction;
        InputManager.Instance.OnAnyKeyAction += InputManager_OnAnyKeyAction;
    }
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChange;
    }

    private void State_OnValueChange(State previousValue, State newValue)
    {
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void InputManager_OnAnyKeyAction(object sender, EventArgs e)
    {
        if(state.Value == State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();
            
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong cliendID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(cliendID) || !playerReadyDictionary[cliendID])
            {
                allClientsReady = false;
                break;
            }
        }
        if(allClientsReady)
        {
            state.Value = State.CountDownTostart;
        }
    }

    private void InputManager_OnPauseAction(object sender, EventArgs e)
    {
        
        PauseGame();
    }

    private void Update()
    {
        if(!IsServer) return;
        switch(state.Value)
        {
            case State.WaitingToStart:            
                break;
            case State.CountDownTostart:
                _countDownToStartTimer.Value -= Time.deltaTime;
                if (_countDownToStartTimer.Value < 0f)
                {
                    state.Value = State.GamePlaying;
                    _gamePlayingTimer.Value = _gamePlayingTimerMax;                    
                }                   
                break;
            case State.GamePlaying:
                _gamePlayingTimer.Value -= Time.deltaTime;
                if (_gamePlayingTimer.Value < 0f)
                {
                    state.Value = State.GameOver;                                   
                }                   
                break;
            case State.GameOver:
                break;
        }
        Debug.Log(state);

    }
    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }
    public bool IsCountDownToStartActive()
    {
        return state.Value == State.CountDownTostart;
    }

    public float GetCountDownTostartTimer()
    {
        return _countDownToStartTimer.Value;
    }

    public bool IsGameOver()
    { 
        return state.Value == State.GameOver;
    }

    public float GetPlayPlayingNormalized()
    {
        return 1 - (_gamePlayingTimer.Value / _gamePlayingTimerMax);
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

    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
        
    }
}
