using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action OnPlayerDie;
    public event Action OnStageTimeOut;
    public event Action OnStageStart;
    public event Action OnStageClear;


    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 30;
    }


    public void NotifyPlayerDie()
    {
        OnPlayerDie?.Invoke();
    }

    public void NotifyStageTimeOut()
    {
        OnStageTimeOut?.Invoke();
    }

    public void NotifyStageStart()
    {
        OnStageStart?.Invoke();
    }

    public void NotifyStageClear()
    {
        OnStageClear?.Invoke();
    }
}
