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
    public int rank;
    //같은 카드 중복개수
    public int count;
    //장착 여부
    public bool isEquiped;
    //보유 여부
    public bool isPossessed;

    public float atk;
    public float atkSpd;
    public float sAtk;
    public float cri;
    public float criDmg;
    public float atkDmg;
    public float hit;

    //픽셀몬 아이콘
    public Sprite icon;


    public UnityAction activeSkill;
    //패시브 능력
    public Trait[] trait;

    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}

public class Trait
{
    public string rcode;
    //패시브 능력 이름
    public string name;
    //픽셀몬 패시브 타입
    public PassiveType psvType;
    //패시브 능력치
    public float ability;
}
