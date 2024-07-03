using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerStateMachine : StateMachine
{
    //public Player Player { get; }
    public string enemyTag = "Slime";

    public Vector2 MovementInput { get; set; }

   // public bool IsAttacking { get; set; }

    public Transform MainCameraTransform { get; set; } // 카메라가 플레이어 따라다님

    // States
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    private void Start()
    {
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this, null); // Todo: 탐색한 타겟 넘겨주기
        moveState.OnTargetReached += ChangeAttackState;
        ChangeState(idleState);
    }

    // 공격 상태로 변경(플레이어는 idle 상태)
    private void ChangeAttackState()
    {
        ChangeState(idleState);
    }
}