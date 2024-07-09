using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

[Serializable]
public class Wrapper<T>
{
    public T[] Items;
}

public class DataManager : Singleton<DataManager>
{
    private readonly string jsonFolderName = "GameData";
    private readonly Dictionary<string, object> dataDictionaries = new Dictionary<string, object>();

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();
        LoadAllData();
    }

    private void LoadAllData()
    {
        // Get all JSON files in the GameData folder
        var jsonFiles = Resources.LoadAll<TextAsset>(jsonFolderName);
        foreach (var jsonFile in jsonFiles)
        {
            LoadJsonData(jsonFile);
        }
    }

    private void LoadJsonData(TextAsset jsonData)
    {
        //파일 이름 = 클래스 이름
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(jsonData.name);

        //reflection으로 Type찾기
        Type dataType = Type.GetType($"{fileNameWithoutExtension}");
        if (dataType != null)
        {
            MethodInfo method = typeof(DataManager).GetMethod("LoadDataToDictionary", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo generic = method.MakeGenericMethod(dataType);
            generic.Invoke(this, new object[] { jsonData.text, fileNameWithoutExtension });
        }
        else
        {
            Debug.LogWarning($"{fileNameWithoutExtension}이란 이름의 Data Class가 존재하지 않습니다.");
        }
    }

    private void LoadDataToDictionary<T>(string jsonData, string key) where T : IData
    {
        try
        {
            //json파일을 Item이라는 객체로 wrap해주기.(안하면 오류 뜸...)
            var wrapper = JsonUtility.FromJson<Wrapper<T>>("{\"Items\":" + jsonData + "}");
            if (wrapper == null || wrapper.Items == null)
            {
                Debug.LogError("Failed to deserialize JSON data.");
                return;
            }

            //새 Dictionary 만들어주기
            var dictionary = new Dictionary<string, T>();

            //Dictionary에 값 할당
            foreach (var item in wrapper.Items)
            {
                dictionary[item.Rcode] = item;
            }

            //만들어진 Dictionary를 또다른 Dictionary로 묶어주기
            dataDictionaries[key] = dictionary;
        }
        catch (Exception e)
        {
            Debug.LogError($"딕셔너리 생성 중 오류발생, 오류 종류: {e.Message}");
        }
    }

    public T GetData<T>(string rcode) where T : class, IData
    {
        string key = typeof(T).Name;
        if (dataDictionaries.TryGetValue(key, out object dictionary))
        {
            var dict = dictionary as Dictionary<string, T>;
            if (dict != null && dict.TryGetValue(rcode, out T data))
            {
                return data;
            }
            else
            {
                Debug.LogWarning($"{typeof(T).Name}에 rcode {rcode}는 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"{typeof(T).Name}에 대한 Dictionary가 존재하지 않습니다.");
        }
        return null;
    }
}