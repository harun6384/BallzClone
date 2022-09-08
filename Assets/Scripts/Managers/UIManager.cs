using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<Canvas> gameCanvases;
    [SerializeField] private TextMeshProUGUI levelScoreText;
    [SerializeField] private TextMeshProUGUI endScoreText;
    private GameCanvas _currentCanvas;
    private int _number;
    [SerializeField] private BlockSpawner spawner;
    public GameCanvas CurrentCanvas
    {
        get { return _currentCanvas; }
        set { CurrentCanvas = _currentCanvas; }
    }
    public void SwitchCanvasState(GameCanvas newCanvas)
    {
        canvasStates = newCanvas;
    }
    public static UIManager Instance { get; private set; }
    public GameCanvas canvasStates { get; private set; }
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
        _number = 0;
    }
    private void OnEnable()
    {
        EventManager.OnBallReturn += OnBallReturnHandler;
    }
    private void OnDisable()
    {
        EventManager.OnBallReturn -= OnBallReturnHandler;
    }
    private void Update()
    {
        SwitchCanvasStateCases();
    }
    private void SwitchCanvasStateCases()
    {
        switch (canvasStates)
        {
            case GameCanvas.PREGAME:
                _number = 0;
                break;
            case GameCanvas.INGAME:
                _number = 1;
                SetLevelScoreText();
                break;
            case GameCanvas.PAUSEGAME:
                _number = 2;
                break;
            case GameCanvas.ENDGAME:
                _number = 3;
                SetEndScoreText();
                break;
            case GameCanvas.CHANGESKIN:
                _number = 4;
                break;
            default:
                break;
        }

        ActivateCanvases(_number);
    }
    private void ActivateCanvases(int number)
    {
        for (int i = 0; i < gameCanvases.Count; i++)
        {
            if (number == i)
            {
                gameCanvases[number].enabled = true;
            }
            else
            {
                gameCanvases[i].enabled = false;
            }
        }
    }
    private void OnBallReturnHandler()
    {
        if (canvasStates == GameCanvas.INGAME)
        {
            SetLevelScoreText();
        }
    }
    private void SetLevelScoreText()
    {
        levelScoreText.text = spawner.RowsSpawned.ToString();
    }
    private void SetEndScoreText()
    {
        endScoreText.text = spawner.RowsSpawned.ToString();
    }
}
public enum GameCanvas
{
    PREGAME,
    CHANGESKIN,
    INGAME,
    PAUSEGAME,
    ENDGAME
}
