public class PlayerHealthSystem : HealthSystem
{
    private void Start()
    {
        InitHealth();
    }

    public void InitHealth()
    {
        maxHealth = Player.Instance.data.baseMaxHp;
        currentHealth = maxHealth;
    }

    public override void TakeDamage(float delta)
    {
        def = Player.Instance.data.baseDef;
        base.TakeDamage(delta);
    }

    protected override void NoticeDead()
    {
        Player.Instance.fsm.ChangeState(Player.Instance.fsm.DieState);
    }
}