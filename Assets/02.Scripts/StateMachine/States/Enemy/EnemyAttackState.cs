
using UnityEngine;

public class EnemyAttackState : AttackState
{
    EnemyStateMachine enemyStateMachine;

    public EnemyAttackState(EnemyStateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        enemyStateMachine.ChaseState.OnTargetReached -= enemyStateMachine.targetReached;
    }

    public override void Exit()
    {
        base.Exit();
        enemyStateMachine.ChaseState.OnTargetReached += enemyStateMachine.targetReached;
    }

    public override void Execute()
    {
        if (!IsAttackRange())
            enemyStateMachine.ChangeState(enemyStateMachine.ChaseState);

    }

    private void Attack()
    {
        enemyStateMachine.OnEnemyAttack();
    }

    private bool IsAttackRange()
    {
        Vector2 currentPosition = enemyStateMachine.rb.position;
        Vector2 targetPosition = Player.Instance.stateMachine.rb.position;

        if (Vector2.Distance(currentPosition, targetPosition) > enemyStateMachine.AttackRange)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}