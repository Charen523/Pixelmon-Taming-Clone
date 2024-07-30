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

    public float perAtk;
    public float lvAtkRate;

    public float baseAtk;
    public float baseCri;
    public float baseCriDmg;
    public float baseAtkSpd;
    public float baseDmg;
    public float baseSDmg;
    public float baseSCri;
    public float baseSCriDmg;

    //픽셀몬 아이콘
    public Sprite icon;
    public Sprite bgIcon;

    string IData.Rcode => rcode;
}

