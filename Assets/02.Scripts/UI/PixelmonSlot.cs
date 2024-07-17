using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonSlot : MonoBehaviour
{
    public PixelmonData pixelmonData;
    public PixelmonTab pixelmontab;
    [SerializeField]
    protected RectTransform rectTr;
    public bool isPossessed => pixelmonData.isPossessed;
    public int slotIndex;
    [SerializeField]
    protected Button slotBtn;
    [SerializeField]
    protected Image slotIcon;
    // Start is called before the first frame update


    public virtual void InitSlot(PixelmonTab tab, PixelmonData data)
    {
        pixelmontab = tab;
        pixelmonData = data;
        slotIcon.sprite = pixelmonData.icon;
        slotBtn.onClick.AddListener(OnClick);
        //if (isLocked) lockIcon.SetActive(false);
    }

    protected virtual void OnClick()
    {
        if (pixelmontab.tabState == TabState.Normal)
        {
            pixelmontab.OnClickSlot(pixelmonData.id, rectTr);
        }
        else if(pixelmontab.tabState == TabState.Equip)
        {
            pixelmontab.EquipedPixelmon(gameObject.transform.GetSiblingIndex());
        }
    }
}
