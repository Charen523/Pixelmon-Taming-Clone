using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy : SerializedMonoBehaviour
{
    public EnemyData data;
    public EnemyStateMachine stateMachine;
    public EnemyHealthSystem healthSystem;

    private void Start()
    {
        data = DataManager.Instance.GetData<EnemyData>("ENY00101");
        
        if (stateMachine == null)
        {
            GetComponent<EnemyStateMachine>();

            if (stateMachine == null)
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PixelmonProjectile"))
        {
            //대충 발사체에 등록된 데미지만큼 내 HP 깎아주기.
            //healthSystem.TakeDamage(발사제 데미지);
        }
    }
}