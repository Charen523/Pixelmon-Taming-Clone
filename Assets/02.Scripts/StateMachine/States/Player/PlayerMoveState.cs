using UnityEngine;

public class PlayerMoveState : MoveState
{
    private PlayerStateMachine playerStateMachine;
    private Transform enemyTarget;
    public PlayerMoveState(PlayerStateMachine stateMachine, Transform target)
        : base(stateMachine, target)
    {
        playerStateMachine = stateMachine;
        enemyTarget = target;
    }

    public override void Execute()
    {
        //if (enemyTarget.position.x - stateMachine.transform.position.x > 0)
        //{
        //    stateMachine.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        //}
        //else
        //{
        //    stateMachine.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        //}
        // 플레이어 입력에 따라 이동

        // 입력이 없을 경우 타겟을 향해 이동
        base.Execute();
    }
}
