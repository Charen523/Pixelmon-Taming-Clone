using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public EnemyData data;
    public GameObject target; //어케 찾아올까유~ 플레이어 싱글톤?
    
    #region Enemy States
    public EnemyChaseState ChaseState {  get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyDieState DieState { get; private set; }
    #endregion

    private void Start()
    {
        MovementSpeed = data.spd;
        AttackRange = data.atkRange;

        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        DieState = new EnemyDieState(this);
    }
}