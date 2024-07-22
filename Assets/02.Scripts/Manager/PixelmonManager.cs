using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PixelmonManager : Singleton<PixelmonManager>
{
    public UnityAction<int, MyPixelmonData> equipAction;
    public UnityAction<int> unEquipAction;
    public Pixelmon[] equippedPixelmon;
    private Player player;
    private SaveManager saveManager;
    private List<PixelmonData> pxmData;
    // Start is called before the first frame update
    void Start()
    {
        pxmData = DataManager.Instance.pixelmonData.data;
        saveManager = SaveManager.Instance;
        player = Player.Instance;
        equipAction += Equipped;
        unEquipAction += UnEquipped;
        InitEquippedPixelmon();
    }
    
    private void InitEquippedPixelmon()
    {
        for (int i = 0; i < 5; i++)
        {
            if (saveManager.userData.equippedPxms[i].isEquiped)
            { 
                Equipped(i, saveManager.userData.equippedPxms[i]);
            }
        }
    }

    private void Equipped(int index, MyPixelmonData myData)
    {
        player.pixelmons[index] = equippedPixelmon[index];
        player.pixelmons[index].gameObject.SetActive(true);
        player.pixelmons[index].myData = myData;
        player.pixelmons[index].data = pxmData[myData.id];
        player.pixelmons[index].fsm.ChangeState(player.fsm.currentState);
        player.currentPixelmonCount++;
        player.LocatedPixelmon();
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

    public PixelmonData FindPixelmonData(MyPixelmonData myData)
    {
        return DataManager.Instance.pixelmonData.data.Find((obj) => obj.id == myData.id);
    }
}
