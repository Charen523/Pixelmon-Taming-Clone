using System;

[Serializable]
public class UserData
{ 
    public string userId;
    public string userName;

    public int gold;
    public int diamond;
    public int seed;
    public int food;
    //public int key;

    public int userLv;
    public int userExp;

    public int eggLv;
    public int eggCount;

    public string curStageRcode;
    public int curStageCount;

    //public string curQuestRode;

    //public int[] StatUpLvs = new int[10]; //나중에 개수 보고 바꾸기.
    public PixelmonData[] equippedPxms = new PixelmonData[5]; //string으로 바꾸기.
    public PixelmonData[] OwnedPxms;
    //public string[] OwnedActiveSkills; 만약 스킬 강화 생기면 string-> 스킬로 바꾸기.
    public FieldData[] fieldDatas = new FieldData[6];
    //public int[] DungeonLvs = new int [3];
}
