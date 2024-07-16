using System;
using UnityEngine;

public class EnemyFSM : FSM
{
    public Enemy enemy;   

    #region Enemy States
    public IdleState IdleState { get; private set; }
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
    }
    public void Init()
    {
        IdleState = new IdleState(this);
        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        DieState = new EnemyDieState(this);

        ChangeState(ChaseState);
    }

    public void OnEnemyAttack()
    {
        target.GetComponent<PlayerHealthSystem>().TakeDamage(enemy.data.dmg);
    }

    public void OnEnemyDead()
    {
        StageManager.Instance.OnMonsterDead(enemy.data, gameObject);
        gameObject.SetActive(false);
        target.GetComponent<PlayerFSM>().target = null;
        Player.Instance.fsm.ChangeState(Player.Instance.fsm.DetectState);
    }
}