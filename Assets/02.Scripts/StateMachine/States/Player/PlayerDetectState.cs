using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class PlayerDetectState : IdleState
{
    private PlayerStateMachine playerStateMachine;
    public float initialDetectionRadius = 4; // 초기 탐지 반경 설정
    public float maxDetectionRadius = 10; // 최대 탐지 반경 설정
    public float radiusIncrement = 2; // 탐지 반경 증가 값
    [HideInInspector] public float currentDetectionRadius = 4;
    [HideInInspector] public GameObject closestTarget = null;

    private WaitForSeconds detectionInterval = new WaitForSeconds(0.5f);

    public PlayerDetectState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        playerStateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.Instance.OnStageStart -= playerStateMachine.stageStart;
    }

    public override void Exit()
    {
        base.Exit();
        GameManager.Instance.OnStageStart += playerStateMachine.stageStart;
    }

    public override void Execute()
    {
        base.Execute();
        playerStateMachine.StartCoroutine(DetectClosestTargetCoroutine());
    }

    private IEnumerator DetectClosestTargetCoroutine()
    {
        currentDetectionRadius = initialDetectionRadius;      

        while (closestTarget == null && currentDetectionRadius <= maxDetectionRadius)
        {
            closestTarget = FindClosestTarget(playerStateMachine.EnemyTag, currentDetectionRadius);
            if (closestTarget != null)
            {
                playerStateMachine.MoveState.targetTransform = closestTarget.transform;

                stateMachine.ChangeState(playerStateMachine.MoveState);
                Player.Instance.NotifyPlayerMove();

                yield break;
            }
            currentDetectionRadius += radiusIncrement;
            yield return detectionInterval;
        }

        // 최대 탐지 반경까지 찾지 못한 경우, currentDetectionRadius를 초기화
        currentDetectionRadius = initialDetectionRadius;
    }

    // 범위 탐색
    private GameObject FindClosestTarget(string enemyTag, float detectionRadius)
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector2 playerPosition = playerStateMachine.transform.position;

        // 탐지 반경 내 모든 오브젝트 찾기
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerPosition, detectionRadius);

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