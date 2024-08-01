using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillManager : Singleton<SkillManager>
{
    public SkillTab skillTab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnEquipSkill(int slotIndex, int skillId) 
    {
        skillTab.equipData[slotIndex].UnEquipAction();
        skillTab.allData[skillId].UnEquipAction();
    }
}
