using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    protected Transform targetTransform;

    public MoveState(BaseStateMachine stateMachine, Transform target) : base(stateMachine)
    {
        this.targetTransform = target;
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.animationData.MoveParameterHash);
    }

    public override void Execute()
    {
        MoveTowardsTarget();
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animationData.MoveParameterHash);
        stateMachine.rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// 플레이어는 Idle, 적과 펫은 Attack 상태로 변경바람.
    /// </summary>
    protected virtual void ChangeFightState()
    {
    }

    protected void MoveTowardsTarget()
    {
        // 자동으로 타겟에게 이동하는 로직
        if (stateMachine.rb != null && targetTransform != null)
        {
            Vector2 currentPosition = stateMachine.rb.position;
            Vector2 targetPosition = targetTransform.position;
            Vector2 direction = (targetPosition - currentPosition).normalized;
            stateMachine.rb.velocity = direction * stateMachine.MovementSpeed;

            // 타겟에게 도착하면 공격 상태로 변경
            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                ChangeFightState();
            }
        }
    }
}
