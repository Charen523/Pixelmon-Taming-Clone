using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDungeonEnterPopup : UIBase
{
    public DungeonTab dungeonTab;

    #region UI
    [SerializeField] private TextMeshProUGUI dgName;
    [SerializeField] private TextMeshProUGUI dgLv;

    [SerializeField] private TextMeshProUGUI rwdExpValue;

    [SerializeField] private Sprite[] rwdDgIcons;
    [SerializeField] private Image rwdDgIcon;
    [SerializeField] private TextMeshProUGUI rwdDgType;
    [SerializeField] private TextMeshProUGUI rwdDgValue;

    [SerializeField] private Button EnterBtn;
    [SerializeField] private Button ExitBtn;
    #endregion

    public DungeonType curDgType;

    public void OnEnterBtn()
    {
        switch(curDgType)
        {//키소모+맵변경
            case DungeonType.Gold:
                SaveManager.Instance.SetFieldData(nameof(SaveManager.Instance.userData.keyGold), -1, true);
                MapManager.Instance.OnMapChanged((int)MapList.DgGold);
                break;
            case DungeonType.Seed:
                SaveManager.Instance.SetFieldData(nameof(SaveManager.Instance.userData.keySeed), -1, true);
                MapManager.Instance.OnMapChanged((int)MapList.DgGold); //TODO: 맵 변경
                break;
            case DungeonType.Skill:
                SaveManager.Instance.SetFieldData(nameof(SaveManager.Instance.userData.keySkill), -1, true);
                MapManager.Instance.OnMapChanged((int)MapList.DgGold); //TODO: 맵 변경
                break;
        }

        UIManager.Instance.tabOverlay.onClick.Invoke();
        gameObject.SetActive(false);
    }

    public void OnClrBtn()
    {
        //열쇠 소모하기
        //보상주기
    }
}
