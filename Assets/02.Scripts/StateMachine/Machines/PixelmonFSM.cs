
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
        minDistance = Player.Instance.data.baseAtkRange;
        ChangeState(IdleState);
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (coolTime == 0)
            {
                var enemies = Search(1);
                if (enemies.Count == 0) Player.Instance.fsm.ChangeState(Player.Instance.fsm.DetectState);
                for (int i = 0; i < enemies.Count; i++)
                {
                    Vector2 direction = enemies[i].transform.position - transform.position;
                    float damage = pixelmon.data.baseDmg;
                    GameObject projectile = PoolManager.Instance.SpawnFromPool("ATV00000").gameObject;
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
        if (isAttack && attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(Attack());
        }
        else if (!isAttack && attackCoroutine != null)
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