public class PlayerHealthSystem : HealthSystem
{
    private void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    protected override void NoticeDead()
    {
        Player.Instance.stateMachine.ChangeState(Player.Instance.stateMachine.DieState);
    }
}