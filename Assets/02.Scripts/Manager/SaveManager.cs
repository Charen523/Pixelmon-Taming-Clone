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
    void Start()
    {
     playerData = Player.Instance   
    }

    public void SaveUserData()
    {

    }

    private void SaveToJsonData<T>(T data, string path)
    {
        string fileName = Path.Combine(jsonFolder, path);
        if(!File.Exists(fileName)) 
        {
            File.Create(fileName);
        }
        string dataJson = JsonUtility.ToJson(data);
        File.WriteAllText(fileName, dataJson);
    }    
}
