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

    public float baseAtk = 1;
    public float baseCri = 0;
    public float baseCriDmg = 0;
    public float baseAtkSpd = 1;
    public float baseDmg = 0;
    public float baseSDmg = 0;
    public float baseSCri = 0;
    public float baseSCriDmg = 0;

    //픽셀몬 아이콘
    public Sprite icon;
    public Sprite bgIcon;

    string IData.Rcode => rcode;
}

