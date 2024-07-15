using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonSlot : MonoBehaviour
{
    public PixelmonData pixelmonData;
    public GameObject lockIcon;
    [SerializeField]
    private Image SlotIcon;
    // Start is called before the first frame update

    public void InitSlot(PixelmonData data)
    {

    }

    public void RemoveInfo()
    {
        pixelmonData = null;
        SlotIcon.sprite = null;
    }
}
