using UnityEngine;

public static class PixelmonStatHandler
{
    public static PixelmonStatus InitStatus(this PixelmonStatus status, PixelmonData data, MyPixelmonData myData)
    {
        
        status.Atk = SetStatus(data.perAtk, myData.lv, data.lvAtkRate);
        status.Cri = SetPercentStatus(data.baseCri, myData.lv, 0, 10);
        status.CriDmg = SetStatus(data.baseCriDmg, myData.lv, 0, 10);
        status.AtkSpd = SetPercentStatus(data.baseAtkSpd, myData.lv, 0, 10);
        status.Dmg = SetStatus(data.baseDmg, myData.lv, 0, 10);
        status.SDmg = SetPercentStatus(data.baseSDmg, myData.lv, 0, 10);
        status.SCri = SetStatus(data.baseSCri, myData.lv, 0, 10);
        status.SCriDmg = SetPercentStatus(data.baseSCriDmg, myData.lv, 0, 10);
        return status;
    }

    public static float SetStatus(float perAtk, int lv, float lvAtkRate)
    {
        return perAtk + (lv - 1) * lvAtkRate;
    }


    //일반수치 데이터 수정 필요
    public static float SetStatus(float baseStat, int lv, float statUpLv, float statValue)
    {
        //baseStat* statRank / 100 + (1 + (lv - 1) * 0.1f) + (statUpLv * statValue);
        return baseStat;
    }

    //퍼센트 수치 데이터 수정 필요
    public static float SetPercentStatus(float baseStat, int lv, float statUpLv, float statValue)
    {
        //(baseStat * statRank * (1 + ((lv - 1) * 0.1f) + (statUpLv * statValue)) / 100);
        return baseStat;
    }

    public static void StatusUp(this PixelmonStatus status, string field, float stat, StatusType type = StatusType.Add)
    {
        var fieldInfo = status.GetType().GetField(field);
        if (fieldInfo != null)
        {
            var value = (float)fieldInfo.GetValue(status);
            switch(type)
            {
                case StatusType.Add:
                    fieldInfo.SetValue(status, value + stat);
                    break;
                case StatusType.Multiple:
                    fieldInfo.SetValue(status, value * stat);
                    break;
                case StatusType.Override:
                    fieldInfo.SetValue(status, value);
                    break;
            };
        }
    }

    public static void PxmLvUpgrade(this PixelmonStatus status, MyPixelmonData myData)
    {
        var saveManager = SaveManager.Instance;
        saveManager.SetDeltaData("exp", 0);
        saveManager.SetDeltaData("lv", ++myData.lv);
        if (myData.lv % 10 == 0)
        {
            saveManager.SetDeltaData("maxExp", (int)(myData.maxExp * 1.5f));
        }
        else
        {
            saveManager.SetDeltaData("maxExp", (int)(myData.maxExp * 1.1f));
        }
    }

    public static void PxmStarUpgrade(this PixelmonStatus status, MyPixelmonData myData)
    {
        if ((myData.star & 1) == 0)
        {
            //보유수치 상승
        }
        else
        {
            //
        }

        if(myData.star == 5) 
        { 
            //패시브 룰렛기능 오픈
        }
    }

    public static void PxmPsvEffect(this PixelmonStatus status)
    {

    }
    
    public static int GetTotalDamage(this PixelmonStatus status, bool isSkill = false)
    {
        int totalDamage = (int)status.Atk;

        if (isSkill)
        {
            SetDamage(status.SDmg);
            if (IsCritical(status.Cri + status.SCri))
            {
                
            }
            else
            {

            }
        }
        else
        {
            SetDamage(status.Dmg);
            if (IsCritical(status.Cri))
            {

            }
            else
            {

            }
        }
        return totalDamage;
    }

    public static int SetDamage(float ability)
    {
        int damage = (int)ability;

        return damage;
    }

    public static bool IsCritical(float rate)
    {
        return Random.Range(0, 10000) <= rate * 100;
    }
}

public enum PxmStatus
{
    Atk,
    Cri,
    CriDmg,
    AtkSpd,
    AtkDmg,
    SDmg,
    SCri,
    SCriDmg
}

public enum StatusType
{
    Add,
    Multiple,
    Override
}