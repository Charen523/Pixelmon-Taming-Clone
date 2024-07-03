using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    protected Transform targetTransform;

    public event Action OnTargetReached;

    public MoveState(StateMachine stateMachine, Transform target) 
        : base(stateMachine)
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
                OnTargetReached?.Invoke();
            }
        }
    }
}
