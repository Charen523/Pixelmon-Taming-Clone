using UnityEngine;

public static class PixelmonStatHandler
{
    public static PixelmonStatus InitStatus(this PixelmonStatus status, PixelmonData data, MyPixelmonData myData)
    {
        status.Atk = SetStatus(data.baseAtk, myData.statRank[(int)PxmStatus.Atk], myData.lv, 0, 10);
        status.Cri = SetPercentStatus(data.baseAtk, myData.statRank[(int)PxmStatus.Cri], myData.lv, 0, 10);
        status.CriDmg = SetStatus(data.baseAtk, myData.statRank[(int)PxmStatus.CriDmg], myData.lv, 0, 10);
        status.AtkSpd = SetPercentStatus(data.baseAtk, myData.statRank[(int)PxmStatus.Atk], myData.lv, 0, 10);
        status.Dmg = SetStatus(data.baseAtk, myData.statRank[(int)PxmStatus.AtkDmg], myData.lv, 0, 10);
        status.SDmg = SetPercentStatus(data.baseAtk, myData.statRank[(int)PxmStatus.SDmg], myData.lv, 0, 10);
        status.SCri = SetStatus(data.baseAtk, myData.statRank[(int)PxmStatus.SCri], myData.lv, 0, 10);
        status.SCriDmg = SetPercentStatus(data.baseAtk, myData.statRank[(int)PxmStatus.SCriDmg], myData.lv, 0, 10);
        return status;
    }

    //일반수치 데이터 수정 필요
    public static float SetStatus(float baseStat, float statRank, int lv, float statUpLv, float statValue)
    {
        return baseStat * statRank / 100 + (1 + (lv - 1) * 0.1f) + (statUpLv * statValue);
    }

    //퍼센트 수치 데이터 수정 필요
    public static float SetPercentStatus(float baseStat, float statRank, int lv, float statUpLv, float statValue)
    {
        return (baseStat * statRank * (1 + ((lv - 1) * 0.1f) + (statUpLv * statValue)) / 100);
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

    public static void PxmLvUpgrade(this PixelmonStatus status)
    {

    }

    public static void PxmStarUpgrade(this PixelmonStatus status)
    {

    }

    public static void PxmPsvEffect(this PixelmonStatus status)
    {

    }
    
    public static int GetTotalDamage(this PixelmonStatus status, bool isSkill = false)
    {
        int totalDamage = (int)status.Dmg;

        if (isSkill)
        {
            SetDamage(status.Dmg);
            if (IsCritical(status.Cri + status.SCri))
            {
                
            }
            else
            {

            }
        }
        else
        {
            SetDamage(status.SDmg);
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
        int damage = 0;

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