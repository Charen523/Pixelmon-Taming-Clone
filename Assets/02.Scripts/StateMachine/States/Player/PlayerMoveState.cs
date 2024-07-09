using UnityEngine;

public class PlayerMoveState : MoveState
{
    private PlayerStateMachine playerStateMachine;
    public PlayerMoveState(PlayerStateMachine stateMachine, Transform target)
        : base(stateMachine, target)
    {
        playerStateMachine = stateMachine;
    }

    public override void Execute()
    {
        Flip();
    }

    public override void FixedExecute()
    {
        // 플레이어 입력에 따라 이동

        // 입력이 없을 경우 타겟을 향해 이동
        base.FixedExecute();
    }
}
