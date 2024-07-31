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
    public float ct;

    public Sprite icon;
    public Sprite bgIcon;


    public string Rcode => rcode;

}
