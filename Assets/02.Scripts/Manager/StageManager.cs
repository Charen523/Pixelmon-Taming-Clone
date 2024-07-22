using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] public StageData Data {get; private set;}
    [SerializeField] private Spawner spawner;

    #region UI
    [Header("Stage UI")]
    [SerializeField] private Image StageIcon;
    [SerializeField] private Sprite[] iconSprite;
    [SerializeField] private TextMeshProUGUI stageTitleTxt;

    [SerializeField] private Slider progressSldr;
    [SerializeField] private TextMeshProUGUI progressTxt;

    [SerializeField] private Slider bossTimeSldr;
    [SerializeField] private TextMeshProUGUI bossTimeTxt;
    #endregion
    
    #region Spawn Monsters
    private string monsterIds;

    public int curSpawnCount = 0;
    public int killCount = 0;

    float prevProgress;
    float curProgress = 0;

    private float bossLeftTime;
    private float bossLimitTime = 30;
    public bool isBossCleared = false;
    #endregion

    #region Current Stage
    private string _currentRcode;
    public string CurrentRcode 
    { 
        get
        {
            if (_currentRcode == null || _currentRcode == "")
            {
                _currentRcode = InventoryManager.Instance.userData.curStageRcode;
            }

            return _currentRcode;
        } 
        
        private set
        {
            _currentRcode = value;
            InventoryManager.Instance.SetData("curStageRcode", value);
        }
    }
    
    [Header("Current Stage")]
    private readonly string stageCodeName = "STG";
    
    public int diffNum;

    [SerializeField] private int worldNum;
    [SerializeField] private int maxWorldNum = 10;

    public int stageNum;
    [SerializeField] private int maxStageNum = 5;
    #endregion

    #region Time Interval
    [Header("Time Interval")]
    private float curInterval = 0; //현재 시간 간격
    public readonly float spawnInterval = 2f; //스폰 간격
    private WaitUntil executeNormalStg;
    private WaitUntil IsBossStgEnd;
    private WaitUntil playerTargetIsNull;
    private WaitForSeconds nextStageInterval = new WaitForSeconds(3);
    #endregion

    //스테이지 로직
    private Coroutine stageCoroutine;

    //플레이어 사망 중복 호출 방지 플래그
    private bool isPlayerDeadHandled = false;

    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();
        LoadData();
    }

    void Start()
    {
        executeNormalStg = new WaitUntil(() => NormalStage());
        IsBossStgEnd = new WaitUntil(() => BossStage());
        GameManager.Instance.OnPlayerDie += OnPlayerDead;
        GameManager.Instance.OnStageStart += InitStage;

        //임시 코드
        if (stageNum == 6)
        {
            CurrentRcode = "STG00105";
            stageNum--;
            killCount = 10;
        }

        InitStage();
        StartCoroutine(SetProgressBar());
    }

    private void LoadData()
    {
        Data = DataManager.Instance.GetData<StageData>(CurrentRcode);
        diffNum = Data.difficulty;
        worldNum = Data.worldId;
        stageNum = Data.stageId;
        killCount = InventoryManager.Instance.userData.curStageCount;
    }

    private void SetRcode()
    {
        CurrentRcode = $"{stageCodeName}{diffNum}{worldNum.ToString("D2")}{stageNum.ToString("D2")}";
        LoadData();
    }

    public void InitStage()
    {        
        //UI초기화
        InitStageUI();
        SummonMonster();
    }

    #region 몬스터소환
    private void SummonMonster()
    {
        stageCoroutine = StartCoroutine(StartStage());
    }

    public IEnumerator StartStage()
    {
        yield return executeNormalStg;
        Player.Instance.fsm.target = null; //targer 초기화

        if (stageNum == maxStageNum)
        {
            yield return nextStageInterval;
            InitBossStg();
            yield return IsBossStgEnd;
            
            if (isBossCleared)
            {
                isBossCleared = false;
                PoolManager.Instance.RemovePool(monsterIds);
                PoolManager.Instance.RemovePool(Data.monsterIds);
                ToNextWorld();
            }
            else
            {
                ToNextStage(-1);
                GameManager.Instance.NotifyStageTimeOut();
            }
        }
        else
        {
            ToNextStage();
        }
        GameManager.Instance.NotifyStageClear();
        yield return nextStageInterval;
        InitStage();
    }

    private bool NormalStage()
    {
        if (killCount >= Data.nextStageCount)
        {//Stage Clear
            ResetSpawnedEnemy();
            return true;
        }

        if (Data.spawnCount == 1)
        {//Boss Stage 조건
            return true;
        }

        curInterval += Time.deltaTime;

        if (curInterval >= spawnInterval)
        {
            //몬스터 최대치 미만 추가 소환
            if (curSpawnCount < Data.spawnCount)
            {
                spawner.RandomSpawnPoint(Data.monsterId, Data.spawnCount);
                curInterval = 0;
            }
        }

        return false;
    }

    private bool BossStage()
    {
        if (isBossCleared)
        {
            ResetSpawnedEnemy();
            bossTimeSldr.gameObject.SetActive(false);
            bossTimeTxt.gameObject.SetActive(false);
            return true;
        }

        bossLeftTime = Mathf.Max(0,  bossLeftTime - Time.deltaTime);
        float percent = bossLeftTime / bossLimitTime;
        bossTimeSldr.value = percent;
        bossTimeTxt.text = string.Format("{0:F2}", bossLeftTime);

        if (bossLeftTime == 0)
        {
            ResetSpawnedEnemy();
            ToNextStage(-1);
            bossTimeSldr.gameObject.SetActive(false);
            bossTimeTxt.gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    private void ResetSpawnedEnemy()
    {
        foreach (var enemy in spawner.isActivatedEnemy)
        {
            enemy.SetActive(false);            
        }
        spawner.isActivatedEnemy.Clear();
        curSpawnCount = 0;
        killCount = 0;
    }
    #endregion

    #region UI세팅
    private void InitStageUI()
    {
        stageTitleTxt.text = $"{SetDiffTxt()} {worldNum}-{stageNum}";
        StageIcon.sprite = iconSprite[0];
        prevProgress = killCount / (float)Data.nextStageCount;
        curProgress = prevProgress;
        progressSldr.value = prevProgress;
        progressTxt.text = string.Format("{0:F2}%", prevProgress * 100);
    }

    private void InitBossStg()
    {
        stageTitleTxt.text = $"{SetDiffTxt()} {worldNum}-BOSS";
        StageIcon.sprite = iconSprite[1];
        progressSldr.value = 1; //TODO: 보스 HealthSystem과 연결.
        progressTxt.text = "100%";
        bossTimeSldr.gameObject.SetActive(true);
        bossTimeTxt.gameObject.SetActive(true);

        monsterIds = Data.monsterIds;
        stageNum++;
        SetRcode();
        PoolManager.Instance.AddPool(Data.monsterIds);
        spawner.RandomSpawnPoint(Data.monsterId, Data.spawnCount);
        bossLeftTime = bossLimitTime;
        GameManager.Instance.NotifyStageClear();
    }

    private string SetDiffTxt()
    {
        switch (diffNum)
        {
            case 0:
                return "쉬움";
            case 1:
                return "보통";
            case 2:
                return "어려움";
            case 3:
                return "매우 어려움";
            default:
                return "지옥";
        }       
    }

    IEnumerator SetProgressBar()
    {
        float time = 0;
        float duration = 0.5f;
        while (true) 
        {
            if (curProgress > prevProgress)
            {
                time += Time.deltaTime;
                prevProgress = Mathf.Lerp(prevProgress, curProgress, time / duration);
                progressSldr.value = prevProgress;
                progressTxt.text = string.Format("{0:F2}%", prevProgress * 100);
            }
            else
            {
                time = 0;
            }
            
            yield return null;
        }
    }
    #endregion


    #region Next Stage/World/Diff
    public void ToNextStage(int index = 1)
    {
        InventoryManager.Instance.SetData("curStageCount", 0);

        if (index == -1)
            stageNum = 1;
        else if (stageNum <= maxStageNum)
            stageNum += index;
        
        SetRcode();
    }

    private void ToNextWorld()
    {
        stageNum = 1;
        if (worldNum <= maxWorldNum)
        {
            worldNum++;
            SetRcode();
            PoolManager.Instance.AddPool(Data.monsterIds);
        }
        else
            ToNextDiff();
    }

    private void ToNextDiff()
    {
        stageNum = 1;
        worldNum = 1;
        diffNum++;
        SetRcode();
        PoolManager.Instance.AddPool(Data.monsterIds);
    }
    #endregion

    #region Death Events
    public void MonsterDead(EnemyData enemyData, GameObject enemyGo)
    {
        if (enemyData.isBoss)
        {
            isBossCleared = true;
        }
        else
        {
            killCount++;
            curSpawnCount--;
            curProgress = Mathf.Min((float)killCount / Data.nextStageCount, 100f);
            spawner.isActivatedEnemy.Remove(enemyGo);
        }
        InventoryManager.Instance.DropItem(enemyData.rewardType, enemyData.rewardRate, enemyData.rewardValue);
        InventoryManager.Instance.SetDeltaData("curStageCount", 1);
    }

    public void OnPlayerDead()
    {
        if (isPlayerDeadHandled) return;

        isPlayerDeadHandled = true;
        
        if (stageCoroutine != null)
        {
            StopCoroutine(stageCoroutine);
        }

        ResetSpawnedEnemy();
        ToNextStage(-1);

        isPlayerDeadHandled = false;
    }
    #endregion
}