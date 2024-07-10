using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; private set; }
    public string EnemyTag = "Enemy";

    public Vector2 MovementInput { get; set; }

    public Action stageClear, stageTimeOut, targetReached;

    #region Player States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerFailState FailState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDieState DieState { get; private set; }
    public PlayerDetectState DetectState;
    #endregion

    public FloatingJoystick joystick;
    private Coroutine activeMovementCoroutine;
    public bool isActiveMove; //능동 움직임 플래그.

    private void Start()
    {
        Player = Player.Instance;
        IdleState = new PlayerIdleState(this);
        DetectState = new PlayerDetectState(this);
        MoveState = new PlayerMoveState(this);
        FailState = new PlayerFailState(this);
        DieState = new PlayerDieState(this);

        GameManager.Instance.OnStageClear += stageClear = () => ChangeState(IdleState);
        GameManager.Instance.OnStageTimeOut += stageTimeOut = () => ChangeState(FailState);
        MoveState.OnTargetReached += targetReached = () => ChangeState(IdleState);
        joystick.OnJoystickVisible += JoystickChecker;

        ChangeState(DetectState);
    }

    public void ReStartPlayer()
    {
        Player.healthSystem.InitHealth();
        Player.reStartStage?.Invoke();
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

    private void JoystickChecker(bool isActivated)
    {
        if (isActivated && currentState != DieState)
        {
            isActiveMove = true;
            ChangeState(MoveState);
            activeMovementCoroutine = StartCoroutine(ActivePlayerMove());
        }
        else
        {
            if (activeMovementCoroutine != null)
                StopCoroutine(activeMovementCoroutine);

            isActiveMove = false;
            rb.velocity = Vector2.zero;
            ChangeState(DetectState);
        }
    }

    private IEnumerator ActivePlayerMove()
    {
        while (isActiveMove)
        {
            MovementInput = joystick.Direction;
            yield return null;
        }
    }
}