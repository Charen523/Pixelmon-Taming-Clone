using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy : SerializedMonoBehaviour
{
    public EnemyData data;
    public EnemyFSM fsm;
    public EnemyHealthSystem healthSystem;

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

        //임시
        data.dmg = data.atk;
        fsm.Init();
    }
    private void Update()
    {
        fsm.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PixelmonProjectile"))
        {
            ProjectileController projectile = collision.gameObject.GetComponent<ProjectileController>();
            healthSystem.TakeDamage(projectile.projectileDamage);
            collision.gameObject.SetActive(false);
        }
    }
}