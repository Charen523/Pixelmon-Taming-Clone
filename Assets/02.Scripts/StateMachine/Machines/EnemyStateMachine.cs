
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Enemy Enemy { get; }
    public GameObject target;
    
    #region Enemy States
    public EnemyChaseState ChaseState {  get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyDieState DieState { get; private set; }
    #endregion

    #region Movement Status
    public float moveSpeed;
    public float movementSpeedModifier; //1 또는 0으로 속도 존재 여부 알려주기.
    public float rotationDamping; //없어질 수도 있음(flip으로 대체.)
    #endregion

    public EnemyStateMachine (Enemy enemy)
    {
        Enemy = enemy;
        target = enemy.PlayerObject;

        /*states 인스턴스 생성*/
        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        DieState = new EnemyDieState(this);

        moveSpeed = Enemy.data.baseSpeed;
        rotationDamping = Enemy.data.baseRotationDamping;
    }
}