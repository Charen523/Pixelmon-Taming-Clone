
using UnityEngine;

public class EnemyChaseState : MoveState
{
    EnemyStateMachine enemyStateMachine;

    public EnemyChaseState(EnemyStateMachine stateMachine) 
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Execute()
    {
        targetTransform = Player.Instance.transform;
        isFlipEnemy = true;
        base.Execute();
    }
}