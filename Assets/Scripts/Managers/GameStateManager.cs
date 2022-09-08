using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private BallLauncher ballLauncher;
    [SerializeField] private GameState _currentState;
    public GameState CurrentState => _currentState;
    public static GameStateManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        EventManager.OnGameLoad += OnGameLoadHandler;
        EventManager.OnGameStart += OnGameStartHandler;
        EventManager.OnGamePause += OnGamePauseHandler;
        EventManager.OnGameLose += OnGameLoseHandler;
        EventManager.OnGameContinue += OnGameContinueHandler;
    }
    private void OnDisable()
    {
        EventManager.OnGameLoad -= OnGameLoadHandler;
        EventManager.OnGameStart -= OnGameStartHandler;
        EventManager.OnGamePause -= OnGamePauseHandler;
        EventManager.OnGameLose -= OnGameLoseHandler;
        EventManager.OnGameContinue -= OnGameContinueHandler;
    }
    private void Start()
    {
        LoadLevel();
    }
    public void LoadLevel()
    {
        SwitchState(GameState.PREGAME);
        UIManager.Instance.SwitchCanvasState(GameCanvas.PREGAME);
        ballLauncher.enabled = false;
        EventManager.RaiseOnGameLoad();
    }
    public void StartLevel()
    {
        OnGameStartHandler();
        
        EventManager.RaiseOnGameStart();
    }
    public void PauseLevel()
    {
        OnGamePauseHandler();
        EventManager.RaiseOnGamePause();
    }
    public void SkinChange()
    {
        UIManager.Instance.SwitchCanvasState(GameCanvas.CHANGESKIN);
    }
    public void ContinueLevel()
    {
        OnGameContinueHandler();
        EventManager.RaiseOnGameContinue();
    }
    private void SwitchState(GameState newState)
    {
        _currentState = newState;
    }
    private void OnGameLoadHandler()
    {
        SwitchState(GameState.PREGAME);
        UIManager.Instance.SwitchCanvasState(GameCanvas.PREGAME);
        ballLauncher.enabled = false;
    }
    private void OnGameStartHandler()
    {
        SwitchState(GameState.INGAME);
        UIManager.Instance.SwitchCanvasState(GameCanvas.INGAME);
        ballLauncher.enabled = true;
        Time.timeScale = 1;
    }
    private void OnGamePauseHandler()
    {
        SwitchState(GameState.PAUSEGAME);
        UIManager.Instance.SwitchCanvasState(GameCanvas.PAUSEGAME);
        Time.timeScale = 0;
    }
    private void OnGameContinueHandler()
    {
        SwitchState(GameState.INGAME);
        UIManager.Instance.SwitchCanvasState(GameCanvas.INGAME);
        Time.timeScale = 1;
    }
    private void OnGameLoseHandler()
    {
        SwitchState(GameState.LOSEGAME);
        UIManager.Instance.SwitchCanvasState(GameCanvas.ENDGAME);
    }
}
public enum GameState
{
    PREGAME,
    INGAME,
    PAUSEGAME,
    LOSEGAME
}
