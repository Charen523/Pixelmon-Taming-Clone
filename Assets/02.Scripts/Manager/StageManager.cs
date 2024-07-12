using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class StageManager : Singleton<StageManager>
{
    [Header("소환")]
    [SerializeField]
    private Spawner spawner;
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI stageTitleTxt;
    [SerializeField]
    private Image clearBar;
    [SerializeField]
    private TextMeshProUGUI clearTxt;

    [Header("난이도")]
    [SerializeField]
    private string stgRcode;

    [SerializeField]
    public StageData Data {get; private set;}
    public int spawnCount = 0;
    public int killedCount = 0;
    float stageGage;
    float processivity = 0;
    private float bossTimer;
    private float bossLimitTime = 30;
    //보스 처치여부
    public bool isStageClear = false;
    [SerializeField]
    public string CurrentRCode { get; private set; }
    private readonly string stagercode = "STG";
    [SerializeField]
    private int difficultyNum;
    [SerializeField]
    private int maxDifficultyNum;

    [SerializeField]
    private int worldNum;
    [SerializeField]
    private int maxWorldNum = 10;

    [SerializeField]
    private int stageNum;
    [SerializeField]
    private int maxStageNum = 5;


    //리스폰 현재시간
    private float intervalTimer = 0;
    //리스폰 대기시간
    public float spawnInterval = 2f;
    private WaitUntil normalStageCondition;
    private WaitUntil BossStageCondition;
    private WaitForSeconds waitTime = new WaitForSeconds(3);



    //몬스터 사망 시 넘겨받을 정보 액션
    public event Action<EnemyData, GameObject> monsterDead;
    //스테이지 로직
    private Coroutine stageCoroutine;

    //플레이어 사망 중복 호출 방지 플래그
    private bool isPlayerDeadHandled = false;

    void Start()
    {
        normalStageCondition = new WaitUntil(() => NormalStage());
        BossStageCondition = new WaitUntil(() => CheckedBossStage());
        monsterDead += MonsterDead;
        GameManager.Instance.OnPlayerDie += OnPlayerDead;
        GameManager.Instance.OnStageStart += StageInitialize;
        StageInitialize();
        StartCoroutine(StageProcessivity());
    }

    private void SetRcode()
    {
        stgRcode = $"{stagercode}{difficultyNum}{worldNum.ToString("D2")}{stageNum.ToString("D2")}";
    }

    public void StageInitialize()
    {
        LoadData();
        //UI초기화
        InitStageUI();
        SummonMonster();
    }

    private void LoadData()
    {
        Data = DataManager.Instance.GetData<StageData>(stgRcode);
        difficultyNum = Data.difficulty ;
        worldNum = Data.worldId;
        stageNum = Data.stageId;
    }


    #region 몬스터소환
    private void SummonMonster()
    {
        stageCoroutine = StartCoroutine(StartStage());
    }

    public IEnumerator StartStage()
    {        
        //노말 소환
        yield return normalStageCondition;
        if (Data.stageId == maxStageNum)
        {
            yield return waitTime;
            stageNum++;
            SetRcode();
            LoadData();
            stageTitleTxt.text = $"{SetTitleTxt()}{worldNum}- BOSS";
            spawner.RandomSpawnPoint(Data.monsterId, Data.spawnCount);
            clearBar.fillAmount = 1;
            bossTimer = bossLimitTime;
            GameManager.Instance.NotifyStageClear();
            //보스클리어여부
            yield return BossStageCondition;
            //넥스트
            if (isStageClear)
            {
                isStageClear = false;
                ToNextStage();
            }
            else
            {
                ToNextStage(-1);
                GameManager.Instance.NotifyStageTimeOut();
            }
        }
        else
            ToNextStage();
        GameManager.Instance.NotifyStageClear();
        yield return waitTime;
        StageInitialize();
    }

    private bool NormalStage()
    {
        if (Data.nextStageCount <= killedCount)
        {
            ReturnPools();
            return true;
        }

        intervalTimer += Time.deltaTime;
        if (intervalTimer >= spawnInterval)
        {
            //몬스터 최대치 미만 추가 소환
            if (spawnCount < Data.spawnCount)
            {
                spawner.RandomSpawnPoint(Data.monsterId, Data.spawnCount);
                intervalTimer = 0;
            }
        }
        return false;
    }

    private bool CheckedBossStage()
    {
        if (isStageClear)
        {
            ReturnPools();
            return true;
        }

        bossTimer -= Time.deltaTime;
        float percent = bossTimer / bossLimitTime;
        clearBar.fillAmount = percent;
        clearTxt.text = string.Format("{0:F2}초", bossTimer);

        if (bossTimer <= 0)
        {
            clearTxt.text = string.Format("0초");
            clearBar.fillAmount = 0;
            ReturnPools();
            ToNextStage(-1);
            return true;
        }
        return false;
    }


    private void ReturnPools()
    {
        foreach (var enemy in spawner.isActivatedEnemy)
        {
            enemy.SetActive(false);            
        }
        spawner.isActivatedEnemy.Clear();
        spawnCount = 0;
        killedCount = 0;
    }
    #endregion

    #region UI세팅
    private void InitStageUI()
    {
        stageTitleTxt.text = $"{SetTitleTxt()}{worldNum}-{stageNum:D2}";
        clearBar.fillAmount = 0;
        stageGage = 0;
        processivity = 0;
        clearTxt.text = string.Format("0%");
    }
    private string SetTitleTxt()
    {
        switch (difficultyNum)
        {
            case 0:
                return "쉬움";
            case 1:
                return "보통";
            case 2:
                return "어려움";
            default:
                Debug.Log("난이도 미설정");
                return "히든";
        }       
    }
    #endregion

    public void OnMonsterDead(EnemyData enemyData, GameObject enemyGo)
    {
        monsterDead?.Invoke(enemyData, enemyGo);
    }
    
    public void MonsterDead(EnemyData enemyData, GameObject enemyGo)
    {
        if (enemyData.isBoss)
        {
            isStageClear = true;
        }
        else
        {
            killedCount++;
            spawnCount--;
            processivity = (float)killedCount / Data.nextStageCount;
            spawner.isActivatedEnemy.Remove(enemyGo);
        }
        //InventoryManager.Instance.DropItem(enemyData.rewardType, enemyData.rewardRate, enemyData.rewardValue);
    }

    public void OnPlayerDead()
    {
        if (isPlayerDeadHandled) return;

        isPlayerDeadHandled = true;
        
        if (stageCoroutine != null)
        {
            StopCoroutine(stageCoroutine);
        }

        ReturnPools();
        ToNextStage(-1);

        isPlayerDeadHandled = false;
    }

    IEnumerator StageProcessivity()
    {
        float time = 0;
        float duration = 0.5f;
        while (true) 
        {
            if (processivity > stageGage)
            {
                time += Time.deltaTime;
                stageGage = Mathf.Lerp(stageGage, processivity, time / duration);
                clearBar.fillAmount = stageGage;
                clearTxt.text = string.Format("{0:F2}%", stageGage * 100);
            }
            else
            {
                time = 0;
            }
            
            yield return null;
        }
    }

    #region 다음 스테이지
    public void ToNextStage(int index = 1)
    {
        if (index == -1)
            stageNum = 1;
        else if (stageNum <= maxStageNum)
            stageNum += index;
        else
            ToNextWorld(); 
        SetRcode();
    }

    private void ToNextWorld()
    {
        stageNum = 1;
        if (worldNum <= maxWorldNum)
            worldNum++;
        else
            ToNextdifficulty();
    }

    private void ToNextdifficulty()
    {
        worldNum = 1;
        difficultyNum++;
    }
    #endregion
}
