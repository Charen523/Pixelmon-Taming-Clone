using UnityEngine;

public class UIDungeonProgress : UIBase
{
    private int rwdBNum;
    private int rwdD1;
    private int rwdD2;
    public void EscapeBtn()
    {
        MapManager.Instance.OnMapChanged((int)MapList.Theme1);
        gameObject.SetActive(false);
    }
}