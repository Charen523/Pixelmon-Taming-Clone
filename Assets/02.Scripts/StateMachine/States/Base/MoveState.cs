using System;
using Unity.VisualScripting;
using UnityEngine;

public class MoveState : BaseState
{
    public Transform targetTransform;
    public event Action OnTargetReached;

    public MoveState(StateMachine stateMachine, Transform target) 
        : base(stateMachine)
    {
        this.targetTransform = target;
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.animData.MoveParameterHash);
    }

    public override void Execute()
    {
    }

    public override void FixedExecute()
    {
        MoveTowardsTarget();
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animData.MoveParameterHash);
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
            if (Vector2.Distance(currentPosition, targetPosition) < stateMachine.AttackRange)
            {
                OnTargetReached?.Invoke();
            }
        }
    }
}
