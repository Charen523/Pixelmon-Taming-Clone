using System.Numerics;
using UnityEngine;

public class RewardManager : Singleton<RewardManager>
{
    private UserData userData;
    private PoolManager poolManager;

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();

        userData = SaveManager.Instance.userData;
        poolManager = PoolManager.Instance;
    }

    public void SpawnRewards(GameObject go, string[] rcodes, int[] amounts, float[] rates = null)
    {
        for (int i = 0; i < rcodes.Length; i++)
        {
            if (rates == null || CheckDropRate(rates[i]))
            {
                if (poolManager.PoolDictionary.ContainsKey(rcodes[i]))
                {
                    poolManager.SpawnFromPool<DropItem>(rcodes[i]).ExeCuteSequence(go, amounts[i]);
                }
                else
                {
                    string itemName = DataManager.Instance.GetData<RewardData>(rcodes[i]).name;
                    GetReward(itemName, amounts[i]);
                }
            }
        }
    }

    public void GetReward(string itemName, int _amount)
    {
        //TODO: 현재 스테이지에 비례하는 보상량 증가
        if (itemName == nameof(userData.gold) || itemName == nameof(userData.userExp))
        {
            BigInteger amount = _amount;
            SaveManager.Instance.SetFieldData(itemName, amount, true);
        }
        else
        {
            int amount = _amount;
            SaveManager.Instance.SetFieldData(itemName, amount, true);
        }
    }
    private bool CheckDropRate(float rate)
    {
        return UnityEngine.Random.Range(0, 100) <= rate;
    }
}