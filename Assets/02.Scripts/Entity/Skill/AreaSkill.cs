using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSkill : BaseSkill
{
    protected override void ExecuteSkill()
    {
        base.ExecuteSkill();
        gameObject.transform.position = target.transform.position;
    }
}
