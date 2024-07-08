using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public EnemyData data;
    public GameObject target; //어케 찾아올까유~ 플레이어 싱글톤?
    
    #region Enemy States
    public EnemyIdleState IdleState { get; private set; }
    public EnemyChaseState ChaseState {  get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyDieState DieState { get; private set; }
    #endregion

    private void OnEnable()
    {
        ChangeState(ChaseState);
    }

    private void Start()
    {
        target = Player.Instance.gameObject;

        //MovementSpeed = data.spd;
        //AttackRange = data.atkRange;

        MovementSpeed = 2f;
        AttackRange = 2f;

        ChaseState = new EnemyChaseState(this, target.transform);
        AttackState = new EnemyAttackState(this);
        DieState = new EnemyDieState(this);

        GameManager.Instance.OnPlayerDie += () => ChangeState(IdleState);
        GameManager.Instance.OnStageTimeOut += () => ChangeState(IdleState);
        ChaseState.OnTargetReached += () => ChangeState(AttackState);

        ChangeState(ChaseState);
    }
}