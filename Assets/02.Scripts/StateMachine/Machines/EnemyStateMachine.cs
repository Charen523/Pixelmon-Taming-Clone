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

    #region Movement Status
    public float moveSpeed;
    public float movementSpeedModifier; //1 또는 0으로 속도 존재 여부 알려주기.
    #endregion

    private void Start()
    {
        MovementSpeed = data.spd;

        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        DieState = new EnemyDieState(this);
    }
}