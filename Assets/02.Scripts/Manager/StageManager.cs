using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : Singleton<StageManager>
{
    private SaveManager saveManager;
    private UserData userData;
    private StageData data;

    [SerializeField] private Spawner spawner;

    #region UI
    [Header("Stage UI")]
    [SerializeField] private Image StageIcon;
    [SerializeField] private Sprite[] iconSprite;
    [SerializeField] private TextMeshProUGUI stageTitleTxt;

    [SerializeField] private Slider progressSldr;
    [SerializeField] private TextMeshProUGUI progressTxt;
    private float prevProgress;
    private float curProgress;

    [SerializeField] private Slider bossTimeSldr;
    [SerializeField] private TextMeshProUGUI bossTimeTxt;
    private float bossLeftTime;
    private readonly float bossLimitTime = 30;
    #endregion

    #region Monsters
    private string[] monsterIds => data.monsterId;

    [HideInInspector] public int curSpawnCount = 0;
    private int killCount = 0;

    private WaitUntil proceedNormalStg;
    private WaitUntil proceedBossStg;
    private bool isBossCleared = false;
    #endregion

    #region Current Stage
    private string _currentRcode;
    public string CurrentRcode 
    { 
        get
        {
            if (_currentRcode == null || _currentRcode == "")
            {
                _currentRcode = userData.curStageRcode;
            }

            return _currentRcode;
        } 
        
        private set
        {
            _currentRcode = value;
            saveManager.SetFieldData(nameof(userData.curStageRcode), _currentRcode);
        }
    }
    
    [Header("Current Stage")]
    private readonly string stageCodeName = "STG0";
    
    public int diffNum => userData.curDifficulty;

    public int worldNum;
    [SerializeField] private readonly int maxWorldNum = 10;

    public int stageNum;
    [SerializeField] private readonly int bossStageNum = 6;
    private bool isBossStage => stageNum == bossStageNum;
    #endregion

    #region Time Interval
    private float curInterval = 0; //현재 시간 간격
    private readonly float spawnInterval = 2f; //스폰 간격
    private WaitForSeconds nextStageInterval = new WaitForSeconds(3);
    #endregion

    private Coroutine stageCoroutine;

    //플레이어 사망 중복 호출 방지 플래그
    private bool isPlayerDead = false;

    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();

        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
        InitData();
    }

    void Start()
    {
        proceedNormalStg = new WaitUntil(() => NormalStage());
        proceedBossStg = new WaitUntil(() => BossStage());
        GameManager.Instance.OnPlayerDie += OnPlayerDead;
        GameManager.Instance.OnStageStart += DelayedInitStage;

        InitStage();
        StartCoroutine(SetProgressBar());
    }

    private void InitData()
    {
        data = DataManager.Instance.GetData<StageData>(CurrentRcode);
        worldNum = data.worldId;
        stageNum = data.stageId;
        killCount = userData.curStageCount;
    }

    private void ResetRcode()
    {
        CurrentRcode = stageCodeName + worldNum.ToString("D2") + stageNum.ToString("D2");
        data = DataManager.Instance.GetData<StageData>(CurrentRcode);
    }

    public void InitStage()
    {
        InitStageUI();
        SummonMonster();
        if (isBossStage)
        {
            StartCoroutine(SetProgressBar());
        }
    }

    #region 몬스터소환
    private void SummonMonster()
    {
        stageCoroutine = StartCoroutine(StartStage());
    }

    private IEnumerator StartStage()
    {
        if (isBossStage)
        {
            yield return null; //없음 서순차이로 보스 소환 안됨.
            InitBossStage();
            yield return proceedBossStg;
            bossTimeSldr.gameObject.SetActive(false);

            if (isBossCleared)
            {
                //현재 월드의 Normal Monster Pool 삭제.
                //foreach (var monsterId in monsterIds)
                //{
                //    PoolManager.Instance.RemovePool(monsterId);
                //}

                isBossCleared = false;
                ToNextWorld();
            }
            else
            {
                ToNextStage(false);
                GameManager.Instance.NotifyStageTimeOut();
                yield break;
            }
        }
        else
        {
            yield return proceedNormalStg;

            ToNextStage();
            GameManager.Instance.NotifyStageClear();
        }

        Player.Instance.fsm.target = null; //target 초기화
        yield return nextStageInterval;
        InitStage();
    }

    /// <summary>
    /// 노말 스테이지일 때 몬스터 스폰시켜주기
    /// </summary>
    /// <returns>노말스테이지 종료 여부</returns>
    private bool NormalStage() 
    {
        if (killCount >= data.nextStageCount)
        {
            // Stage Clear
            ResetSpawnedEnemy();
            RewardManager.Instance.GetRewards(data.rewardType, data.rewardValue);
            return true;
        }

        if (isBossStage)
        {
            return true;
        }

        curInterval += Time.deltaTime;

        if (curInterval >= spawnInterval)
        {
            // 몬스터 최대치 미만 추가 소환
            if (curSpawnCount < data.spawnCount)
            {
                spawner.SpawnMonsterTroop(data.monsterId, data.spawnCount);
                curInterval = 0;
            }
        }

        return false;
    }

    private void InitBossStage()
    {
        //PoolManager.Instance.AddPool(Data.monsterIds);
        spawner.SpawnMonsterTroop(data.monsterId, data.spawnCount);
        bossLeftTime = bossLimitTime;
    }

    private bool BossStage()
    {
        if (isBossCleared)
        {
            ResetSpawnedEnemy();
            RewardManager.Instance.GetRewards(data.rewardType, data.rewardValue);
            return true;
        }

        bossLeftTime = Mathf.Max(0, bossLeftTime - Time.deltaTime);
        float percent = bossLeftTime / bossLimitTime;
        bossTimeSldr.value = percent;
        bossTimeTxt.text = string.Format("{0:F2}", bossLeftTime);

        if (bossLeftTime == 0 || isPlayerDead)
        {
            ResetSpawnedEnemy();
            return true;
        }

        return false;
    }

    private void ResetSpawnedEnemy()
    {
        foreach (var enemy in spawner.isActivatedEnemy)
        {
            enemy.gameObject.SetActive(false);            
        }
        spawner.isActivatedEnemy.Clear();
        curSpawnCount = 0;
        killCount = 0;
    }
    #endregion

    #region UI세팅
    private void InitStageUI()
    {
        if (isBossStage)
        {
            stageTitleTxt.text = $"{SetDiffTxt(diffNum)} {worldNum}-BOSS";
            StageIcon.sprite = iconSprite[1];
            progressSldr.value = 1;
            progressTxt.text = "100.00%";
            bossTimeSldr.gameObject.SetActive(true);
        }
        else
        {
            stageTitleTxt.text = $"{SetDiffTxt(diffNum)} {worldNum}-{stageNum}";
            StageIcon.sprite = iconSprite[0];
            prevProgress = killCount / (float)data.nextStageCount;
            curProgress = prevProgress;
            progressSldr.value = prevProgress;
            progressTxt.text = string.Format("{0:F2}%", prevProgress * 100);
        }
    }

    public string SetDiffTxt(int num)
    {
        return num switch
        {
            0 => "쉬움",
            1 => "보통",
            2 => "어려움",
            3 => "매우 어려움",
            _ => "지옥"
        };
    }

    private IEnumerator SetProgressBar() 
    {
        while (true)
        {
            if (curProgress > prevProgress)
            {
                StartCoroutine(UIUtils.AnimateSliderChange(progressSldr, prevProgress, curProgress));
                progressTxt.text = string.Format("{0:F2}%", curProgress * 100);
                prevProgress = curProgress;
            }
            yield return null;
        }
    }

    public Slider GetBossSlider()
    {
        return progressSldr;
    }

    public TextMeshProUGUI GetBossHpText()
    {
        return progressTxt;
    }
    #endregion

    #region Next Stage/World/Diff
    public void ToNextStage(bool isClear = true)
    {
        saveManager.SetFieldData(nameof(userData.curStageCount), 0);

        if (!isClear)
        {
            stageNum = 1;
        }
        else 
        {
            stageNum++;
        }

        ResetRcode();
    }

    /// <summary>
    ///  마지막 world면 diff도 함꼐 바뀜!
    /// </summary>
    private void ToNextWorld()
    {
        stageNum = 1;

        if (worldNum < maxWorldNum)
        {
            worldNum++;
        }
        else
        {
            worldNum = 1;
            saveManager.SetFieldData(nameof(userData.curDifficulty), 1, true);
        }

        ResetRcode();

        if (worldNum % 2 == 0 && QuestManager.Instance.isStageQ)
        {
            QuestManager.Instance.OnQuestEvent();
        }
    }
    #endregion

    #region Death Events
    public void MonsterDead(Enemy enemy)
    {
        EnemyData enemyData = enemy.statHandler.data;

        if (enemyData.isBoss)
        {
            isBossCleared = true;
        }
        else
        {
            killCount++;
            curSpawnCount--;
            curProgress = Mathf.Min((float)killCount / data.nextStageCount, 100f);
            spawner.isActivatedEnemy.Remove(enemy);
        }
        RewardManager.Instance.GetRewards(enemyData.rewardType, enemyData.rewardValue, enemyData.rewardRate);
        
        if (QuestManager.Instance.isMonsterQ)
        {
            QuestManager.Instance.OnQuestEvent();
        }

        saveManager.SetFieldData(nameof(userData.curStageCount), 1, true);
    }

    public void OnPlayerDead()
    {
        if (isPlayerDead) return;
        isPlayerDead = true;
        
        if (stageCoroutine != null)
        {
            StopCoroutine(stageCoroutine);
        }

        ResetSpawnedEnemy();
        ToNextStage(false);
        Player.Instance.fsm.target = null; //target 초기화
        bossTimeSldr.gameObject.SetActive(false);       
    }

    private void DelayedInitStage()
    {
        isPlayerDead = false;
        InitStage();
    }
    #endregion
}