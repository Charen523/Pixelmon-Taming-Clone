using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DungeonTab : UIBase
{
    SaveManager saveManager;
    UserData userData;

    public int keyGold => saveManager.userData.keyGold;
    public int keySeed => saveManager.userData.keySeed;
    public int keySkill => saveManager.userData.keySkill;

    #region UI
    [SerializeField] private TextMeshProUGUI keyChargeTime;
    [SerializeField] private DungeonSlot[] dungeonSlots;
    public UIDungeonEnterPopup dgPopup;
    public UIDungeonProgress dgProgress;
    #endregion

    private bool isInit = false;
    private Coroutine chargeTimeCoroutine;

    private async void Awake()
    {
        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
        
        dgPopup = await UIManager.Show<UIDungeonEnterPopup>();
        dgPopup.dungeonTab = this;
        
        dgProgress = await UIManager.Show<UIDungeonProgress>();

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
                    //하루 뒤.
                    ResetKey();
                }
                else if (date.Date < DateTime.Now.Date)
                {
                    //혹시나 시차때문일지 모르니 강종로직 뺌.
                    Debug.LogError("과거로 오면 안됩니다...");
                }
            }
            saveManager.SetFieldData(nameof(userData.lastConnectTime), DateTime.Now.ToString());

            if (chargeTimeCoroutine != null)
            {
                StopCoroutine(chargeTimeCoroutine);
            }
            chargeTimeCoroutine = StartCoroutine(UpdateChargeTime());
        }
    }

    private void OnDisable()
    {
        if (chargeTimeCoroutine != null)
        {
            StopCoroutine(chargeTimeCoroutine);
            chargeTimeCoroutine = null;
        }
    }

    private IEnumerator UpdateChargeTime()
    {
        DateTime midnight = DateTime.Now.Date.AddDays(1);

        while (true)
        {
            DateTime now = DateTime.Now;
            TimeSpan timeUntilMidnight = now - now.Date.AddDays(1);

            keyChargeTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                timeUntilMidnight.Hours,
                                                timeUntilMidnight.Minutes,
                                                timeUntilMidnight.Seconds);
            if (now >= midnight)
            {
                ResetKey();
                midnight = midnight.AddDays(1);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void ResetKey()
    {
        saveManager.SetFieldData(nameof(userData.keyGold), 3);
        saveManager.SetFieldData(nameof(userData.keySeed), 3);
        saveManager.SetFieldData(nameof(userData.keySkill), 3);
    }

    public string GetKeyString(DungeonType type)
    {
        string result = "";

        switch (type)
        {
            case DungeonType.Gold:
                result = $"{keyGold}/3";
                break;
            case DungeonType.Seed:
                result = $"{keySeed}/3";
                break;
            case DungeonType.Skill:
                result = $"{keySkill}/3";
                break;
        }

        return result;
    }
}
