using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy : SerializedMonoBehaviour
{
    public EnemyFSM fsm;
    public EnemyHealthSystem healthSystem;
    public BossHealthSystem bossHealthSystem;
    public EnemyStatHandler statHandler;
    public Collider2D enemyCollider;
    public SpriteRenderer myImg;

    public bool isFade;

    private void Start()
    {
        if (fsm == null)
        {
            GetComponent<EnemyFSM>();

            if (fsm == null)
            {
                Debug.LogError($"{gameObject.name} 객체에 stateMachine 부여되지 않음!");
            }
        }

        if (healthSystem == null )
        {
            GetComponent<EnemyHealthSystem>();

            if (healthSystem == null)
            {
                Debug.LogError($"{gameObject.name} 객체에 healthSystem이 부여되지 않음!");
            }
        }

        fsm.Init();
    }

    private void OnEnable()
    {
        myImg.color = Color.white;
        enemyCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (fsm.currentState == fsm.DieState || isFade)
        {
            enemyCollider.enabled = false;
            return;
        }

        if (collision.CompareTag("PixelmonProjectile"))
        {
            ProjectileController projectile = collision.gameObject.GetComponent<ProjectileController>();
            healthSystem.TakeDamage(projectile.projectileDamage);
            projectile.ResetProjectile();
        }
    }
}