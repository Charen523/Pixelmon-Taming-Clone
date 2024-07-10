using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField]
    private UserData userData = new UserData();

    bool isDirty;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChagnedValue());
        SetData(nameof(userData.money), 10);
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


    private void DropItem(string rcode, int count)
    {
        SetData(DataManager.Instance.GetData<RewardData>(rcode).name, count);
    }
}

