using System;
using System.Collections;
using UnityEngine;

public class PlayerFSM : FSM
{
    public string EnemyTag = "Enemy";

    #region Player States
    public PlayerFailState FailState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDieState DieState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDetectState DetectState { get; private set; }
    #endregion

    #region Player Input
    public Vector2 MovementInput { get; set; }
    public FloatingJoystick joystick;
    public bool isActiveMove; //능동 움직임 플래그.
    #endregion

    #region Player Detect
    public float initialDetectionRadius = 4; // 초기 탐지 반경 설정
    public float maxDetectionRadius = 8; // 최대 탐지 반경 설정
    public float radiusIncrement = 2; // 탐지 반경 증가 값
    #endregion

    public void Init()
    {
        FailState = new PlayerFailState(this);
        MoveState = new PlayerMoveState(this);
        DetectState = new PlayerDetectState(this);
        DieState = new PlayerDieState(this);
        AttackState = new PlayerAttackState(this);

        GameManager.Instance.OnStageClear += ReStartPlayer;
        GameManager.Instance.OnStageTimeOut += StageTimeOut;

        ChangeState(DetectState);
    }

    public void ReStartPlayer()
    {
        ChangeState(DetectState);
        Player.Instance.statHandler.UpdateStats();
    }

    public void StageTimeOut()
    {
        ChangeState(FailState);
    }

    public void NotifyTimeOut()
    {
        ReStartPlayer();
        StageManager.Instance.InitStage();        
    }

    public void NotifyPlayerDie()
    {
        GameManager.Instance.NotifyStageStart();
        ReStartPlayer();
    }

    public void FadeEffect()
    {
        StageManager.Instance.fadeOut.gameObject.SetActive(true);
        StageManager.Instance.fadeOut.StartFade();
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