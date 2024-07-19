using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private string userPath;
    private string initPath;

    public UserData userData = new UserData();
    protected override void Awake()
    {
        base.Awake();
        initPath = Path.Combine(Application.dataPath, "initData.json");
        userPath = Path.Combine(Application.persistentDataPath, "userData.json");
    }

    public void SaveToJson<T>(T data)
    {
        if (!File.Exists(userPath))
        {
            File.Create(userPath);
        }
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(userPath, jsonData);
    }

    public void SaveToJson<T>(T data, string path)
    {
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, jsonData);
    }

    public void LoadData()
    {
        if (!File.Exists(userPath))
        {
            if (!File.Exists(initPath))
            {
                UserData userData = new UserData();
                SaveToJson<UserData>(userData, initPath);
                LoadData();
            }
            else
            {
                LoadFromJson(initPath);
                SaveToJson<UserData>(userData, userPath);
            }
        }
        else
        {
            LoadFromJson(userPath);
        }
    }

    public void LoadFromJson(string path)
    {
        // 데이터를 불러올 경로 지정
        string dataPath = Path.Combine(Application.persistentDataPath, path);
        string jsonData = File.ReadAllText(dataPath);
        // 파일의 텍스트를 string으로 저장
        userData = JsonUtility.FromJson<UserData>(jsonData);
    }
}
