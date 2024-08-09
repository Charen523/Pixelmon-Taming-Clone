using Sirenix.OdinInspector;
using System;
using UnityEngine.UI;
using UnityEngine;

public abstract class HealthSystem : SerializedMonoBehaviour
{
    public Image hpBar;

    /*체력변수*/
    public float maxHealth = 1;
    public float currentHealth;
    protected float def;

    protected virtual void Update()
    {
        hpBar.fillAmount = currentHealth / maxHealth;
    }

    public virtual void GetHealed(float delta)
    {
        if (currentHealth != 0)
        {
            currentHealth = MathF.Min(maxHealth, currentHealth + delta);
        }
    }

    public virtual void TakeDamage(float delta, bool isCri = false, bool isPlayer = false)
    {
        float damage = Mathf.Max(0, delta - def);
        currentHealth = Mathf.Max(0, currentHealth - damage);
        PoolManager.Instance.SpawnFromPool<DamageText>("TXT00001").ShowDamageText((int)damage, gameObject.transform.position, isCri, isPlayer);        
        if (currentHealth == 0)
        {
            NoticeDead();
        } 
    }

    //사망 이벤트
    protected abstract void NoticeDead();
}