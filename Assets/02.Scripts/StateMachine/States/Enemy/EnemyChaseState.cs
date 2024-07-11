
using UnityEngine;

public class EnemyChaseState : MoveState
{
    private new EnemyFSM fsm;

    public EnemyChaseState(EnemyFSM fsm) 
        : base(fsm)
    {
        this.fsm = fsm;
    }

    public override void Execute()
    {
        base.Execute();
        MoveTowardsTarget(fsm.enemy.data.spd, fsm.enemy.data.atkRange, fsm.AttackState);
    }
}