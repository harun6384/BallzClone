using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action OnBallReturn;
    public static void RaiseOnBallReturn() => OnBallReturn?.Invoke();

    public static Action OnGameLoad;
    public static void RaiseOnGameLoad() => OnGameLoad?.Invoke();

    public static Action OnGamePause;
    public static void RaiseOnGamePause() => OnGamePause?.Invoke();

    public static Action OnGameStart;
    public static void RaiseOnGameStart() => OnGameStart?.Invoke();

    public static Action OnGameContinue;
    public static void RaiseOnGameContinue() => OnGameContinue?.Invoke();

    public static Action OnGameLose;
    public static void RaiseOnGameLose() => OnGameLose?.Invoke();

    public static Action OnAddBall;
    public static void RaiseOnAddBall() => OnAddBall?.Invoke();
}
