using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action OnPlayerDie;
    public event Action OnGameEnded;
    public event Action OnGameStarted;


    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 30;
    }


    public void NotifyPlayerDie()
    {
        OnPlayerDie?.Invoke();
    }

    public void NotifyGameEnded()
    {
        OnGameEnded?.Invoke();
    }

    public void NotifyGameStarted()
    {
        OnGameStarted?.Invoke();
    }
}
