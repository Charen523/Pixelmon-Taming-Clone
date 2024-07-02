
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
        //�ø����� ȸ�� ���� �ʿ�.
    }

    private Vector2 GetMovementDirection()
    {
        //enemy -> target ����.
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
        //�ִϸ��̼� GetBool�� ���� ���� ��������??
        return false;
    }

    protected bool IsAttackRange()
    {
        float playerDistanceSqr = (fsm.target.transform.position - fsm.Enemy.transform.position).sqrMagnitude;
        return playerDistanceSqr <= fsm.Enemy.data.attackRange * fsm.Enemy.data.attackRange;
    }
}