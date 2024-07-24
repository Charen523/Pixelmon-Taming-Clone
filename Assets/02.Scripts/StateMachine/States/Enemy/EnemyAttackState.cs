
using UnityEngine;

public class EnemyAttackState : AttackState
{
    private new EnemyFSM fsm;

    public EnemyAttackState(EnemyFSM fsm)
        : base(fsm)
    {
        this.fsm = fsm;
    }

    public override void Execute()
    {
        if (!IsAttackRange())
            fsm.ChangeState(fsm.ChaseState);
    }

    private bool IsAttackRange()
    {
        Vector2 currentPosition = fsm.rb.position;
        Vector2 targetPosition = Player.Instance.HitPosition.transform.position;

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