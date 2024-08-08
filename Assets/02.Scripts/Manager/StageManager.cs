using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : Singleton<StageManager>
{
    private SaveManager saveManager;
    private UserData userData;

    public FadeOut fadeOut;

    #region Stage Info
    [Header("Stage Info")]
    private StageData data; //set CurrentRcode에서 대신함.
    private string _currentRcode;
    public string CurrentRcode
    {
        get
        {
            if (_currentRcode == null || _currentRcode == "")
            {
                _currentRcode = userData.stageRcode;
            }
            return _currentRcode;
        }

        private set
        {
            _currentRcode = value;
            saveManager.SetFieldData(nameof(userData.stageRcode), _currentRcode);
            data = DataManager.Instance.GetData<StageData>(_currentRcode);
        }
    }

    [SerializeField] private readonly int nextThemeNum = 1;
    [SerializeField] private readonly int totalThemeNum = 5;
    [SerializeField] private readonly int maxWorldNum = 10;
    private readonly string fixedRcode = "STG_";
    public int diffNum;
    public int worldNum;
    public int stageNum;
    private int themeNum;

    private string[] monsterIds => data.monsterId;
    public int curSpawnCount = 0;
    private int killCount = 0;

    private float bossLeftTime;
    private readonly float bossLimitTime = 30;

    private Coroutine stageCoroutine;
    private WaitUntil proceedNormalStg;
    private WaitUntil proceedBossStg;
    private WaitUntil proceedDgStg;
    #endregion

    #region Spawn Interval
    private float curInterval = 0; //현재 시간 간격
    private readonly float spawnInterval = 2f; //스폰 간격
    #endregion


    #region Flags
    private bool isBossStage;
    private bool isBossCleared;
    public bool isDungeon;
    private bool isPlayerDead;
    private bool isEnemyReset;
    #endregion

    #region Dungeon Info
    public int dgIndex = 0;
    private DgMonster boss;
    #endregion

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

    public Spawner spawner;
    #endregion

    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();

        saveManager = SaveManager.Instance;
        userData = saveManager.userData;   
        InitData();
    }

    private void InitData()
    {
        data = DataManager.Instance.GetData<StageData>(userData.stageRcode);

        themeNum = CurrentRcode[CurrentRcode.Length - 1];
        diffNum = int.Parse(userData.curStage.Substring(0, 2));
        worldNum = int.Parse(userData.curStage.Substring(2, 2));
        stageNum = int.Parse(userData.curStage.Substring(4, 2));
        
        killCount = userData.curHuntCount;
    }

    private void Start()
    {
        GameManager.Instance.OnPlayerDie += OnPlayerDead;
        GameManager.Instance.OnStageStart += RestartStage;

        proceedNormalStg = new WaitUntil(() => NormalStage());
        proceedBossStg = new WaitUntil(() => BossStage());
        proceedDgStg = new WaitUntil(() => DungeonStage());

        InitStage();
        StartCoroutine(SetProgressBar());
    }
    #region Stage
    public void InitStage()
    {
        InitStageUI();
        stageCoroutine = StartCoroutine(StartStage());

        if (isBossStage || isDungeon)
        {
            StartCoroutine(SetProgressBar());
        }
    }

    private IEnumerator StartStage()
    {
        isEnemyReset = false;
        if (isBossStage)
        {
            yield return null; //없음 서순차이로 보스 소환 안됨.
            InitBossStage();
            yield return proceedBossStg;
            bossTimeSldr.gameObject.SetActive(false);

            if (isBossCleared)
            {
                isBossCleared = false;
                NextStageData();
            }
            else if (!isDungeon)
            {
                NextStageData(false);
                GameManager.Instance.NotifyStageTimeOut();
                yield break;
            }
        }
        else
        {
            yield return proceedNormalStg;

            if (!isDungeon)
            {
                NextStageData();
                GameManager.Instance.NotifyStageClear();
            }
        }

        Player.Instance.fsm.target = null; //target 초기화
        if (isDungeon)
        {
            InitDgStage();
            yield return proceedDgStg;
            bossTimeSldr.gameObject.SetActive(false);
        }

        fadeOut.gameObject.SetActive(true);
        fadeOut.StartFade();
        InitStage();
    }

    private bool NormalStage() 
    {
        if (isDungeon)
        {
            ResetSpawnedEnemy();
            return true;
        }

        if (killCount >= data.nextStageCount)
        {
            ResetSpawnedEnemy();
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
        spawner.SpawnMonsterTroop(data.monsterId, data.spawnCount);
        bossLeftTime = bossLimitTime;
    }

    private bool BossStage()
    {
        if (isDungeon)
        {
            ResetSpawnedEnemy();
            return true;
        }

        if (isBossCleared)
        {
            ResetSpawnedEnemy();
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

    private void InitDgStage()
    {
        bossLeftTime = bossLimitTime;
        boss = spawner.GetDgMonster(dgIndex);
        boss.InitDgMonster(dgIndex);
        InitStageUI();
    }

    private bool DungeonStage()
    {
        if (!isDungeon)
        {
            Destroy(boss.gameObject);
            return true;
        }

        stageTitleTxt.text = $"Dungeon Lv.{boss.dgLv}";
        bossLeftTime = Mathf.Max(0, bossLeftTime - Time.deltaTime);
        float percent = bossLeftTime / bossLimitTime;
        bossTimeSldr.value = percent;
        bossTimeTxt.text = string.Format("{0:F2}", bossLeftTime);

        if (bossLeftTime == 0 || isPlayerDead)
        {
            Destroy(boss.gameObject);
            isDungeon = false;
            return true;
        }

        return false;
    }
    
    private void NextStageData(bool isClear = true)
    {
        string newRcode = fixedRcode;
        string newStageProgress;
        isBossStage = false;

        if (!isClear)
        {
            newRcode += "N";
        }
        else if (isBossStage)
        {//Next Stage
            if (stageNum == nextThemeNum * totalThemeNum)
            {//Next World
                if (worldNum == maxWorldNum)
                {//Next Diff
                    diffNum++;
                    worldNum = 1;
                }
                worldNum++;
                themeNum = 1;
                stageNum = 1;
            }
            else if (stageNum % 5 == 0)
            {//Next Theme
                themeNum++;
                stageNum++;
            }
            else stageNum++;
            newRcode += "N";
        }
        else
        {//Normal -> Boss
            newRcode += "B";
            isBossStage = true;
        }

        newRcode += themeNum;
        newStageProgress = diffNum.ToString("D2") + worldNum.ToString("D2") + (stageNum).ToString("D2");
        saveManager.SetFieldData(nameof(userData.stageRcode), newRcode);
        saveManager.SetFieldData(nameof(userData.curStage), newStageProgress);
    }

    private void ResetSpawnedEnemy()
    {
        int j = 0;
        for (int i = 0; i < spawner.isActivatedEnemy.Count; i++)
        {
            spawner.isActivatedEnemy[i].healthSystem.TakeDamage(9999999f);
            j++;
        }
        saveManager.SetFieldData(nameof(userData.curHuntCount), -j, true);

        spawner.isActivatedEnemy.Clear();
        curSpawnCount = 0;
        killCount = 0;
        isEnemyReset = true;
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
            curProgress = Mathf.Min((float)killCount / data.nextStageCount, 1f);
            spawner.isActivatedEnemy.Remove(enemy);
        }
        RewardManager.Instance.GetRewards(enemyData.rewardType, enemyData.rewardValue, enemyData.rewardRate);
        
        if (QuestManager.Instance.isMonsterQ)
        {
            QuestManager.Instance.OnQuestEvent();
        }

        saveManager.SetFieldData(nameof(userData.curHuntCount), 1, true);
    }

    public void OnPlayerDead()
    {
        if (isPlayerDead) return;
        isPlayerDead = true;
        
        if (stageCoroutine != null)
        {
            StopCoroutine(stageCoroutine); //Stop Stage
        }
        ResetSpawnedEnemy();
        Player.Instance.fsm.target = null;

        NextStageData(false);
        isBossStage = false;
        isDungeon = false;
    }

    private void RestartStage()
    {//Notice Player Respawn
        isPlayerDead = false;
        InitStage();
    }
    #endregion

    #region UI
    private void InitStageUI()
    {
        if (isDungeon)
        {
            stageTitleTxt.text = $"Dungeon Lv.{boss.dgLv}";
            StageIcon.gameObject.SetActive(false);
            bossTimeSldr.gameObject.SetActive(true);
            return;
        }

        if (isBossStage)
        {
            stageTitleTxt.text = $"{SetDiffTxt(diffNum)} {worldNum}-BOSS";
            StageIcon.gameObject.SetActive(true);
            StageIcon.sprite = iconSprite[1];
            progressSldr.value = 1;
            progressTxt.text = "100%";
            bossTimeSldr.gameObject.SetActive(true);
            return;
        }

        bossTimeSldr.gameObject.SetActive(false);
        stageTitleTxt.text = $"{SetDiffTxt(diffNum)} {worldNum}-{stageNum}";
        StageIcon.gameObject.SetActive(true);
        StageIcon.sprite = iconSprite[0];
        curProgress = Mathf.Min((float)killCount / data.nextStageCount, 1f);
        prevProgress = curProgress;
        progressSldr.value = prevProgress;
        progressTxt.text = string.Format($"{(int)(prevProgress * 100)}%");
    }

    private IEnumerator SetProgressBar()
    {
        while (true)
        {
            if (curProgress > prevProgress)
            {
                StartCoroutine(UIUtils.AnimateSliderChange(progressSldr, prevProgress, curProgress));
                progressTxt.text = string.Format($"{(int)(prevProgress * 100)}%");
                prevProgress = curProgress;
            }
            yield return null;
        }
    }

    public string SetDiffTxt(int num)
    {
        return num switch
        {
            0 => "쉬움",
            1 => "보통",
            2 => "어려움",
            3 => "매우어려움",
            4 => "악몽I",
            5 => "악몽II",
            6 => "악몽III",
            7 => "심연I",
            8 => "심연II",
            9 => "심연III",
            10 => "지옥I",
            11 => "지옥II",
            12 => "지옥III",
            13 => "지옥IV",
            14 => "지옥V",
            15 => "불지옥I",
            16 => "불지옥II",
            17 => "불지옥III",
            18 => "불지옥IV",
            19 => "불지옥V",
            _ => "공허"
        };
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
}