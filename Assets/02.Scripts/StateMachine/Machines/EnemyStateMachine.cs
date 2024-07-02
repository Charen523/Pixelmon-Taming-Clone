
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
    public float moveSpeed;
    public float movementSpeedModifier; //1 �Ǵ� 0���� �ӵ� ���� ���� �˷��ֱ�.
    public float rotationDamping; //������ ���� ����(flip���� ��ü.)
    #endregion

    public EnemyStateMachine (Enemy enemy)
    {
        Enemy = enemy;
        target = enemy.PlayerObject;

        /*states �ν��Ͻ� ����*/
        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        DieState = new EnemyDieState(this);

        moveSpeed = Enemy.data.baseSpeed;
        rotationDamping = Enemy.data.baseRotationDamping;
    }
}