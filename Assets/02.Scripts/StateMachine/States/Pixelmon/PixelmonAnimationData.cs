using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelmonAnimationData
{
    [SerializeField]
    private string skillParameterName = "Skill";

    public int SkillParameterHash { get; private set;}

    public void Initialize()
    {
        SkillParameterHash = Animator.StringToHash(skillParameterName);
    }
}
