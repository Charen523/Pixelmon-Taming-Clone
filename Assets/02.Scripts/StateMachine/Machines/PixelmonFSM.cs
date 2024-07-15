
using System;

public class PixelmonFSM : FSM
{
    public Pixelmon pixelmon;

    #region Pixelmon States
    public IdleState IdleState { get; private set; }
    public PixelmonMoveState MoveState { get; private set; }
    public PixelmonAttackState AttackState { get; private set; }
    #endregion

    public void InitStates()
    {
        IdleState = new IdleState(this);
        MoveState = new PixelmonMoveState(this);
        AttackState = new PixelmonAttackState(this);

        ChangeState(IdleState);
    }

    public void Attack()
    {
        //몹이 null이 아니라면 공격 및 스킬
        if (target != null)
        {
            target.GetComponent<EnemyHealthSystem>().TakeDamage(pixelmon.data.atk);
        }
    }
}