using UnityEngine;

public abstract class BaseState : IState
{
    protected BaseStateMachine stateMachine;

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();

    public BaseState(BaseStateMachine stateMachine)
    { 
        this.stateMachine = stateMachine;
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.anim.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.anim.SetBool(animationHash, false);
    }
}