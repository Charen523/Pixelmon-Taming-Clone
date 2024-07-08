using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<DataManager>
{
    private readonly string jsonPlayerData = "PlayerData";

    [SerializeField]
    Dictionary<string, PlayerData> PlayerDictionary = new Dictionary<string, PlayerData>();
    void Start()
    {
        
    }
}
