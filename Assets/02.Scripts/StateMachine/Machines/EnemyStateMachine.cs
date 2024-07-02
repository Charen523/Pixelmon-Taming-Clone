
using UnityEngine;

public class EnemyStateMachine : DeathableStateMachine
{
    public Enemy Enemy { get; }
    public GameObject target;  

    #region Enemy States
    public EnemyChaseState ChaseState {  get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyDieState DieState { get; private set; }
    #endregion

    #region Movement Status
    public Vector2 moveInput;
    public float moveSpeed;
    public float movementSpeedModifier;
    public float rotationDamping;
    #endregion

    public EnemyStateMachine (Enemy enemy)
    {
        this.Enemy = enemy;
        target = enemy.PlayerObject;

        /*states 인스턴스 생성*/
        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        DieState = new EnemyDieState(this);

        moveSpeed = Enemy.data.baseSpeed;
        rotationDamping = Enemy.data.baseRotationDamping;
    }
}