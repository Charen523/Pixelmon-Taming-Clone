using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PixelmonManager : Singleton<PixelmonManager>
{
    public UnityAction<int, PixelmonData> equipAction;
    public UnityAction<int> unEquipAction;
    public Pixelmon[] equippedPixelmon;
    private Player player;
    private InventoryManager inven;
    // Start is called before the first frame update
    void Start()
    {
        inven = InventoryManager.Instance;
        inven.userData.equippedPxms = new PixelmonData[5];
        player = Player.Instance;
        equipAction += Equipped;
        unEquipAction += UnEquipped;
        //InitEquippedPixelmon();
    }
    
    private void InitEquippedPixelmon()
    {
        for (int i = 0; i < 5; i++)
        {
            if (inven.userData.equippedPxms[i] == null)
            {
                UnEquipped(i);
            }
            else
            {
                Equipped(i, inven.userData.equippedPxms[i]);
            }
        }
    }

    private void Equipped(int index, PixelmonData data)
    {
        player.pixelmons[index] = equippedPixelmon[index];
        player.pixelmons[index].gameObject.SetActive(true);
        player.pixelmons[index].data = data;
        player.pixelmons[index].fsm.ChangeState(player.fsm.currentState);
        //inven.SetData("equipedPixelmons", player.pixelmons);
    }

    private void UnEquipped(int index)
    {
        player.pixelmons[index].fsm.InvokeAttack(false);
        player.pixelmons[index].gameObject.SetActive(false);
        player.pixelmons[index]= null;
        //inven.SetData("equipedPixelmons", player.pixelmons);
    }
}
