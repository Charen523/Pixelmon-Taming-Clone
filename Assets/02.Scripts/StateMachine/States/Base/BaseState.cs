using UnityEngine;

public abstract class BaseState : IState
{
    protected StateMachine stateMachine;
    protected Animator anim;
    protected AnimationData animationData;

    public BaseState(StateMachine stateMachine, Animator anim, AnimationData animationData)
    {
        this.stateMachine = stateMachine;
        this.anim = anim;
        this.animationData = animationData;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();

    protected void StartAnimation(int animationHash)
    {
        anim.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        anim.SetBool(animationHash, false);
    }
}