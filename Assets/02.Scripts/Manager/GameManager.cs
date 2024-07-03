using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action OnPlayerDie;
    public event Action OnGameEnded;
    public event Action OnGameStarted;

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
