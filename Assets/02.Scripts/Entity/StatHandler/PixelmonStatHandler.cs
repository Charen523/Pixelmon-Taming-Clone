using System.Collections.Generic;
using UnityEngine;

public static class PixelmonStatHandler
{
    public static void InitStatus(this PixelmonStatus status, PixelmonData data, MyPixelmonData myData)
    {   
        status.perAtk = SetStatus(data.basePerAtk, myData.lv, data.lvAtkRate);
        status.Atk = SetMultiStatus(PixelmonManager.Instance.upgradeStatus.Atk, myData.FindType(AbilityType.Attack));
        status.Cri = SetPlusStatus(PixelmonManager.Instance.upgradeStatus.Cri, myData.FindType(AbilityType.PSVCri));
        status.CriDmg = SetPlusStatus(PixelmonManager.Instance.upgradeStatus.CriDmg, myData.FindType(AbilityType.PSVCriDmg));
        status.Dmg = SetMultiStatus(PixelmonManager.Instance.upgradeStatus.Dmg, myData.FindType(AbilityType.PSVDmg));
        status.SDmg = SetMultiStatus(PixelmonManager.Instance.upgradeStatus.SDmg, myData.FindType(AbilityType.PSVSDmg));
        status.SCri = SetPlusStatus(PixelmonManager.Instance.upgradeStatus.SCri, myData.FindType(AbilityType.PSVSCri));
        status.SCriDmg = SetPlusStatus(PixelmonManager.Instance.upgradeStatus.SCri, myData.FindType(AbilityType.PSVSCriDmg));
    }

    public static void ApplyMyPixelmon(UpgradeIndex type, MyPixelmonData myData)
    {
        for(int i = 0; i < 5;  i++) 
        {
            if (PixelmonManager.Instance.player.pixelmons[i] != null) 
            {
                PixelmonManager.Instance.player.pixelmons[i].ApplyStatus(type, myData);
            }
        }
    }

    public static void ApplyStatus(this Pixelmon pixelmon, UpgradeIndex type, MyPixelmonData myData)
    {
        switch(type) 
        { 
            case UpgradeIndex.Atk:
                pixelmon.status.Atk = SetMultiStatus(PixelmonManager.Instance.upgradeStatus.Atk, myData.FindType(AbilityType.Attack));
                break;
            case UpgradeIndex.Cri:
                pixelmon.status.Cri = SetPlusStatus(PixelmonManager.Instance.upgradeStatus.Cri, myData.FindType(AbilityType.PSVCri));
                break;
            case UpgradeIndex.CriDmg:
                pixelmon.status.CriDmg = SetPlusStatus(PixelmonManager.Instance.upgradeStatus.CriDmg, myData.FindType(AbilityType.PSVCriDmg));
                break;
            case UpgradeIndex.Dmg:
                pixelmon.status.Dmg = SetMultiStatus(PixelmonManager.Instance.upgradeStatus.Dmg, myData.FindType(AbilityType.PSVDmg));
                break;
            case UpgradeIndex.SDmg:
                pixelmon.status.SDmg = SetMultiStatus(PixelmonManager.Instance.upgradeStatus.SDmg, myData.FindType(AbilityType.PSVSDmg));
                break;
            case UpgradeIndex.SCri:
                pixelmon.status.SCri = SetPlusStatus(PixelmonManager.Instance.upgradeStatus.SCri, myData.FindType(AbilityType.PSVSCri));
                break;
            case UpgradeIndex.SCriDmg:
                pixelmon.status.SCriDmg = SetPlusStatus(PixelmonManager.Instance.upgradeStatus.SCri, myData.FindType(AbilityType.PSVSCriDmg));
                break;
            default:
                break;
        }
    }

    public static float SetStatus(float perAtk, int lv, float lvAtkRate)
    {
        return (perAtk + lv * lvAtkRate);
    }

    public static float SetMultiStatus(float upgradeAtk, float psvAtk)
    {
        return upgradeAtk * psvAtk;
    }

    public static float SetPlusStatus(float upgradeAtk, float psvAtk)
    {
        return upgradeAtk + psvAtk;
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

    public static void PxmLvUp(this PixelmonStatus status, MyPixelmonData myData)
    {
        SaveManager.Instance.SetDeltaData("lv", ++myData.lv);
        if (myData.lv % 10 == 0)
        {
            SaveManager.Instance.SetDeltaData("maxExp", (int)(myData.maxExp * 1.5f));
        }
        else
        {
            SaveManager.Instance.SetDeltaData("maxExp", (int)(myData.maxExp * 1.1f));
        }
    }

    public static void PxmStarUp(this PixelmonStatus status, MyPixelmonData myData)
    {
        if ((myData.star & 1) == 0)
        {
            if (myData.star / 2 == 1)
            {
                //보유수치 상승
                for (int i = 0; i < myData.ownEffectValue.Length; i++)
                    myData.ownEffectValue[i] += 10;
            }
            else
            {
                for (int i = 0; i < myData.ownEffectValue.Length; i++)
                    myData.ownEffectValue[i] += 30;
            }
        }
        else
        {
            PsvSkill newSkill = new PsvSkill();
            myData.psvSkill.Add(newSkill);
        }

        if(myData.star == 5) 
        {
            //패시브 룰렛기능 오픈
        }
    }

    public static List<PsvSkill> PxmPsvEffect(this PixelmonStatus status, MyPixelmonData myData, bool[] isLocked)
    {
        List<PsvSkill> newSkills = new List<PsvSkill>();
        for (int i = 0; i < myData.psvSkill.Count; i++)
        {
            if (!isLocked[i])
            {
                PsvSkill newSkill = new PsvSkill();
                int randType = Random.Range(0, 7);
                newSkill.psvType = (AbilityType)randType;
                //TODO : 수치 값 랜덤 후 대입
                newSkills.Add(newSkill);
            }
            else
            {
                newSkills.Add(myData.psvSkill[i]);
            }
        }       
        return newSkills;
    }
    
    public static float GetTotalDamage(this PixelmonStatus status, MyPixelmonData myData ,bool isSkill = false)
    {
        float dealDmg;
        if (isSkill)
        {
            if (IsCritical(status.Cri + status.SCri))
                dealDmg =  status.perAtk * (status.Atk + status.SDmg) * (100 + status.SCriDmg + status.CriDmg)/100;
            else
                dealDmg = status.perAtk * (status.Atk + status.SDmg);
        }
        else
        {
            if (IsCritical(status.Cri))
                dealDmg = status.perAtk * (status.Atk + status.Dmg) * (100 + status.CriDmg)/100;
            else
                dealDmg = status.perAtk * (status.Atk + status.Dmg);
        }
        //버프가 있다면 dealDmg *= 1;

        return dealDmg;
    }


    public static bool IsCritical(float rate)
    {
        return Random.Range(0, 10000) <= rate * 100;
    }
}

public enum StatusType
{
    Add,
    Multiple,
    Override
}