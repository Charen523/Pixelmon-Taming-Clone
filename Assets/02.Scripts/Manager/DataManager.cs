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
    public Sprite[] pxmBgIcons;
    public Sprite[] skillBgIcons;

    public bool isPxmInit;
    public async Task SetBaseData()
    {
        foreach (var data in pixelmonData.data)
        {
            data.icon = await ResourceManager.Instance.LoadAsset<Sprite>(data.rcode, eAddressableType.thumbnail);
            switch (data.rank)
            {
                case "Common":
                    data.bgIcon = pxmBgIcons[0];
                    break;
                case "Advanced":
                    data.bgIcon = pxmBgIcons[1];
                    break;
                case "Rare":
                    data.bgIcon = pxmBgIcons[2];
                    break;
                case "Epic":
                    data.bgIcon = pxmBgIcons[3];
                    break;
                case "Legendary":
                    data.bgIcon = pxmBgIcons[4];
                    break;
                case "Unique":
                    data.bgIcon = pxmBgIcons[5];
                    break;
                default:
                    break;
            }
        }

        foreach (var data in activeData.data)
        {
            data.icon = await ResourceManager.Instance.LoadAsset<Sprite>(data.rcode, eAddressableType.thumbnail);
            switch (data.rank)
            {
                case "C":
                    data.bgIcon = skillBgIcons[0];
                    break;
                case "B":
                    data.bgIcon = skillBgIcons[1];
                    break;
                case "A":
                    data.bgIcon = skillBgIcons[2];
                    break;
                case "S":
                    data.bgIcon = skillBgIcons[3];
                    break;
                case "SS":
                    data.bgIcon = skillBgIcons[4];
                    break;
                default:
                    break;
            }
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