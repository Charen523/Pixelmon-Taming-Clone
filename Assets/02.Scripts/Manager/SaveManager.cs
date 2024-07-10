using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
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
    public void SaveToPrefs<T>(T data)
    {
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(saveData, jsonData);
    }


    [ContextMenu("Prefs로드")]
    public void LoadFromPrefs<T>(T data)
    {
        if(PlayerPrefs.HasKey(saveData)) 
        { 
            string prefsValue = PlayerPrefs .GetString(saveData);
            Player.Instance.data = JsonUtility.FromJson<PlayerData>(prefsValue);
        }
        else
        {
            Player.Instance.data = new PlayerData();
            SaveToPrefs(data);
            LoadFromPrefs(data);
        }
    }

    [ContextMenu("Prefs제거")]
    public void RemoveAllPrefs()
    {
        if (PlayerPrefs.HasKey(saveData))
        {
            PlayerPrefs.DeleteKey(saveData);
            Debug.Log($"{saveData}제거");
        }
    }

}
