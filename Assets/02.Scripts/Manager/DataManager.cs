using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : GSpreadReader<DataManager>
{
    private readonly Dictionary<string, IData> dataDics = new Dictionary<string, IData>();

    public GameData<StageData> stageData;
    public GameData<PixelmonData> pixelmonData;
    public GameData<EnemyData> enemyData;
    public GameData<RewardData> rewardData;
    public GameData<EggRateData> eggRateData;
    public GameData<EvolveData> evolveData;
    public GameData<AbilityRateData> abilityRateData;
    public GameData<BasePsvData> basePsvData;
    public GameData<ActiveData> activeData;
    public Sprite[] bgIcons;
    
    public bool isPxmInit;
    public async Task SetBaseData()
    {
        foreach (var data in pixelmonData.data)
        {
            data.icon = await ResourceManager.Instance.LoadAsset<Sprite>(data.rcode, eAddressableType.thumbnail);
            switch (data.rank)
            {
                case "Common":
                    data.bgIcon = bgIcons[0];
                    break;
                case "Advanced":
                    data.bgIcon = bgIcons[1];
                    break;
                case "Rare":
                    data.bgIcon = bgIcons[2];
                    break;
                case "Epic":
                    data.bgIcon = bgIcons[3];
                    break;
                case "Legendary":
                    data.bgIcon = bgIcons[4];
                    break;
                case "Unique":
                    data.bgIcon = bgIcons[5];
                    break;
                default:
                    break;
            }
        }

        foreach (var data in activeData.data)
        {
            data.icon = await ResourceManager.Instance.LoadAsset<Sprite>(data.rcode, eAddressableType.thumbnail);
        }

        isPxmInit = true;
    }

    public T GetData<T>(string rcode) where T : class, IData
    {
        return (T)dataDics[rcode];
    }

    public override void AddDataDics<T>(List<T> datas)
    {
        foreach (T data in datas)
        {
            if (!dataDics.ContainsKey(data.Rcode)) if (!dataDics.ContainsKey(data.Rcode))
                    dataDics.Add(data.Rcode, data);
        }
    }
}