using UnityEngine;

public class EnemyDieState : DieState
{
    private new EnemyFSM fsm;

    public EnemyDieState(EnemyFSM fsm) 
        : base(fsm)
    {
        this.fsm = fsm;
    }

    public override void Enter()
    {
        fsm.rb.bodyType = RigidbodyType2D.Kinematic;
        base.Enter();
    }

    public override void Exit()
    {
        fsm.rb.bodyType = RigidbodyType2D.Dynamic;
        base.Exit();
    }
}