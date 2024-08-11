using System;
using UnityEngine;

public class UIDungeonProgress : UIBase
{
    public void EscapeBtn()
    {
        StageManager.Instance.isDungeon = false;
        gameObject.SetActive(false);
    }
}