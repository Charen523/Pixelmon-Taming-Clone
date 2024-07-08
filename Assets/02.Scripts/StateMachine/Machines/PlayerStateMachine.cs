using System;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerData data;
    public string EnemyTag = "Enemy";

    public Vector2 MovementInput { get; set; }

    // States
    public IdleState IdleState { get; private set; }
    public PlayerDetectState DetectState;
    public PlayerMoveState MoveState { get; private set; }

    private void Start()
    {
        IdleState = new IdleState(this);
        DetectState = new PlayerDetectState(this);
        MoveState = new PlayerMoveState(this, null);

        GameManager.Instance.OnStageStart += () => ChangeState(DetectState);
        GameManager.Instance.OnStageTimeOut += () => ChangeState(IdleState);
        MoveState.OnTargetReached += ChangeAttackState;

        ChangeState(DetectState);
    }

    // 공격 상태로 변경(플레이어는 idle 상태)
    private void ChangeAttackState()
    {
        ChangeState(IdleState);
    }

    // Gizmos를 사용하여 탐지 반경을 시각적으로 표시
    private void OnDrawGizmos()
    {
        if (DetectState != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, DetectState.currentDetectionRadius);
        }
    }
}