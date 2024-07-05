using UnityEngine;

public class PlayerDetectState : IdleState
{
    PlayerStateMachine PlayerStateMachine;

    public PlayerDetectState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        PlayerStateMachine = stateMachine;
    }

    public override void Execute()
    {
        base.Execute();
        GameObject closestTarget = FindClosestTarget(PlayerStateMachine.EnemyTag);
        if (closestTarget != null)
        {
            PlayerStateMachine.MoveState.targetTransform = closestTarget.transform;
            stateMachine.ChangeState(PlayerStateMachine.MoveState);
        }
    }

    // 범위 탐색
    private GameObject FindClosestTarget(string enemyTag)
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector2 playerPosition = PlayerStateMachine.transform.position;

        // 탐지 반경 내 모든 오브젝트 찾기
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerPosition, Player.Instance.DetectionRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(enemyTag))
            {
                float distanceToEnemy = Vector2.Distance(playerPosition, hitCollider.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hitCollider.gameObject;
                }
            }
        }

        return closestEnemy;
    }
}
