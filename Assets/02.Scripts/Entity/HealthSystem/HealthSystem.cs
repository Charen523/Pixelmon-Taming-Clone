using Sirenix.OdinInspector;
using System;
using UnityEngine.UI;
using UnityEngine;

public abstract class HealthSystem : SerializedMonoBehaviour
{
    public Image hpBar;

    /*체력변수*/
    protected float maxHealth;
    protected float currentHealth;

    private void Update()
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
        currentHealth = MathF.Max(0, currentHealth - delta);
        PoolManager.Instance.SpawnFromPool<DamageText>("TXT00001").ShowDamageText((int)delta, gameObject.transform.position);        
        if (currentHealth == 0)
        {
            NoticeDead();
        } 
    }

    //사망 이벤트
    protected abstract void NoticeDead();
}