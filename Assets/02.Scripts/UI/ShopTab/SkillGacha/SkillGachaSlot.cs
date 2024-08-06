using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGachaSlot : MonoBehaviour
{
    public Image Bg;
    public Image Icon;

    public void InitSlot(Sprite bg, Sprite icon)
    {
        Bg.sprite = bg;
        Icon.sprite = icon;
    }
}
