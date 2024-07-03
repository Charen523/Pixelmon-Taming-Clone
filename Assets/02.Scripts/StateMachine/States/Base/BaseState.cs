using UnityEngine;

public abstract class BaseState : IState
{
    protected StateMachine stateMachine;

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();

    public BaseState(StateMachine stateMachine)
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