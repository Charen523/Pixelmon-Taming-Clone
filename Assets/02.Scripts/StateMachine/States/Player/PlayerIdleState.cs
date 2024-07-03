using UnityEngine;

public class PlayerIdleState : IdleState
{
    PlayerStateMachine PlayerStateMachine;
    public PlayerIdleState(PlayerStateMachine stateMachine) 
        : base(stateMachine)
    {
        PlayerStateMachine = stateMachine;
    }

    public override void Execute()
    {      
        GameObject closestTarget = FindClosestTarget(PlayerStateMachine.EnemyTag);
        if (closestTarget != null)
        {
            PlayerStateMachine.moveState.targetTransform = closestTarget.transform;
            stateMachine.ChangeState(PlayerStateMachine.moveState);
        }
    }

    // 가장 가까운 적 탐색
    private GameObject FindClosestTarget(string enemyTag)
    {
        GameObject[] enemies = PoolManager.Instance.GetActiveObjectsFromPool(enemyTag);
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 playerPosition = PlayerStateMachine.transform.position;

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