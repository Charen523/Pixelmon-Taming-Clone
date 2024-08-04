using System;
using UnityEngine;


[Serializable]
public class ActiveData : IData
{
    public string rcode;
    public int id;
    public string rank;
    public string name;
    public string description;
    public AtvSkillType type;
    public bool isCT;
    public float coolTime;
    public float maxRate;

    public string prefabrcode;
    public int count = 1;
    public float range;
    public float scale;


    public Sprite icon;
    public Sprite bgIcon;


    public string Rcode => rcode;

}
