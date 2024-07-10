using System;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [Header("EnemyStateMachine")]
    [SerializeField] private Enemy enemy;
    public GameObject target;

    public Action playerDie, stageClear, stageTimeOut, targetReached;

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
        if (enemy == null)
        {
            enemy = GetComponent<Enemy>();

            if (enemy == null)
            {
                Debug.LogError($"{gameObject.name} 객체에 Enemy 클래스 없음!");
            }
        }

        target = Player.Instance.gameObject;

        MovementSpeed = enemy.data.spd;

        IdleState = new EnemyIdleState(this);
        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        DieState = new EnemyDieState(this);

        GameManager.Instance.OnPlayerDie += playerDie = () => ChangeState(IdleState);
        GameManager.Instance.OnStageClear += stageClear = () => ChangeState(IdleState);
        GameManager.Instance.OnStageTimeOut += stageTimeOut = () => ChangeState(IdleState);
        ChaseState.OnTargetReached += targetReached = () => ChangeState(AttackState);

        ChangeState(ChaseState);
    }

    public void OnEnemyAttack()
    {
        target.GetComponent<PlayerHealthSystem>().TakeDamage(enemy.data.dmg);
    }

    public void OnEnemyDead()
    {
        //StageManager.Instance.OnMonsterDead(enemy.data.rcode, gameObject);
        gameObject.SetActive(false);
    }
}