
using System.Collections;

public class EnemyDieState : DieState
{
    private new EnemyFSM fsm;

    public EnemyDieState(EnemyFSM fsm) 
        : base(fsm)
    {
        this.fsm = fsm;
    }
}