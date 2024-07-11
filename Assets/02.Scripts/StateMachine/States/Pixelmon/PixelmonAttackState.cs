public class PixelmonAttackState : AttackState
{
    private new PixelmonFSM fsm;
    public PixelmonAttackState(PixelmonFSM fsm) 
        : base(fsm)
    {
        this.fsm = fsm;
    }

    public override void Execute()
    {
        //몹이 null이 아니라면 공격 및 스킬
        if (fsm.target != null)
        {
            fsm.target.GetComponent<EnemyHealthSystem>().TakeDamage(fsm.pixelmon.data.atk);
        }
    }
}
