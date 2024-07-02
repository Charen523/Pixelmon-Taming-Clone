
using UnityEngine;

public class EnemyStateMachine : DeathableStateMachine
{
    public Enemy enemy;
    public GameObject target; //�÷��̾�.

    #region Enemy States
    public EnemyChaseState ChaseState {  get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyDieState DieState { get; private set; }
    #endregion

    /*movement status*/
    public Vector2 moveInput;
    public float moveSpeed;
    public float movementSpeedModifier;
    public float rotationDamping;

    public EnemyStateMachine (Enemy enemy)
    {
        this.enemy = enemy;
        //target �ʱ�ȭ.
        
        /*states �ν��Ͻ� ����*/

        /*movement status�� Enemy.Data�κ��� �޾ƿ���*/
    }
}