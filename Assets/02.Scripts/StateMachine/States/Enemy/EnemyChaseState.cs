
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
        if (targetTransform.position.x - stateMachine.transform.position.x > 0)
        {
            stateMachine.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else
        {
            stateMachine.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }

        base.Execute();
    }
}