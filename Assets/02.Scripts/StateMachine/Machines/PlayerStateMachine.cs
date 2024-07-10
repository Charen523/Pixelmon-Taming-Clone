using System;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; private set; }
    public string EnemyTag = "Enemy";

    public Vector2 MovementInput { get; set; }

    public Action stageStart, stageClear, stageTimeOut, targetReached;

    #region Player States
    public IdleState IdleState { get; private set; }
    public FailState FailState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDetectState DetectState;

    #endregion

    private void Start()
    {
        Player = Player.Instance;
        IdleState = new PlayerIdleState(this);
        DetectState = new PlayerDetectState(this);
        MoveState = new PlayerMoveState(this);
        FailState = new PlayerFailState(this);

        GameManager.Instance.OnStageStart += stageStart = () => ChangeState(DetectState);
        GameManager.Instance.OnStageClear += stageClear = () => ChangeState(IdleState);
        GameManager.Instance.OnStageTimeOut += stageTimeOut = () => ChangeState(FailState);
        MoveState.OnTargetReached += targetReached = () => ChangeState(IdleState);

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