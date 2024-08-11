using System;
using UnityEngine;

public class UIDungeonProgress : UIBase
{
    Transform middleBar;
    Transform bottomBar;

    public override void Opened(object[] param)
    {
        middleBar = UIManager.Get<UIMiddleBar>().transform;
        bottomBar = UIManager.Instance.parents[2].GetChild(0);

        Vector3 middleBarPosition = middleBar.position;
        middleBarPosition.y -= 620;
        middleBar.position = middleBarPosition;

        Vector3 bottomBarPosition = bottomBar.position;
        bottomBarPosition.y -= 620;
        bottomBar.position = bottomBarPosition;
    }

    private void OnDestroy()
    {
        MapManager.Instance.OnMapChanged((int)MapList.Theme1);

        Vector3 middleBarPosition = middleBar.position;
        middleBarPosition.y += 620;
        middleBar.position = middleBarPosition;

        Vector3 bottomBarPosition = bottomBar.position;
        bottomBarPosition.y += 620;
        bottomBar.position = bottomBarPosition;
    }

    public void EscapeBtn()
    {
        StageManager.Instance.isDungeon = false;
        MapManager.Instance.OnMapChanged((int)MapList.Theme1);
        gameObject.SetActive(false);
    }
}