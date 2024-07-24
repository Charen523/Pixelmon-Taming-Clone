
using System.Collections;

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
        fsm.enemyCollision.SetActive(false);
        base.Enter();
    }

    public override void Exit()
    {
        fsm.enemyCollision.SetActive(false);
        base.Exit();
    }
}