using System;
using TMPro;
using UnityEngine;

public class DungeonTab : UIBase
{
    SaveManager saveManager;
    UserData userData;

    private int keyGold => saveManager.userData.keyGold;
    private int keySeed => saveManager.userData.keySeed;
    private int keySkill => saveManager.userData.keySkill;

    #region UI
  [SerializeField] private TextMeshProUGUI keyChargeTime;
    [SerializeField] private DungeonSlot[] dungeonSlots;
    public UIDungeonEnterPopup popup;
    #endregion

    private bool isInit;

    private async void Awake()
    {
        isInit = false;
        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
        popup = await UIManager.Show<UIDungeonEnterPopup>();
        popup.dungeonTab = this;
        isInit = true;
    }

    private void OnEnable()
    {
        if (isInit)
        {
            string lastTime = userData.lastConnectTime;
            if (DateTime.TryParse(lastTime, out DateTime date))
            {
                if (date.Date > DateTime.Now.Date)
                {
                    //미래
                    saveManager.SetFieldData(nameof(userData.keyGold), 3);
                    saveManager.SetFieldData(nameof(userData.keySeed), 3);
                    saveManager.SetFieldData(nameof(userData.keySkill), 3);
                }
                else if (date.Date < DateTime.Now.Date)
                {
                    //과거: 버그임.
                    Debug.LogError("과거로 오면 안됩니다...앱 강제종료.");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
                }
            }
            saveManager.SetFieldData(nameof(userData.lastConnectTime), DateTime.Now.ToString());
        }
    }

    private void Update()
    {//TODO: 최적화 대상. coroutine등으로 초에 1번 부를 것.
        DateTime now = DateTime.Now;
        DateTime midnight = now.Date.AddDays(1);
        TimeSpan timeUntilMidnight = midnight - now;

        keyChargeTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                          timeUntilMidnight.Hours,
                                          timeUntilMidnight.Minutes,
                                          timeUntilMidnight.Seconds);
    }
}