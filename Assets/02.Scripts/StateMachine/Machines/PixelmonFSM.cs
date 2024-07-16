
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PixelmonFSM : FSM
{
    public Pixelmon pixelmon;
    public string EnemyTag = "Enemy";

    #region Pixelmon States
    public IdleState IdleState { get; private set; }
    public PixelmonMoveState MoveState { get; private set; }
    public PixelmonAttackState AttackState { get; private set; }
    #endregion

    Coroutine attackCoroutine;
    float attackSpeed = 1;
    WaitUntil waitAttack;
    float coolTime = 0;
    float minDistance;
    private void Start()
    {
        waitAttack = new WaitUntil(() => AttackCoolTime());
        //attackSpeed = pixelmon.data.atkSpd;
    }

    public void InitStates()
    {
        IdleState = new IdleState(this);
        MoveState = new PixelmonMoveState(this);
        AttackState = new PixelmonAttackState(this);
        minDistance = Player.Instance.data.atkRange;
        ChangeState(IdleState);
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            //몹이 null이 아니라면 공격 및 스킬
            //기다리는 시간과 날아가는 시간에 대한 보정 필요!!!!!
            if (coolTime == 0)
            {
                var enemies = Search(1);
                for (int i = 0; i < enemies.Count; i++)
                {
                    Vector2 direction = enemies[i].transform.position - transform.position;
                    float damage = pixelmon.data.atkDmg;
                    GameObject projectile = PoolManager.Instance.SpawnFromPool("ATV00000");
                    projectile.GetComponent<ProjectileController>().GetAttackSign(transform.position, direction, damage, minDistance, 10);
                }
            }
            yield return waitAttack;
        }
    }

    private bool AttackCoolTime()
    {
        coolTime += Time.deltaTime;
        if (coolTime >= attackSpeed)
        {
            coolTime = 0;
            return true;
        }
        return false;
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

    public List<Collider2D> Search(int count)
    {
        var hitColliders = Physics2D.OverlapCircleAll(Player.Instance.transform.position, minDistance).ToList();
        hitColliders.RemoveAll(obj => !obj.gameObject.CompareTag(EnemyTag));
        hitColliders.Sort((a, b) =>
        {
            var aDist = (transform.position - a.gameObject.transform.position).sqrMagnitude;
            var bDist = (transform.position - b.gameObject.transform.position).sqrMagnitude;
            return aDist.CompareTo(bDist);
        });
        count = Mathf.Min(count, hitColliders.Count);
        hitColliders.RemoveRange(count, hitColliders.Count - count);
        return hitColliders;
    }
}