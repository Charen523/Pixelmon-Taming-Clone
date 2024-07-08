using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private readonly string jsonFolderName = "GameData";

    #region json file names
    private readonly string jsonStageData = "StageData";
    private readonly string jsonEnemyData = "EnemyData";
    private readonly string jsonPixelmonData = "PixelmonData";
    #endregion

    #region Dictionaries
    [SerializeField]
    Dictionary<string, StageData> StageDictionary = new Dictionary<string, StageData>();
    [SerializeField]
    Dictionary<string, EnemyData> EnemyDictionary = new Dictionary<string, EnemyData>();
    Dictionary<string, PixelmonData> PixelmonDictionary = new Dictionary<string, PixelmonData>();
    Dictionary<Type, object> dataDictionaries = new Dictionary<Type, object>();
    #endregion

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();
        InitializeDictionaries();
        //LoadData();
    }

    private void InitializeDictionaries()
    {
        dataDictionaries.Add(typeof(StageData), StageDictionary);
        dataDictionaries.Add(typeof(EnemyData), EnemyDictionary);
        dataDictionaries.Add(typeof(PixelmonData), PixelmonDictionary);
    }

    private void LoadData()
    {
        LoadJsonData(jsonStageData, StageDictionary);
        LoadJsonData(jsonEnemyData, EnemyDictionary);
        LoadJsonData(jsonPixelmonData, PixelmonDictionary);
    }

    private void LoadJsonData<T>(string jsonFileName, Dictionary<string, T> dictionary) where T : IData
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>($"{jsonFolderName}/{jsonFileName}");
        if (jsonTextAsset == null)
        {
            Debug.LogError($"{jsonFileName}가 경로 Resources/{jsonFolderName}에 존재하지 않음.");
            return;
        }

        T[] datas = JsonUtility.FromJson<T[]>(jsonTextAsset.text);
        foreach (var data in datas)
        {
            dictionary.Add(data.Rcode, data);
        }
    }

    public T GetData<T>(string rcode) where T : class, IData
    {
        if (dataDictionaries.TryGetValue(typeof(T), out var dictionary))
        {
            var typedDictionary = dictionary as Dictionary<string, T>;
            if (typedDictionary != null && typedDictionary.TryGetValue(rcode, out var data))
            {
                return data;
            }
            else
            {
                Debug.LogError("잘못된 Rcode입니다.");
            }
        }
        else
        {
            Debug.LogError("잘못된 데이터 타입입니다.");
        }

        return null;
    }
}
