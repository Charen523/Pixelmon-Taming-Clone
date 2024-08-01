using System;
using System.Collections.Generic;
using System.Numerics;

[Serializable]
public class UserData
{ 
    public string userId;
    public string userName = "Name한글";

    public string _gold = "1000";
    public BigInteger gold;

    public int diamond = 100;
    public int seed = 10;
    public int food = 10;
    //public int key;

    public int userLv = 1;
    public int userExp = 0; //매우 커질 가능성 있음

    public int eggLv = 1;
    public int eggCount = 3;
    public int fullGaugeCnt = 0;

    public string curStageRcode = "STG00101";
    public int curDifficulty = 0;
    public int curStageCount = 0;

    public string questId = "R000_Q1";

    public int[] UpgradeLvs = { 1, 1, 1, 1, 1, 1, 1};
    public bool[] isLockedSlot = {false, false, false, true, true};
    public MyPixelmonData[] equippedPxms = new MyPixelmonData[5];
    public MyPixelmonData[] ownedPxms = new MyPixelmonData[20];

    public int[] equippedSkills = new int[5];
    public List<MyAtvData> ownedSkills = new List<MyAtvData>();
    //public string[] OwnedActiveSkills; 만약 스킬 강화 생기면 string-> 스킬로 바꾸기.
    public FieldData[] fieldDatas = new FieldData[6];
    //public int[] DungeonLvs = { 1, 1, 1 };
}
