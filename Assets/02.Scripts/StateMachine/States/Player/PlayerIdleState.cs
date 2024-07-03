using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerIdleState : IdleState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) 
        : base(stateMachine)
    {
    }

    public override void Execute()
    {      
        GameObject closestTarget = FindClosestTarget((stateMachine as PlayerStateMachine).EnemyTag);
        if (closestTarget != null)
        {
            (stateMachine as PlayerStateMachine).moveState.targetTransform = closestTarget.transform;
            stateMachine.ChangeState((stateMachine as PlayerStateMachine).moveState);
        }
    }

    // 가장 가까운 적 탐색
    private GameObject FindClosestTarget(string enemyTag)
    {
        GameObject[] enemies = PoolManager.Instance.GetActiveObjectsFromPool(enemyTag);
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 playerPosition = (stateMachine as PlayerStateMachine).transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}