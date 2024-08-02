using System.IO;
using System.Collections;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Numerics;
using System.Reflection;

public class SaveManager : Singleton<SaveManager>
{
    public UserData userData = new UserData();
    [SerializeField] private DataManager dataManager;

    private string userPath;
    private string initPath;

    private static bool isDirty;
    private WaitUntil CheckDirty = new WaitUntil(() => isDirty);

    protected override void Awake()
    {
        base.Awake();
        initPath = Path.Combine(Application.dataPath, "initData.json");
        userPath = Path.Combine(Application.persistentDataPath, "userData.json");
        LoadData();
    }

    void Start()
    {
        dataManager = DataManager.Instance;
        StartCoroutine(ChangedValue());
    }

    public void SaveToJson<T>(T data, string path = null)
    {
        path ??= userPath;

        try
        {
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError($"{path}에 데이터를 저장하는데 실패함: {e.Message}");
        }
    }

    public void LoadData()
    {
        if (File.Exists(userPath))
        {
            LoadFromJson(userPath);
            userData.gold = BigInteger.Parse(userData._gold);
        }
        else if (File.Exists(initPath))
        {
            LoadFromJson(initPath);
            SaveToJson(userData, userPath);
            userData.gold = BigInteger.Parse(userData._gold);
        }
        else
        {
            userData = new UserData();
            SaveToJson(userData, initPath);
            LoadData();
        }
    }

    public void LoadFromJson(string path)
    {
        try
        {
            string jsonData = File.ReadAllText(path);
            userData = JsonUtility.FromJson<UserData>(jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError($"{path}로부터 데이터를 로드하는 데에 실패했습니다: {e.Message}");
        }
    }

    private IEnumerator ChangedValue()
    {
        while (true)
        {
            yield return CheckDirty;
            isDirty = false;
            SaveToJson(userData);
        }
    }

    private async void SaveDataAsync()
    {
        await Task.Run(() => SaveToJson(userData));
    }

    public void SetData(string field, object value)
    {
        var fieldInfo = userData.GetType().GetField(field);
        if (fieldInfo != null)
        {
            fieldInfo.SetValue(userData, value);
            isDirty = true;
            Debug.Log(fieldInfo.Name);
            UIManager.Instance.InvokeUIChange(field);
        }
        else
        {
            Debug.LogWarning($"{field} 변수를 UserData에서 찾을 수 없습니다.");
        }
    }

    /// <param name="field">nameof(userData.변수)</param>
    /// <param name="value">데이터 변화량</param>
    public void SetDeltaData(string field, int value)
    {
        var fieldInfo = userData.GetType().GetField(field);
        if (fieldInfo != null)
        {
            int currentValue = (int)fieldInfo.GetValue(userData);
            fieldInfo.SetValue(userData, currentValue + value);
            isDirty = true;
            Debug.Log(fieldInfo.Name);
            UIManager.Instance.InvokeUIChange(field);
        }
        else
        {
            Debug.LogWarning($"{field} 변수를 UserData에서 찾을 수 없습니다.");
        }
    }

    public void SetGold(BigInteger value, bool isDelta = false)
    {
        if (isDelta)
        {
            userData.gold += value;
            userData._gold = userData.gold.ToString();
        }
        else
        {
            userData.gold = value;
            userData._gold = value.ToString();
        }
        isDirty = true;
        Debug.Log("SetGold");
        UIManager.Instance.InvokeUIChange(nameof(userData.gold));
    }

    public void GetRewards(string[] rcodes, int[] amounts, float[] rates = null)
    {
        for(int i = 0; i < rcodes.Length; i++) 
        {
            if (rates == null || CheckDropRate(rates[i]))
            {
                string itemName = dataManager.GetData<RewardData>(rcodes[i]).name;
                SetDeltaData(itemName, amounts[i]);
            }
        }
    }

    public bool CheckDropRate(float rate)
    {
        return UnityEngine.Random.Range(0, 100) <= rate;
    }

    /// <summary>
    /// 주의: OwnedPixelmon만 바꿀 수 있는 메서드.
    /// </summary>
    /// <param name="index">owned에서 픽셀몬 데이터를 고칠 수 있는 index.</param>
    /// <param name="field">바꿀 myPixelmonData의 필드명.</param>
    /// <param name="value">새로 대입할 value</param>
    public void UpdatePixelmonData(int index, string field, object value)
    {
        if (index >= 0 && index < userData.ownedPxms.Length)
        {
            userData.ownedPxms[index].UpdateField(field, value);
            isDirty = true;
        }
        else
        {
            Debug.LogWarning($"유효하지 않은 인덱스: {index}");
        }
    }

    public void UpdateSkillData(int index, string field, object value)
    {
        if (index >= 0 && index < userData.ownedSkills.Count)
        {
            userData.ownedSkills[index].UpdateField(field, value);
            isDirty = true;
        }
        else
        {
            Debug.LogWarning($"유효하지 않은 인덱스: {index}");
        }
    }
}