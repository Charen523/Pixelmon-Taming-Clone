using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PixelmonManager : Singleton<PixelmonManager>
{
    public UnityAction<int, MyPixelmonData> equipAction;
    public UnityAction<int> unEquipAction;

    private SaveManager saveManager;
    private UserData userData;
    public Player player;
    public PixelmonTab pxmTab;

    private List<PixelmonData> pxmData;
    public Pixelmon[] equippedPixelmon;
    public PixelmonStatus upgradeStatus = new PixelmonStatus();

    public Sprite plusIcon;
    public Sprite defaultBg;
    // Start is called before the first frame update
    void Start()
    {
        pxmData = DataManager.Instance.pixelmonData.data;
        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
        player = Player.Instance;
        equipAction += Equipped;
        unEquipAction += UnEquipped;
        InitUpgradeStatus();
        InitEquippedPixelmon();
    }
    
    private void InitUpgradeStatus()
    {
        int[] upgradeArr = userData.UpgradeLvs;

        upgradeStatus.Atk = upgradeArr[0];
        upgradeStatus.Cri = upgradeArr[1] * 0.05f;
        upgradeStatus.CriDmg = upgradeArr[2] * 0.5f;
        upgradeStatus.Dmg = upgradeArr[3] * 0.1f;
        upgradeStatus.SDmg = upgradeArr[4] * 0.2f;
        upgradeStatus.SCri = upgradeArr[5] * 0.025f;
        upgradeStatus.SCriDmg = upgradeArr[6] * 0.3f;
    }

    private void InitEquippedPixelmon()
    {
        for (int i = 0; i < 5; i++)
        {
            if (userData.equippedPxms[i].isEquipped)
            {
                
                Equipped(i, userData.equippedPxms[i]); 
            }
        }
        //player.LocatedPixelmon();
    }

    private async void Equipped(int index, MyPixelmonData myData)
    {
        player.pixelmons[index] = equippedPixelmon[index];
        player.pixelmons[index].gameObject.SetActive(true);
        player.pixelmons[index].myData = myData;
        player.pixelmons[index].data = pxmData[myData.id];
        player.pixelmons[index].fsm.ChangeState(player.fsm.currentState);
        player.pixelmons[index].fsm.anim.runtimeAnimatorController = await ResourceManager.Instance.LoadAsset<RuntimeAnimatorController>(myData.rcode, eAddressableType.animator);
        player.pixelmons[index].InitPxm();
        player.currentPixelmonCount++;
        player.LocatedPixelmon();
        SkillManager.Instance.ExecuteSkill(player.pixelmons[index], index);
    }

    private void UnEquipped(int index)
    {
        player.pixelmons[index].fsm.InvokeAttack(false);
        player.pixelmons[index].gameObject.SetActive(false);
        player.pixelmons[index]= null;
        player.currentPixelmonCount--;
        player.LocatedPixelmon();

        saveManager.SetData("equippedPxms", userData.equippedPxms);
    }

    public void UnLockedPixelmon(int index)
    {
        pxmTab.unLockAction?.Invoke(index);
    }

    public PixelmonData FindPixelmonData(int id)
    {
        return DataManager.Instance.pixelmonData.data[id];
    }

    public void ApplyStatus(PixelmonData data, MyPixelmonData myData)
    {
        if (myData.isEquipped)
        {
            foreach (var equipData in pxmTab.equipData)
            {
                if (equipData.myPxmData != null && equipData.myPxmData.id == myData.id)
                {
                    equipData.SetPxmLv();
                    Player.Instance.pixelmons[equipData.slotIndex].status.InitStatus(data, myData);
                    break;
                }
            }
        }
    }
}
