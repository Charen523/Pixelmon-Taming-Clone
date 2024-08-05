using TMPro;
using UnityEngine;

public class DungeonTab : UIBase
{
    SaveManager saveManager;

    #region UI
    [SerializeField] private TextMeshProUGUI keyChargeTime;
    [SerializeField] private DungeonSlot[] dungeonSlots;
    private UIBase popup;
    #endregion

    private async void Awake()
    {
        saveManager = SaveManager.Instance;
        popup = await UIManager.Show<DungeonEnterPopup>();

        for (int i = 0; i < dungeonSlots.Length; i++)
        {
            
        }
    }
}