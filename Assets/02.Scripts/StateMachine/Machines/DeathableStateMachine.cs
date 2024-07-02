
using UnityEngine;

public class DeathableStateMachine : BaseStateMachine
{
    [HideInInspector] public DieState dieState;
    [HideInInspector] public HitState hitState;

    protected override void Start()
    {
        base.Start();
        dieState = new DieState(this);
        hitState = new HitState(this);
    }
}