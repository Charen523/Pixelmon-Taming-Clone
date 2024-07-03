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
    private readonly string jsonItemData = "ItemData";
    #endregion

    #region Dictionaries
    Dictionary<string, StageData> StageDictionary = new Dictionary<string, StageData>();
    Dictionary<string, EnemyData> EnemyDictionary = new Dictionary<string, EnemyData>();
    Dictionary<string, PixelmonData> PixelmonDictionary = new Dictionary<string, PixelmonData>();
    Dictionary<string, ItemData> ItemDictionary = new Dictionary<string, ItemData>();
    #endregion

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();
        LoadData();
    }

    private void LoadData()
    {
        LoadJsonData<StageData>(jsonStageData, StageDictionary);
        LoadJsonData<EnemyData>(jsonEnemyData, EnemyDictionary);
        LoadJsonData<PixelmonData>(jsonPixelmonData, PixelmonDictionary);
        LoadJsonData<ItemData>(jsonItemData, ItemDictionary);
    }

    private void LoadJsonData<T>(string jsonFileName, Dictionary<string, T> dictionary) where T : IData
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>($"{jsonFolderName}/{jsonFileName}");
        if (jsonTextAsset == null)
        {
            Debug.LogError($"Failed to load {jsonFileName} from Resources/{jsonFolderName}");
            return;
        }

        T[] datas = JsonUtility.FromJson<T[]>(jsonTextAsset.text);
        foreach (var data in datas)
        {
            dictionary.Add(data.Rcode, data);
        }
    }

}