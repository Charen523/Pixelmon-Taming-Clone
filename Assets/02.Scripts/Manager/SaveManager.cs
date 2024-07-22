using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveManager : Singleton<SaveManager>
{
    public UserData userData = new UserData();
    [SerializeField] private DataManager dataManager;

    private string userPath;
    private string initPath;

    bool isDirty;

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
            if (isDirty)
            {
                isDirty = false;
                SaveToJson(userData);
            }
            yield return null;
        }
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
            Debug.LogWarning($"{field}라는 변수를 UserData에서 찾을 수 없습니다.");
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
        }
        else
        {
            Debug.LogWarning($"{field}라는 변수를 UserData에서 찾을 수 없습니다.");
        }
    }

    public void DropItem(string[] rcodes, float[] rates ,int[] amounts)
    {
        for(int i = 0; i < rcodes.Length; i++) 
        {
            if (CheckDropRate(rates[i]))
            {
                SetDeltaData(dataManager.GetData<RewardData>(rcodes[i]).name, amounts[i]);
            }
        }
    }

    public bool CheckDropRate(float rate)
    {
        return UnityEngine.Random.Range(0, 100) <= rate;
    }

    public bool CheckedInventory(string field, int value)
    {
        var fieldInfo = userData.GetType().GetField(field);
        if (fieldInfo != null)
        {
            return (int)fieldInfo.GetValue(userData) >= value;
        }
        else
        {
            Debug.LogWarning($"{field}라는 변수를 UserData에서 찾을 수 없습니다.");
            return false;
        }
    }

    public void SetPossessPixelmons(List<PixelmonData> data) 
    {
        SetData(nameof(userData.ownedPxms), data.ToArray());
    }
}