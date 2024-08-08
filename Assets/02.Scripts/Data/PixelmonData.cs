using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PixelmonData : IData
{
    public string rcode;
    //도감 넘버
    public int id;
    public string name;
    public string rank;

    public float basePerAtk;
    public float lvAtkRate;
    public string trait;
    public float traitValue;
    public float basePerHp;
    public float lvHpRate;
    public float basePerDef;
    public float lvDefRate;

    //픽셀몬 아이콘
    public Sprite icon;
    public Sprite bgIcon;

    string IData.Rcode => rcode;

    public float FindTraitType(string type)
    {
        if (type == trait)
            return traitValue;
        else
            return 1;
    }
}

