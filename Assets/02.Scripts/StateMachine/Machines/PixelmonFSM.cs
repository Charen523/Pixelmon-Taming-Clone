
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
        while (true)
        {
            //몹이 null이 아니라면 공격 및 스킬
            //기다리는 시간과 날아가는 시간에 대한 보정 필요!!!!!
            if (target != null)
            {
                Vector2 direction = target.transform.position - transform.position;
                float damage = pixelmon.data.atkDmg;
                float bulletRange = 2f; //임의의 변수

                GameObject projectile = PoolManager.Instance.SpawnFromPool("ATV00000");
                projectile.GetComponent<ProjectileController>().GetAttackSign(transform.position, direction, damage, bulletRange, 1);
                
                yield return attackSpeed;
            }
        }
    }

    public void InvokeAttack(bool isAttack)
    {
        if (isAttack)
        {
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(Attack());
            }
        }
        else if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }
}