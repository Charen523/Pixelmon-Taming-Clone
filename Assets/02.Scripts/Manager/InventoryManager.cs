using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : Singleton<InventoryManager>
{
    public UserData userData = new UserData();
    [SerializeField]
    private DataManager dataManager;
    bool isDirty;
    public UnityAction<int, PixelmonData> equipAction;
    public UnityAction<int> unEquipAction;
    // Start is called before the first frame update
    void Start()
    {
        dataManager = DataManager.Instance;
        SaveManager.Instance.LoadFromPrefs(userData);
        StartCoroutine(ChangedValue());
        //SetData(nameof(userData.money), 20);
    }

    IEnumerator ChangedValue()
    {
        while (true)
        {
            if (isDirty)
            {
                isDirty = false;
                SaveManager.Instance.SaveToPrefs(userData);
            }
            yield return null;
        }
    }

    public void SetData(string field, object value)
    {
        var fields = userData.GetType().GetField(field);
        fields.SetValue(userData, value);
        isDirty = true;
    }

    /// <param name="field">nameof(userData.변수)</param>
    /// <param name="value">데이터 변화량</param>
    public void SetDeltaData(string field, int value)
    {
        var fields = userData.GetType().GetField(field);
        int val = (int)fields.GetValue(userData) + value;
        fields.SetValue(userData, val);
        isDirty = true;
    }

    public void DropItem(string[] rcodes, float[] rates ,int[] amounts)
    {
        for(int i = 0; i < rcodes.Length; i++) 
        {
            if (CheckedDropRate(rates[i]))
            {
                SetDeltaData(dataManager.GetData<RewardData>(rcodes[i]).name, amounts[i]);
            }
        }
    }

    public bool CheckedDropRate(float rate)
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand <= rate) return true;
        return false;
    }

    public bool CheckedInventory(string field, int value)
    {
        var fields = userData.GetType().GetField(field);
        if((int)fields.GetValue(userData) >= value) return true;
        return false;
    }

    //정보 가져오는 용도
    public List<PixelmonData> GetPossessPixelmons()
    {
        return userData.prossessedPixelmons.ToList();
    }

    public void SetPossessPixelmons(List<PixelmonData> data) 
    {
        SetData(nameof(userData.prossessedPixelmons), data.ToArray());
    }
}

