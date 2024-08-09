using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PixelmonManager : Singleton<PixelmonManager>
{
    public UnityAction<int, MyPixelmonData> equipAction;
    public UnityAction<int> unEquipAction;
    public UnityAction<int> unlockSlotAction;

    private SaveManager saveManager;
    private UserData userData;
    public Player player;
    public PixelmonTab pxmTab;

    private List<PixelmonData> pxmData;
    public Pixelmon[] equippedPixelmon;
    public PixelmonStatus upgradeStatus = new PixelmonStatus();

    public Sprite plusIcon;
    public Sprite defaultBg;

    public float perHp = 0;
    public float perDef = 0;
    // Start is called before the first frame update
    void Start()
    {
        pxmData = DataManager.Instance.pixelmonData.data;
        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
        player = Player.Instance;
        equipAction += Equipped;
        unEquipAction += UnEquipped;
        unlockSlotAction += unlockSlotAction;
        InitUpgradeStatus();
        InitEquippedPixelmon();
        InitPlayerStat();
    }

    public void InitPlayerStat()
    {
        foreach (MyPixelmonData pxm in userData.ownedPxms)
        {
            if (!pxm.isOwned) continue;
            Debug.Log(pxm.id);
            perHp += pxm.ownEffectValue[0];
            perDef += pxm.ownEffectValue[1];
        }
        player.statHandler.UpdateStats(perHp, perDef);
        player.healthSystem.currentHealth = player.statHandler.maxHp;
    }

    public void UpdatePlayerStat(float hp, float def)
    {
        perHp += hp;
        perDef += def;
        player.statHandler.UpdateStats(perHp, perDef, hp/100);
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

    public void UnLockedSlot(int index)
    {
        switch (index) 
        {
            case 5:
                pxmTab.equipData[2].isLocked = false;
                pxmTab.equipData[2].stateIcon.sprite = plusIcon;
                break;
            case 10:
                pxmTab.equipData[3].isLocked = false;
                pxmTab.equipData[3].stateIcon.sprite = plusIcon;
                break;
            case 15:
                pxmTab.equipData[4].isLocked = false;
                pxmTab.equipData[4].stateIcon.sprite = plusIcon;
                unlockSlotAction = null;
                break;
            default:
                break;
        }
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
