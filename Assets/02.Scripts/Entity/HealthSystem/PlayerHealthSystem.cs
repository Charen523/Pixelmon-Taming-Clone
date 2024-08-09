public class PlayerHealthSystem : HealthSystem
{ 
    public void InitHealth(float hp)
    {
        maxHealth = hp;
        currentHealth = maxHealth;
    }

    public override void TakeDamage(float delta, bool isCri = false, bool isPlayer = true)
    {
        def = Player.Instance.statHandler.def;
        base.TakeDamage(delta, isCri, isPlayer);
    }

    protected override void NoticeDead()
    {
        Player.Instance.fsm.ChangeState(Player.Instance.fsm.DieState);
    }
}