
using UnityEngine;

public class EnemyStateMachine : DeathableStateMachine
{
    public Enemy enemy;
    public GameObject target; //플레이어.

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
        //target 초기화.
        
        /*states 인스턴스 생성*/

        /*movement status를 Enemy.Data로부터 받아오기*/
    }
}