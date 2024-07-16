using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonSlot : MonoBehaviour
{
    public PixelmonData pixelmonData;
    private PixelmonTab pixelmontab;
    [SerializeField]
    private RectTransform rectTr;
    public bool isPossessed => pixelmonData.isPossessed;
    public GameObject lockIcon;
    public bool isLocked = true;
    public int slotIndex;
    [SerializeField]
    private Button slotBtn;
    [SerializeField]
    private Image SlotIcon;
    // Start is called before the first frame update


    public void InitSlot(PixelmonTab tab, PixelmonData data)
    {
        pixelmontab = tab;
        pixelmonData = data;
        slotBtn.onClick.AddListener(OnClick);
        //if (isLocked) lockIcon.SetActive(false);
    }

    public void OnClick()
    {
        pixelmontab.OnClickSlot(pixelmonData.id, rectTr);
    }

    public void Equip(PixelmonData data)
    {
        pixelmonData = data;
        SlotIcon.sprite = data.icon;
    }

    public void RemoveInfo()
    {
        pixelmonData = null;
        SlotIcon.sprite = null;
    }
}
