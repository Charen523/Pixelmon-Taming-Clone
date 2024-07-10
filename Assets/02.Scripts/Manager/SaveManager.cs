using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<DataManager>
{
    private readonly string jsonPlayerData = "PlayerData";
    private string jsonFolder;
    //[SerializeField]
    //Dictionary<string, PlayerData> PlayerDictionary = new Dictionary<string, PlayerData>();

    [SerializeField]
    private string playerPrefs;
    private string saveData = "SaveFile";
    void Start()
    {
        jsonFolder = Application.persistentDataPath;        
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


    [ContextMenu("Prefs저장")]
    public void SaveToPrefs()
    {
        string jsonData = JsonUtility.ToJson(Player.Instance.data);
        PlayerPrefs.SetString(saveData, jsonData);
    }


    [ContextMenu("Prefs로드")]
    public void LoadFromPrefs()
    {
        if(PlayerPrefs.HasKey(saveData)) 
        { 
            string prefsValue = PlayerPrefs .GetString(saveData);
            Player.Instance.data = JsonUtility.FromJson<PlayerData>(prefsValue);
        }
        else
        {
            Player.Instance.data = new PlayerData();
            SaveToPrefs();
            LoadFromPrefs();
        }
    }

    [ContextMenu("Prefs제거")]
    public void RemoveAllPrefs()
    {
        if (PlayerPrefs.HasKey(saveData))
        {
            PlayerPrefs.DeleteKey(saveData);
            Debug.Log("제거");
        }
    }

}
