public class PlayerHealthSystem : HealthSystem
{
    private void Start()
    {
        InitHealth();
    }

    public void InitHealth()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    protected override void NoticeDead()
    {
        Player.Instance.stateMachine.ChangeState(Player.Instance.stateMachine.DieState);
    }
}