using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillManager : Singleton<SkillManager>
{
    public SkillTab skillTab;

    public Transform skillStorage;
    public Dictionary<int, List<BaseSkill>> dicSkill = new Dictionary<int, List<BaseSkill>>();
    public List<BaseSkill> prefablst = new List<BaseSkill>();

    public List<UnityAction<Pixelmon, ActiveData, MyAtvData>> actionStorage = new List<UnityAction<Pixelmon, ActiveData, MyAtvData>>();
    private UnityAction<Pixelmon, ActiveData, MyAtvData> skillAction;

    public DataManager dataManager;
    public SaveManager saveManager;
    protected override void Awake()
    {
        base.Awake();
        dataManager = DataManager.Instance;
        saveManager = SaveManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        skillAction += OnSkillAction;
        InitStorage();
    }

    public void InitStorage()
    {
        for(int i = 0; i < dataManager.activeData.data.Count; i++)
        {
            actionStorage.Add(skillAction);
            if(i == 0 || i == 1)
                SpawnSkill(i);
        }
    }

    public void ExecuteSkill(Pixelmon pxm)
    {
        if(pxm.myData.atvSkillId != -1)
            StartCoroutine(SkillAction(pxm));
    }

    public IEnumerator SkillAction(Pixelmon pxm)
    {
        var myData = saveManager.userData.ownedSkills[pxm.myData.atvSkillId];
        var data = dataManager.activeData.data[pxm.myData.atvSkillId];
        while (myData.isEquipped)
        {
            yield return new WaitUntil(
                () => pxm.fsm.target != null && 
                pxm.fsm.currentState == pxm.fsm.AttackState ||
                !myData.isEquipped);
            if (!myData.isEquipped) yield break;
            data.isCT = true;
            actionStorage[data.id]?.Invoke(pxm, data, myData);
            yield return new WaitForSeconds(1);
            data.isCT = false;
        }
    }

    public void OnSkillAction(Pixelmon pxm, ActiveData atvData, MyAtvData myAtvData)
    {
        var targets = pxm.fsm.Search(atvData.count);
        if(targets.Count == 0) return;
        for (int i = 0; i < atvData.count; i++)
        {
            //프리팹 생성
            var skill = dicSkill[pxm.myData.atvSkillId][i];
            skill.gameObject.SetActive(true);
            //조건 설정
            skill.InitInfo(pxm, targets[i].gameObject, atvData, myAtvData);
        }
    }

    public void UnEquipSkill(int slotIndex, int skillId)
    {
        skillTab.equipData[slotIndex].UnEquipAction();
        skillTab.allData[skillId].UnEquipAction();
    }

    public void SpawnSkill(int id)
    {
        if (!dicSkill.ContainsKey(id))
        {
            dicSkill.Add(id , new List<BaseSkill>());
            for (int i = 0; i < dataManager.activeData.data[id].count; i++)
            {
                var skill = Instantiate(prefablst[id], skillStorage);
                dicSkill[id].Add(skill);
                skill.gameObject.SetActive(false);
            }
        }
    }
}

public enum AtvSkillType
{
    Single,
    Area,
    Special
}
