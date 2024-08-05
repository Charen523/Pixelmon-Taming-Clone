using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapList
{
    Theme1,
    Theme2, 
    Theme3,
    DgGold,
    DgSeed,
    DgSkill
}

public class MapManager : Singleton<MapManager>
{
    public GameObject[] mapList;
}
