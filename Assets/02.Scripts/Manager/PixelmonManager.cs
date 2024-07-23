using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PixelmonManager : Singleton<PixelmonManager>
{
    public UnityAction<int, MyPixelmonData> equipAction;
    public UnityAction<int> unEquipAction;
    public UnityAction<int> unLockAction;

    private Player player;
    private UserData userData;
    private List<PixelmonData> pxmData;
    public Pixelmon[] equippedPixelmon;

    public Sprite plusIcon;
    public Sprite defaultBg;
    // Start is called before the first frame update
    void Start()
    {
        pxmData = DataManager.Instance.pixelmonData.data;
        userData = SaveManager.Instance.userData;
        player = Player.Instance;
        equipAction += Equipped;
        unEquipAction += UnEquipped;
        InitEquippedPixelmon();
    }
    
    private void InitEquippedPixelmon()
    {
        for (int i = 0; i < 5; i++)
        {
            if (userData.equippedPxms[i].isEquiped)
            {
                
                Equipped(i, userData.equippedPxms[i]); 
            }
        }
        player.LocatedPixelmon();
    }

    private void Equipped(int index, MyPixelmonData myData)
    {
        player.pixelmons[index] = equippedPixelmon[index];
        player.pixelmons[index].gameObject.SetActive(true);
        player.pixelmons[index].myData = myData;
        player.pixelmons[index].data = pxmData[myData.id];
        player.pixelmons[index].fsm.ChangeState(player.fsm.currentState);
        //player.pixelmons[index].InitPxm();
        player.currentPixelmonCount++;
        //inven.SetData("equipedPixelmons", player.pixelmons);
    }

    private void UnEquipped(int index)
    {
        player.pixelmons[index].fsm.InvokeAttack(false);
        player.pixelmons[index].gameObject.SetActive(false);
        player.pixelmons[index]= null;
        player.currentPixelmonCount--;
        player.LocatedPixelmon();
        //inven.SetData("equipedPixelmons", player.pixelmons);
    }

    public PixelmonData FindPixelmonData(int id)
    {
        return DataManager.Instance.pixelmonData.data[id];
    }
}
