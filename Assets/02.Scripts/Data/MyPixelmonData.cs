using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MyPixelmonData
{
    //도감 넘버
    public string rcode;
    public int id;
    public int lv = 1;
    public int exp = 0;
    public int[] statRank;
    public int star;
    //같은 카드 중복개수
    public int evolvedCount;

    //장착 여부
    public bool isEquiped;
    //보유 여부
    public bool isOwned;

    public UnityAction activeSkill;
    //패시브 능력
    public PsvSkill[] trait;

    public void UpdateField(string fieldName, object value)
    {
        var fieldInfo = GetType().GetField(fieldName);
        if (fieldInfo != null)
        {
            fieldInfo.SetValue(this, value);
        }
        else
        {
            Debug.LogWarning($"{fieldName}라는 변수를 MyPixelmonData에서 찾을 수 없습니다.");
        }
    }
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
