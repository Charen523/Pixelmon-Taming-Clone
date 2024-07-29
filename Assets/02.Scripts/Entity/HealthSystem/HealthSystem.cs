using Sirenix.OdinInspector;
using System;
using UnityEngine.UI;
using UnityEngine;

public abstract class HealthSystem : SerializedMonoBehaviour
{
    public Image hpBar;

    /*체력변수*/
    protected float maxHealth = 1;
    protected float currentHealth;
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

    public virtual void TakeDamage(float delta)
    {
        float damage = Mathf.Max(0, delta - def);
        currentHealth = MathF.Max(0, currentHealth - damage);
        PoolManager.Instance.SpawnFromPool<DamageText>("TXT00001").ShowDamageText((int)damage, gameObject.transform.position);        
        if (currentHealth == 0)
        {
            NoticeDead();
        } 
    }

    //사망 이벤트
    protected abstract void NoticeDead();
}