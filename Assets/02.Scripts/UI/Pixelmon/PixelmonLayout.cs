using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonLayout : MonoBehaviour
{
    public Image thumbnailIcon;
    public GameObject plusIcon;
    public GameObject lockIcon;

    public void InsertIcon(PixelmonData data)
    {
        plusIcon.SetActive(false);
        thumbnailIcon.sprite = data.icon;
    }

    public void DeleteIcon()
    {
        thumbnailIcon.sprite = null;
        plusIcon.SetActive(true);
    }
    public void UnLockedIcon()
    {
        lockIcon.gameObject.SetActive(false);
        plusIcon.SetActive(true);
    }
}
