using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static bool isInit;

    public event Action OnPlayerDie;
    public event Action OnStageTimeOut;
    public event Action OnStageStart;
    public event Action OnStageClear;


    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 30;
    }

    public void OnInit()
    {
        StartCoroutine(OnManagerInit());
    }

    public IEnumerator OnManagerInit()
    {
        UILoading.Show();
        //DataManager.Instance.Init();
        //yield return new WaitUntil(() => DataManager.Instance.isInit);
        ResourceManager.Instance.Init();
        yield return new WaitUntil(() => ResourceManager.Instance.isInit);
        isInit = true;
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

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        OnPlayerDie = null;
        OnStageTimeOut = null;
        OnStageStart = null;
        OnStageClear = null;
    }
}
