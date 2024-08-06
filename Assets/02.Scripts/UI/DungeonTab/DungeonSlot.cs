using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DungeonType
{
    Gold, 
    Seed,
    Skill
}

public class DungeonSlot : MonoBehaviour
{
    public DungeonTab dungeonTab;
    public DungeonType type;
    public TextMeshProUGUI keyTxt;

    public void enterDungeonBtn()
    {
        dungeonTab.popup.SetActive(true);
        dungeonTab.popup.curDgType = type;
    }
}
