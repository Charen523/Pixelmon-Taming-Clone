using System;
using UnityEngine.Events;

[Serializable]
public class PixelmonData : IData
{
    public string rcode;
    //도감 넘버
    public int collectionNum;
    public string name;
    public int rank;
    //같은 카드 중복개수
    public int possessionCount;
    public float atk;
    public float sAtk;
    public float cri;
    public float criDmg;
    public float atkDmg;
    public float hit;
    //편성 여부
    public bool isComposed;
    //보유 여부
    public bool isPossess;

    public UnityAction activeSkill;
    public UnityAction passiveSkill;
    public UnityAction trait;
    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}
