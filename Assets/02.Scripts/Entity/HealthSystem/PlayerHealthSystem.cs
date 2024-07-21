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

    protected override void NoticeDead()
    {
        Player.Instance.fsm.ChangeState(Player.Instance.fsm.DieState);
    }
}