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
    public int maxExp = 10;
    public int[] statRank;
    public int star = 0;
    //진화 가능 여부
    public bool isAdvancable;
    //같은 카드 중복개수
    public int evolvedCount = 0;
    
    //장착 여부
    public bool isEquipped;
    //보유 여부
    public bool isOwned;

    public UnityAction activeSkill;

    //공격수치
    public float atkValue;
    //특성타입
    public string traitType;
    public float traitValue;
    //패시브 능력
    public PsvSkill[] psvSkill = new PsvSkill[1];
    //보유효과
    public float[] ownEffectValue = new float[2];

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

[Serializable]
public class PsvSkill
{
    public string psvRank;
    public string psvName;
    public float psvValue;
}
