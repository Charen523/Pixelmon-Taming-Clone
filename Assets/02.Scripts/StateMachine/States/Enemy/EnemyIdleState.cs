public class EnemyIdleState : IdleState
{
    EnemyStateMachine enemyStateMachine;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.Instance.OnPlayerDie -= enemyStateMachine.playerDie;
        GameManager.Instance.OnStageClear -= enemyStateMachine.stageClear;
        GameManager.Instance.OnStageTimeOut -= enemyStateMachine.stageTimeOut;
    }
}