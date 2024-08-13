using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public PixelmonLayout layout;
    public float[] skillCoolTime = new float[5];

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
            if (prefablst.Count > i && prefablst[i] != null)
                SpawnSkill(i);
        }
    }

    public void ExecuteSkill(Pixelmon pxm, int index)
    {
        if (pxm.myData.atvSkillId != -1 && pxm.data.skillCoroutine == null)
        {
            pxm.data.layoutIndex = index;
            pxm.data.skillCoroutine = StartCoroutine(SkillAction(pxm, pxm.data.layoutIndex));
        }
    }

    public IEnumerator SkillAction(Pixelmon pxm, int index)
    {
        yield return new WaitUntil(() => pxm.fsm.target != null);
        ActiveData data = skillTab.allData[pxm.myData.atvSkillId].atvData;
        MyAtvData myData = skillTab.allData[pxm.myData.atvSkillId].myAtvData;
        while (myData.isEquipped)
        {
            yield return new WaitUntil(
                () => pxm.fsm.target != null &&
                pxm.fsm.currentState == pxm.fsm.AttackState ||
                !myData.isEquipped);
            if (!myData.isEquipped) yield break;
            data.isCT = true;
            actionStorage[data.id]?.Invoke(pxm, data, myData);
            skillCoolTime[index] = data.coolTime;
            while (0 <= skillCoolTime[index])
            {
                skillCoolTime[index] -= Time.deltaTime;
                if (layout.timer[index] != null)
                    layout.timer[index].fillAmount = skillCoolTime[index] / data.coolTime;
                yield return null;
            }
            skillCoolTime[index] = 0;
            data.isCT = false;
        }
        pxm.data.layoutIndex = -1;
        pxm.data.skillCoroutine = null;
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

    public void UnEquipSkill(int slotIndex)
    {
        skillTab.equipData[slotIndex].UnEquipAction();
    }

    public void AddSkill(int id)
    {
        skillTab.AddSkillAction?.Invoke(id);
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
