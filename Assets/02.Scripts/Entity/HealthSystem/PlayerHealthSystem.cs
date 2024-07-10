public class PlayerHealthSystem : HealthSystem
{
    private void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    protected override void NoticeDead()
    {
       //여기다가 상태변경 느 
    }
}