using System;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; private set; }
    public PixelmonManager PixelmonManager;
    public string EnemyTag = "Enemy";

    public Vector2 MovementInput { get; set; }

    #region Player States
    public IdleState IdleState { get; private set; }
    public FailState FailState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDetectState DetectState;
    #endregion

    private void Start()
    {
        IdleState = new IdleState(this);
        DetectState = new PlayerDetectState(this);
        MoveState = new PlayerMoveState(this, null);
        FailState = new FailState(this);

        GameManager.Instance.OnStageStart += () => ChangeState(DetectState);
        GameManager.Instance.OnStageTimeOut += () => ChangeState(FailState);
        MoveState.OnTargetReached += () => ChangeState(IdleState);

        ChangeState(DetectState);
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