
using UnityEngine;

public class EnemyChaseState : MoveState
{
    EnemyStateMachine enemyStateMachine;

    public EnemyChaseState(EnemyStateMachine stateMachine, Transform target) : base(stateMachine, target)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Execute()
    {
        base.Execute();
    }
}