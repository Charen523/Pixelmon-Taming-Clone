public class EnemyIdleState : IdleState
{
    EnemyStateMachine enemyStateMachine;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        enemyStateMachine = stateMachine;
    }
}