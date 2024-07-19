using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private readonly string jsonPlayerData = "userData.json";
    private string jsonFolder;
    //[SerializeField]
    //Dictionary<string, PlayerData> PlayerDictionary = new Dictionary<string, PlayerData>();

    [SerializeField]
    private string playerPrefs;
    private string saveData = "SaveFile";
    void Start()
    {
        
    }

    public void SaveUserData()
    {

    }

    public void SaveToJsonData<T>(T data)
    {
        string jsonData = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, jsonPlayerData);
        if(!File.Exists(path)) 
        {
            File.Create(path);
        }       
        File.WriteAllText(path, jsonData);
        Debug.Log("저장");
    }

    void LoadPlayerDataFromJson()
    {
        // 데이터를 불러올 경로 지정
        string path = Path.Combine(Application.dataPath, jsonPlayerData);
        // 파일의 텍스트를 string으로 저장
        string jsonData = File.ReadAllText(path);
        InventoryManager.Instance.userData = JsonUtility.FromJson<UserData>(jsonData);
    }


    [ContextMenu("Prefs저장")]
    public void SaveToPrefs<T>(T data)
    {
        string jsonData = JsonUtility.ToJson(data);
        SaveToJsonData<T>(data);
        PlayerPrefs.SetString(saveData, jsonData);
    }


    [ContextMenu("Prefs로드")]
    public void LoadFromPrefs(UserData data)
    {
        if(PlayerPrefs.HasKey(saveData)) 
        { 
            string prefsValue = PlayerPrefs .GetString(saveData);
            InventoryManager.Instance.userData = JsonUtility.FromJson<UserData>(prefsValue);
        }
        else
        {
            LoadPlayerDataFromJson();
            SaveToPrefs(InventoryManager.Instance.userData);
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
