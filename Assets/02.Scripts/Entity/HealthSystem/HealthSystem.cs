using Sirenix.OdinInspector;

public abstract class HealthSystem : SerializedMonoBehaviour
{
    /*체력변수*/
    public float MaxHealth { get; protected set; }
    protected float currentHealth;

    //체력변화 추상클래스
    public abstract bool ChangeHealth(float damage);

    //사망 이벤트
    protected abstract void NoticeDead();
}