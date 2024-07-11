using System;
using System.Collections;
using UnityEngine;

public class PlayerFSM : FSM
{
    public string EnemyTag = "Enemy";

    #region Player States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerFailState FailState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDieState DieState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDetectState DetectState { get; private set; }
    #endregion

    #region Player Input
    public Vector2 MovementInput { get; set; }
    public FloatingJoystick joystick;
    private Coroutine activeMovementCoroutine;
    public bool isActiveMove; //능동 움직임 플래그.
    #endregion

    #region Player Detect
    public float initialDetectionRadius = 4; // 초기 탐지 반경 설정
    public float maxDetectionRadius = 10; // 최대 탐지 반경 설정
    public float radiusIncrement = 2; // 탐지 반경 증가 값
    #endregion

    public void Init()
    {
        IdleState = new PlayerIdleState(this);
        FailState = new PlayerFailState(this);
        MoveState = new PlayerMoveState(this);
        DetectState = new PlayerDetectState(this);
        DieState = new PlayerDieState(this);
        AttackState = new PlayerAttackState(this);

        joystick.OnJoystickVisible += JoystickChecker;

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