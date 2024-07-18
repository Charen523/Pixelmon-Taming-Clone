using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class UserData
{ 
    public string userId;
    public string userName;
    public int userLv;
    public int userExp;
    public int money;
    public int diamond;
    public int eggLv;
    public int eggCount;
    public int seed;
    public int petFood;
    public int compost;
    public int dungeonKeyA;
    public int dungeonKeyB;
    public int dungeonKeyC;
    public PlayerData data;
    public int equipCount;
    public PixelmonData[] equipedPixelmons = new PixelmonData[5]; //오타: equipped이 맞는 문법!
    public PixelmonData[] prossessedPixelmons;
    public FieldData[] fieldDatas = new FieldData[6];
}
