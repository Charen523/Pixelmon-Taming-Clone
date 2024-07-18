using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class UserData
{ 
    public string userId;
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
    public PixelmonData[] equipedPixelmons = new PixelmonData[5];
    public PixelmonData[] prossessedPixelmons;
    public PixelmonData[] farmPixelmons = new PixelmonData[6];
    public int[] farmTimer = new int[6];
}
