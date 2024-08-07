using System;
using UnityEngine;

public class UIDungeonProgress : UIBase
{
    private void OnDestroy()
    {
        MapManager.Instance.OnMapChanged((int)MapList.Theme1);
    }

    public void EscapeBtn()
    {
        StageManager.Instance.isDungeon = false;
        MapManager.Instance.OnMapChanged((int)MapList.Theme1);
        gameObject.SetActive(false);
    }
}