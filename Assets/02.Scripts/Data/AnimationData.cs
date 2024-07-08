using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationData
{
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string moveParameterName = "Move";
    [SerializeField] private string attackParameterName = "Attack";
    [SerializeField] private string dieParameterName = "Die";
    [SerializeField] private string hitParameterName = "Hit";
    [SerializeField] private string failParameterName = "Fail";

    public int IdleParameterHash { get; private set; }
    public int MoveParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int DieParameterName { get; private set; }
    public int HitParameterName { get; private set; }
    public int FailParameterName { get; private set; }

    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        MoveParameterHash = Animator.StringToHash(moveParameterName);

        AttackParameterHash = Animator.StringToHash(attackParameterName);
        DieParameterName = Animator.StringToHash(dieParameterName);
        HitParameterName = Animator.StringToHash(hitParameterName);
        FailParameterName = Animator.StringToHash(failParameterName);
    }
}
