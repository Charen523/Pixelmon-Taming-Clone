
using UnityEngine;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine stateMachine;
    //PlayerGroundData �������� ����. Why? ��ġ?

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void HandleInput()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    public virtual void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movementDirection = GetMovementDirection();
        Rotate(movementDirection);
        Move(movementDirection);
    }

    private Vector2 GetMovementDirection()
    {
        //enemy -> target ����.
        Vector2 dir = (stateMachine.target.transform.position - stateMachine.Enemy.transform.position).normalized;
        return dir;
    }

    private void Move(Vector2 direction)
    {
        float magnitude = stateMachine.moveSpeed * stateMachine.movementSpeedModifier;
        stateMachine.Enemy.rb.velocity = direction * magnitude * Time.deltaTime;
    }

    private void Rotate(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            stateMachine.Enemy.transform.rotation = Quaternion.Lerp(stateMachine.Enemy.transform.rotation, targetRotation, stateMachine.rotationDamping * Time.deltaTime);
        }
    }
    protected bool IsDead()
    {
        //�ִϸ��̼� GetBool�� ���� ���� ��������??
        return false;
    }

    protected bool IsAttackRange()
    {
        float playerDistanceSqr = (stateMachine.target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Enemy.data.attackRange * stateMachine.Enemy.data.attackRange;
    }
}