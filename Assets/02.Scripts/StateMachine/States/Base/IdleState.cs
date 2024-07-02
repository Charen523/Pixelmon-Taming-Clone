using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine stateMachine, Animator animator, AnimationData animationData)
        : base(stateMachine, animator, animationData)
    {
    }

    public override void Enter()
    {
        StartAnimation(animationData.IdleParameterHash);
    }

    public override void Execute()
    {
        // Å½»ö
    }

    public override void Exit()
    {
        StopAnimation(animationData.IdleParameterHash);
    }
}
