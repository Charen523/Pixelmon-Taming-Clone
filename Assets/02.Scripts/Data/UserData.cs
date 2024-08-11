using System;
using System.Collections.Generic;
using System.Numerics;

[Serializable]
public class UserData
{
    #region Private User
    public string userId;
    public string userName = "song이름";
    public int userLv = 1;
    public string _exp = "0";
    public BigInteger userExp = 0;
    #endregion

    #region Goods
    public string _gold = "0";
    public BigInteger gold = 0;
    public int diamond = 0;
    public int seed = 3;
    public int food = 0;
    public int skillTicket = 0;
    #endregion

    #region Tutorial
    public bool isDoneTutorialMsg = false;
    public bool isOpenSkillTab = false;
    public bool isOpenFarmTab = false;
    public bool isOpenDungeonTab = false;
    #endregion

    #region Stage Info
    public string stageRcode = "STG_N1";
    public string curStage = "000101";
    public int curHuntCount = 0;
    public bool isInfinite;
    #endregion

    #region Main Quest
    public string questId = "0000Q1";
    public int questProgress = 1;
    #endregion

    #region EggLv
    public int eggLv = 1;
    public int fullGaugeCnt = 0;
    public bool isLvUpMode = false;
    public string startLvUpTime = null;
    public float skipTime = 0;
    #endregion

    #region Hatch Egg
    public int eggCount = 3;
    public bool isGetPxm = true;
    public bool isOwnedPxm = false;
    public PixelmonData hatchPxmData = null;
    public MyPixelmonData hatchMyPxmData = null;
    public PxmPsvData[] psvData = new PxmPsvData[4];
    #endregion

    #region Upgrade Tab
    public int[] UpgradeLvs = { 1, 1, 1, 1, 1, 1, 1 };
    public bool[] isLockedSlot = { false, false, false, true, true };
    #endregion

    #region Pixelmon Tab
    public MyPixelmonData[] equippedPxms = new MyPixelmonData[5];
    public MyPixelmonData[] ownedPxms = new MyPixelmonData[20];
    #endregion 

    #region Skill Tab
    public int[] equippedSkills = {-1, -1, -1, -1, -1};
    public List<MyAtvData> ownedSkills = new List<MyAtvData>();
    #endregion

    #region Farm Tab
    public FieldData[] fieldDatas = new FieldData[6];
    #endregion

    #region Dungeon Tab
    public int[] bestDgLvs = { 1, 1, 1 };
    public int key0 = 3;
    public int key1 = 3;
    public int key2 = 3;
    public string lastConnectTime;
    #endregion
}
