
using UnityEngine;

public class EnemyAttackState : AttackState
{
    private new EnemyFSM fsm;

    public EnemyAttackState(EnemyFSM fsm)
        : base(fsm)
    {
        this.fsm = fsm;
    }

    public override void Enter()
    {
        fsm.enemyCollision.SetActive(false);
        base.Enter();
    }

    public override void Execute()
    {
        if (!IsAttackRange())
            fsm.ChangeState(fsm.ChaseState);
    }

    public override void Exit()
    {
        fsm.enemyCollision.SetActive(true);
        base.Exit();
    }

    private bool IsAttackRange()
    {
        Vector2 currentPosition = fsm.rb.position;
        Vector2 targetPosition = Player.Instance.fsm.rb.position;

        if (Vector2.Distance(currentPosition, targetPosition) > fsm.enemy.statHandler.data.atkRange)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}