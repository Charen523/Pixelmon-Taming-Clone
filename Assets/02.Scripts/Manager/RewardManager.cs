using System.Numerics;

public class RewardManager : Singleton<RewardManager>
{
    private SaveManager saveManager;
    private UserData userData;
    private StageManager stageManager;

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();

        saveManager = SaveManager.Instance;
        stageManager = StageManager.Instance;
        userData = saveManager.userData;
    }

    public void GetRewards(string[] rcodes, int[] amounts, float[] rates = null)
    {
        for (int i = 0; i < rcodes.Length; i++)
        {
            if (rates == null || CheckDropRate(rates[i]))
            {
                string itemName = DataManager.Instance.GetData<RewardData>(rcodes[i]).name;

                //현재 스테이지에 비례하는 보상량 증가
                if (itemName == nameof(userData.gold) || itemName == nameof(userData.userExp))
                {
                    BigInteger amount = amounts[i];
                    saveManager.SetFieldData(itemName, amount, true);
                }
                else
                {
                    int amount = amounts[i];
                    saveManager.SetFieldData(itemName, amount, true);
                }
            }
        }
    }

    private bool CheckDropRate(float rate)
    {
        return UnityEngine.Random.Range(0, 100) <= rate;
    }

}