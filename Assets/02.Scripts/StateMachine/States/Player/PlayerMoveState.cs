using UnityEngine;

public class PlayerMoveState : MoveState
{
    PlayerStateMachine PlayerStateMachine;
    public PlayerMoveState(PlayerStateMachine stateMachine, Transform target)
        : base(stateMachine, target)
    {
        PlayerStateMachine = stateMachine;
    }

    public override void Execute()
    {
        // 플레이어 입력에 따라 이동

        // 입력이 없을 경우 타겟을 향해 이동
        base.Execute();
    }
}
