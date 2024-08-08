public class PlayerHealthSystem : HealthSystem
{ 
    public void InitHealth(float hp)
    {
        maxHealth = hp;
        currentHealth = maxHealth;
    }

    public override void TakeDamage(float delta)
    {
        def = Player.Instance.statHandler.def;
        base.TakeDamage(delta);
    }

    protected override void NoticeDead()
    {
        Player.Instance.fsm.ChangeState(Player.Instance.fsm.DieState);
    }
}