using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
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

    private float timer = 0;
    public float spawnInterval = 2f;
    private WaitUntil normalStageCondition;
    private WaitUntil BossStageCondition;
    private WaitForSeconds waitTime = new WaitForSeconds(3);

    public event Action<EnemyData, GameObject> monsterDead;
    private Coroutine stageCorutine;
    void Start()
    {
        normalStageCondition = new WaitUntil(() => NormalStage());
        BossStageCondition = new WaitUntil(() => BossStage());
        monsterDead += MonsterDead;
        GameManager.Instance.OnPlayerDie += OnPlayerDead;
        StageInitialize();
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
        stageCorutine = StartCoroutine(StartStage());
    }

    public IEnumerator StartStage()
    {        
        //노말 소환
        yield return normalStageCondition;
        if (Data.stageId == maxStageNum)
        {
            /*TODO: 보스 rcode처리해야함*/
            //spawner.RandomSpawnPoint(Data.bossId, Data.spawnCount);

            //보스클리어여부
            yield return BossStageCondition;
            //넥스트
            if (isStageClear)
            {
                isStageClear = false;
                ToNextStage();
            }
            else
                ToNextStage(-1);
        }
        else
            ToNextStage();
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

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            //몬스터 최대치 미만 추가 소환
            if (spawnCount < Data.spawnCount)
            {
                spawner.RandomSpawnPoint(Data.monsterId, Data.spawnCount);
                timer = 0;
            }
        }
        return false;
    }

    private bool BossStage()
    {
        if (isStageClear)
        {
            ReturnPools();
            return true;
        }

        //시간체크

        return false;
    }
    private void ReturnPools()
    {
        foreach (var enemy in spawner.isActivatedEnemy)
        {
            enemy.SetActive(false);
            spawner.isActivatedEnemy.Remove(enemy);
        }
    }
    #endregion

    #region UI세팅
    private void InitStageUI()
    {
        stageTitleTxt.text = $"{SetTitleTxt()}{worldNum}-{stageNum:D2}";
        clearBar.fillAmount = 0;
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
            float percent = killedCount / Data.nextStageCount;
            clearBar.fillAmount = percent;
            clearTxt.text = string.Format("{0}%", percent);
            spawner.isActivatedEnemy.Remove(enemyGo);
        }
    }

    public void OnPlayerDead()
    {
        StopCoroutine(stageCorutine);
        ToNextStage(-1);
        StageInitialize();
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
