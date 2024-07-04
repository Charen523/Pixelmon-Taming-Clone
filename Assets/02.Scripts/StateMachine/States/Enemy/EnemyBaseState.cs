
using UnityEngine;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine fsm;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.fsm = stateMachine;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void Execute()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movementDirection = GetMovementDirection();
        Move(movementDirection);
        //플립으로 회전 구현 필요.
    }

    private Vector2 GetMovementDirection()
    {
        //enemy -> target 방향.
        Vector2 dir = (fsm.target.transform.position - fsm.Enemy.transform.position).normalized;
        return dir;
    }

    private void Move(Vector2 direction)
    {
        float magnitude = fsm.moveSpeed * fsm.movementSpeedModifier;
        fsm.Enemy.rb.velocity = direction * magnitude * Time.deltaTime;
    }

    protected bool IsDead()
    {
        //애니메이션 GetBool로 죽음 상태 가져오기??
        return false;
    }

    protected bool IsAttackRange()
    {
        float playerDistanceSqr = (fsm.target.transform.position - fsm.Enemy.transform.position).sqrMagnitude;
        return playerDistanceSqr <= fsm.Enemy.data.atkRange * fsm.Enemy.data.atkRange;
    }
}