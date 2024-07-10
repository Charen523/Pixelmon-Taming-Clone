using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public UserData userData = new UserData();
    [SerializeField]
    private DataManager dataManager;
    bool isDirty;
    // Start is called before the first frame update
    void Start()
    {
        dataManager = DataManager.Instance;
        SaveManager.Instance.LoadFromPrefs(userData);
        StartCoroutine(ChagnedValue());
        //SetData(nameof(userData.money), 20);
    }

    IEnumerator ChagnedValue()
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

    public void SetData(string field, int value)
    {
        var fields = userData.GetType().GetField(field);
        int val = (int)fields.GetValue(userData) + value;
        fields.SetValue(userData, val);
        isDirty = true;
    }

    private void DropItem(string rcode, int amount)
    {
        SetData(dataManager.GetData<RewardData>(rcode).name, amount);
    }

    public void UseGold(int amount)
    {
        SetData(nameof(userData.money), userData.money - amount);
    }
}

