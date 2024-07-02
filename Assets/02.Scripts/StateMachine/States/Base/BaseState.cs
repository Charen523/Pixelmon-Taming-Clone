using UnityEngine;

public abstract class BaseState : IState
{
    protected StateMachine stateMachine;
    protected Animator animator;
    protected AnimationData animationData;

    public BaseState(StateMachine stateMachine, Animator animator, AnimationData animationData)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.animationData = animationData;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();

    protected void StartAnimation(int animationHash)
    {
        animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        animator.SetBool(animationHash, false);
    }
}