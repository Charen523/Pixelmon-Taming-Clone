using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<DataManager>
{
    private readonly string jsonPlayerData = "PlayerData";
    private readonly string jsonFolder = Application.persistentDataPath;
    [SerializeField]
    Dictionary<string, PlayerData> PlayerDictionary = new Dictionary<string, PlayerData>();

    [SerializeField]
    private string playerPrefs;
    private PlayerData playerData;
    private string saveData = "SaveFile";
    void Start()
    {
        playerData = Player.Instance.data;
    }

    public void SaveUserData()
    {

    }

    public void SaveToJsonData<T>(T data, string path)
    {
        string fileName = Path.Combine(jsonFolder, path);
        if(!File.Exists(fileName)) 
        {
            File.Create(fileName);
        }
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(fileName, jsonData);
    }

    public void SaveToPrefs()
    {
        string jsonData = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(saveData, jsonData);
    }

    private void LoadFromPrefs()
    {
        if(PlayerPrefs.HasKey(saveData)) 
        { 
            string prefsValue = PlayerPrefs .GetString(saveData);
            playerData = JsonUtility.FromJson<PlayerData>(prefsValue);
        }
        else
        {
            SaveToPrefs();
            LoadFromPrefs();
        }
    }
}
