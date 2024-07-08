
using System.Collections;

public class EnemyDieState : DieState
{
    EnemyStateMachine enemyStateMachine;

    public EnemyDieState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        //stage에게 사망소식 알리기.
    }

    public override void Exit()
    {
        base.Exit();
        //poolManager에서 setactive false 하기.
    }
}