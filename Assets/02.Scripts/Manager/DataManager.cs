using System;
using System.Collections.Generic;

public class DataManager : GSpreadReader<DataManager>
{
    private readonly Dictionary<string, IData> dataDics = new Dictionary<string, IData>();

    public GameData<StageData> stageData;
    public GameData<PixelmonData> pixelmonData;
    public GameData<EnemyData> enemyData;
    public GameData<RewardData> rewardData;

    public T GetData<T>(string rcode) where T : class, IData
    {
        return (T)dataDics[rcode.Split(' ')[0]];
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