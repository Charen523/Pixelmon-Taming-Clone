using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : DeathableStateMachine
{
    //public Player Player { get; }

    public Vector2 MovementInput { get; set; }

   // public bool IsAttacking { get; set; }

    public Transform MainCameraTransform { get; set; } // 카메라가 플레이어 따라다님

    // States
    private PlayerMoveState moveState;

    protected override void Start()
    {
        base.Start();
        moveState = new PlayerMoveState(this, null); // Todo: 탐색한 타겟 넘겨주기
        ChangeState(moveState);
    }

    // 탐색 메서드 필요
}