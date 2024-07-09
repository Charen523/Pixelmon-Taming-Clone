using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private readonly string jsonFolderName = "GameData";
    private readonly Dictionary<string, object> dataDictionaries = new Dictionary<string, object>();

    #region json file names
    private readonly string jsonStageData = "StageData";
    private readonly string jsonEnemyData = "EnemyData";
    private readonly string jsonPixelmonData = "PixelmonData";
    private readonly string jsonRewardData = "RewardData";
    #endregion

    #region Dictionaries
    [SerializeField]
    Dictionary<string, StageData> StageDataDictionary = new Dictionary<string, StageData>();
    //[SerializeField]
    //Dictionary<string, EnemyData> EnemyDictionary = new Dictionary<string, EnemyData>();
    //[SerializeField]
    //Dictionary<string, PixelmonData> PixelmonDictionary = new Dictionary<string, PixelmonData>();
    [SerializeField]
    private Dictionary<string, RewardData> RewardDataDictionary = new Dictionary<string, RewardData>();
    #endregion

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();
        LoadData();
    }

    private void LoadData()
    {
        LoadStageData();
        //LoadJsonData(jsonEnemyData, EnemyDictionary);
        //LoadJsonData(jsonPixelmonData, PixelmonDictionary);
    }

    private void LoadStageData()
    {
        TextAsset jsonData = Resources.Load<TextAsset>($"{jsonFolderName}/{jsonStageData}");
        if (jsonData != null)
        {
            // JSON 파일을 읽어 StageDataArrayWrapper 객체로 변환합니다.
            StageDataArrayWrapper stageDataWrapper = JsonUtility.FromJson<StageDataArrayWrapper>(jsonData.text);

            foreach (var stageData in stageDataWrapper.Items)
            {
                stageData.ParseData();
                StageDataDictionary[stageData.rcode] = stageData;
            }
        }
        else
        {
            Debug.LogError("Cannot find StageData file!");
        }
    }

    private void LoadRewardData()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("GameData/RewardData");
        if (jsonData != null)
        {
            RewardDataArrayWrapper rewardDataArray = JsonUtility.FromJson<RewardDataArrayWrapper>(jsonData.text);

            foreach (var rewardData in rewardDataArray.Items)
            {
                RewardDataDictionary[rewardData.rcode] = rewardData;
            }
        }
        else
        {
            Debug.LogError("Cannot find RewardData file!");
        }
    }

    public StageData GetData(string rcode)
    {
        if (StageDataDictionary.TryGetValue(rcode, out StageData stageData))
        {
            return stageData;
        }
        else
        {
            Debug.LogWarning($"StageData with rcode {rcode} not found.");
            return null;
        }
    }

    public RewardData GetRewardData(string rcode)
    {
        if (RewardDataDictionary.TryGetValue(rcode, out RewardData rewardData))
        {
            return rewardData;
        }
        else
        {
            Debug.LogWarning($"RewardData with rcode {rcode} not found.");
            return null;
        }
    }
}

[Serializable]
public class StageDataArrayWrapper
{
    public StageData[] Items;
}

[Serializable]
public class RewardDataArrayWrapper
{
    public RewardData[] Items;
}