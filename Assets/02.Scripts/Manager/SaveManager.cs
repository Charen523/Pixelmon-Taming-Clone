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
        SetFieldData(nameof(userData.gold), BigInteger.Parse(userData._gold));
        SetFieldData(nameof(userData.userExp), BigInteger.Parse(userData._exp));
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
        }
        else if (File.Exists(initPath))
        {
            LoadFromJson(initPath);
            SaveToJson(userData, userPath);
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

    private async void SaveDataAsync() //추후 저장데이터를 서버로 보낼 때 활성화
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
        }
        else
        {
            Debug.LogWarning($"{field} 변수를 UserData에서 찾을 수 없습니다.");
        }
    }

    public void SetDeltaData(string field, int value)
    {
        var fieldInfo = userData.GetType().GetField(field);
        if (fieldInfo != null)
        {
            int currentValue = (int)fieldInfo.GetValue(userData);
            fieldInfo.SetValue(userData, currentValue + value);
            isDirty = true;
        }
        else
        {
            Debug.LogWarning($"{field} 변수를 UserData에서 찾을 수 없습니다.");
        }
    }

    /// <summary>
    /// SetData, SetDeltaData, SetGold 모두 섞은 하이브리드 메서드
    /// </summary>
    /// <param name="field">필드 이름: userData.(변수명) 형식으로 사용할 것.</param>
    /// <param name="value">필드에 들어갈 값 또는 변화량.</param>
    /// <param name="isDelta">만약 변화량이라면 true인 default매개변수</param>
    public void SetFieldData(string field, object value, bool isDelta = false)
    {
        var fieldInfo = userData.GetType().GetField(field);
        if (fieldInfo != null)
        {
            var currentValue = fieldInfo.GetValue(userData);

            if (isDelta)
            {
                if (currentValue is int currentInt)
                {
                    fieldInfo.SetValue(userData, currentInt + (int)value);
                }
                else if (currentValue is BigInteger currentBigInt)
                {
                    fieldInfo.SetValue(userData, currentBigInt + (BigInteger)value);
                }
                else
                {
                    Debug.LogWarning($"Delta 연산이 지원되지 않는 타입입니다: {field}");
                    return;
                }
            }
            else
            {
                fieldInfo.SetValue(userData, value);
            }

            if (field == nameof(userData.gold))
            {
                userData._gold = userData.gold.ToString();
            }
            else if (field == nameof(userData.userExp))
            {
                userData._exp = userData.userExp.ToString();
            }

            isDirty = true;

            if (Enum.TryParse(field, true, out DirtyUI dirtyUI))
            {
                UIManager.Instance.InvokeUIChange(dirtyUI);
            }
        }
        else
        {
            Debug.LogWarning($"{field} 변수를 UserData에서 찾을 수 없습니다.");
        }
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