using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MyPixelmonData
{
    //도감 넘버
    public int id;
    public int lv = 1;
    public int exp = 0;
    public int star;
    //같은 카드 중복개수
    public int evolvedCount;

    //장착 여부
    public bool isEquiped;
    //보유 여부
    public bool isPossessed;

    public UnityAction activeSkill;
    //패시브 능력
    public PsvSkill[] trait;
}

public class PsvSkill
{
    public string rcode;
    //패시브 능력 이름
    public string name;
    //픽셀몬 패시브 타입
    public PassiveType psvType;
    //패시브 능력치
    public float ability;
}
