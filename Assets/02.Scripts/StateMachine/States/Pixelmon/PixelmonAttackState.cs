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
        
    }
}
