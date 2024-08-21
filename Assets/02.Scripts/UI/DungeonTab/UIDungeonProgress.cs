
using UnityEngine;
using UnityEngine.UI;

public class UIDungeonProgress : UIBase
{
    [SerializeField] private GameObject popup;
    [SerializeField] private Button btn;

    public void StopTime()
    {
        btn.enabled = false;
        Time.timeScale = 0;
    }

    public void GoTime()
    {
        btn.enabled = true;
        Time.timeScale = 1;
    }

    public void EscapeBtn()
    {
        Time.timeScale = 1;
        btn.enabled = true;
        popup.SetActive(false);
        StageManager.Instance.isDungeon = false;
        SaveManager.Instance.SetFieldData("key" + StageManager.Instance.dgIndex, 1, true);
        gameObject.SetActive(false);
    }
}