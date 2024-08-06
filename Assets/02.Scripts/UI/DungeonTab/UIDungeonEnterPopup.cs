using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDungeonEnterPopup : UIBase
{
    public DungeonTab dungeonTab;
    public DungeonSlot curSlot;
    public DungeonType type;

    private int lv;

    #region UI
    [SerializeField] private TextMeshProUGUI dgName;
    [SerializeField] private TextMeshProUGUI dgLv;

    [SerializeField] private Sprite[] rwdDgIcons;
    [SerializeField] private Image rwdDgIcon;
    [SerializeField] private TextMeshProUGUI rwdDgType;
    [SerializeField] private TextMeshProUGUI rwdDgValue;
    [SerializeField] private TextMeshProUGUI keyValue;
    
    [SerializeField] private GameObject ClrBtn;
    #endregion

    public void SetDgPopup(string name)
    {
        type = curSlot.type;

        dgName.text = name;
        lv = dungeonTab.dgLv[(int)type];
        dgLv.text = $"최고 {lv}단계";

        rwdDgIcon.sprite = rwdDgIcons[(int)type];
        string curRwdTxt = (int)type switch
        {
            0 => "Gold",
            1 => "Seed",
            2 => "Skill",
            _ => ""
        };
        rwdDgType.text = curRwdTxt;
        
        if (type == DungeonType.Gold)
        {
            BigInteger curRwdValue = Calculater.CalPrice(lv, curSlot.rwdBNum, curSlot.rwdD1, curSlot.rwdD2);
            rwdDgValue.text = Calculater.NumFormatter(curRwdValue);
            keyValue.text = dungeonTab.GetKeyString((int)type);
        }

        if (lv == 1)
        {
            ClrBtn.gameObject.SetActive(false);
        }
        else
        {
            ClrBtn.gameObject.SetActive(true);
        }
    }

    public void OnEnterBtn()
    {
        curSlot.UseKey();
        MapManager.Instance.OnMapChanged((int)type);

        UIManager.Instance.tabOverlay.onClick.Invoke();
        dungeonTab.dgProgress.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClrBtn()
    {
        curSlot.UseKey();
        
        switch(type)
        {
            case DungeonType.Gold:
                BigInteger getValue = Calculater.CalPrice(lv - 1, curSlot.rwdBNum, curSlot.rwdD1, curSlot.rwdD2);
                SaveManager.Instance.SetFieldData("gold", getValue, true);
                break;
            case DungeonType.Seed:
                break;
            case DungeonType.Skill:
                break;
        }
    }
}
