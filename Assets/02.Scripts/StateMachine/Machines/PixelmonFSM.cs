
using System;
using System.Collections;
using UnityEngine;

public class PixelmonFSM : FSM
{
    public Pixelmon pixelmon;

    #region Pixelmon States
    public IdleState IdleState { get; private set; }
    public PixelmonMoveState MoveState { get; private set; }
    public PixelmonAttackState AttackState { get; private set; }
    #endregion

    Coroutine attackCoroutine;
    WaitForSeconds attackSpeed;

    private void Start()
    {
        attackSpeed = new WaitForSeconds(1f);
        //attackSpeed = new WaitForSeconds(pixelmon.data.atkSpd);
    }

    public void InitStates()
    {
        IdleState = new IdleState(this);
        MoveState = new PixelmonMoveState(this);
        AttackState = new PixelmonAttackState(this);

        ChangeState(IdleState);
    }

    private IEnumerator Attack()
    {
        //몹이 null이 아니라면 공격 및 스킬
        if (target != null)
        {
            target.GetComponent<EnemyHealthSystem>().TakeDamage(pixelmon.data.atk);
            yield return attackSpeed;
        }
    }

    public void InvokeAttack(bool isAttack)
    {
        if (isAttack)
        {
            attackCoroutine = StartCoroutine(Attack());
        }else if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }
}