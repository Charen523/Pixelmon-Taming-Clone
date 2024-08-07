using System;
using System.Collections.Generic;
using System.Numerics;

[Serializable]
public class UserData
{ 
    public string userId;
    public string userName = "Name한글";
    public string lastConnectTime;

    public string _gold = "33348332500000";
    public BigInteger gold = 0;

    public int diamond = 10000;
    public int seed = 10;
    public int food = 10;
    public int skillTicket = 0;
    public int key0 = 3;
    public int key1 = 3;
    public int key2 = 3;

    public int userLv = 1;
    public string _exp = "0";
    public BigInteger userExp = 0;

    public int eggLv = 1;
    public int eggCount = 3;
    public int fullGaugeCnt = 0;
    public bool isLvUpMode = false;
    public string startLvUpTime = null;
    public float skipTime = 0;
    public bool isGetPxm = true;
    public bool isOwnedPxm = false;
    public PixelmonData hatchPxmData = null;
    public MyPixelmonData hatchMyPxmData = null;
    public PxmPsvData[] psvData = new PxmPsvData[4];    

    public string curStageRcode = "STG00101";
    public int curDifficulty = 0;
    public int curStageCount = 0;

    public string questId = "0000Q1";
    public int questProgress = 1;

    public int[] UpgradeLvs = { 1, 1, 1, 1, 1, 1, 1};
    public bool[] isLockedSlot = {false, false, false, true, true };
    public MyPixelmonData[] equippedPxms = new MyPixelmonData[5];
    public MyPixelmonData[] ownedPxms = new MyPixelmonData[20];

    public int[] equippedSkills = {-1, -1, -1, -1, -1};
    public List<MyAtvData> ownedSkills = new List<MyAtvData>();
    public FieldData[] fieldDatas = new FieldData[6];
    public int[] bestDgLvs = { 1, 1, 1 };
}
