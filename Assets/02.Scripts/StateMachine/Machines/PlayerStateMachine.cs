using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    //public Player Player { get; }

    public Vector2 MovementInput { get; set; }

   // public bool IsAttacking { get; set; }

    public Transform MainCameraTransform { get; set; } // 카메라가 플레이어 따라다님

    // States
    private PlayerMoveState moveState;

    private void Start()
    {
        moveState = new PlayerMoveState(this, null); // Todo: 탐색한 타겟 넘겨주기
        moveState.OnTargetReached += ChangeAttackState;
        //ChangeState(idleState);
    }

    // 탐색 메서드 필요

    // 공격 상태로 변경
    private void ChangeAttackState()
    {
        
    }
}