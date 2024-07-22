using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class PixelmonLayout : MonoBehaviour
{
    public Image[] backgrounds;
    public Image[] thumbnailIcon;
    public GameObject[] plusIcon;
    public GameObject[] lockIcon;
    public Sprite defaultBg;
    private void Start()
    {
        PixelmonManager pxmManager = PixelmonManager.Instance;
        pxmManager.equipAction += InsertIcon;
        pxmManager.unEquipAction += DeleteIcon;
    }

    public void InsertIcon(int index, MyPixelmonData data)
    {
        plusIcon[index].SetActive(false);
        //backgrounds[index].sprite = data.bgIcon;
        thumbnailIcon[index].sprite = PixelmonManager.Instance.FindPixelmonData(data).icon;
    }

    public void DeleteIcon(int index)
    {
        backgrounds[index].sprite = defaultBg;
        thumbnailIcon[index].sprite = null;
        plusIcon[index].SetActive(true);
    }
    public void UnLockedIcon(int index)
    {
        lockIcon[index].gameObject.SetActive(false);
        plusIcon[index].SetActive(true);
    }
}
