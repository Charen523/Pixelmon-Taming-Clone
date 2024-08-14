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

    public void SpawnRewards(string rcode, int amount)
    {
        string itemName = DataManager.Instance.GetData<RewardData>(rcode).name;
        GetReward(itemName, amount);
    }

    public void GetReward(string itemName, int _amount)
    {
        switch(itemName)
        {//TODO: 땜빵으로 떼운 스테이지별 증가 보상
            case nameof(userData.gold):
            case nameof(userData.userExp):
                BigInteger amount1 = _amount;
                amount1 *= (StageManager.Instance.worldNum + StageManager.Instance.diffNum * 10);
                SaveManager.Instance.SetFieldData(itemName, amount1, true);
                break;
            default:
                int amount3 = _amount;
                SaveManager.Instance.SetFieldData(itemName, amount3, true);
                break;
        }
    }

    private bool CheckDropRate(float rate)
    {
        return UnityEngine.Random.Range(0, 100) <= rate;
    }
}