using System.Collections;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DgMonster : MonoBehaviour
{
    public UIDungeonProgress dgProgress;

    private int dgIndex;
    public int dgLv;

    private BigInteger maxHealth 
        => Calculater.CalPrice(dgLv, baseHealth, d1Health, d2Health);
    private BigInteger currentHealth;
    private int baseHealth = 10000;
    private int d1Health = 5000;
    private int d2Health = 2000;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpTxt;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PixelmonProjectile"))
        {
            ProjectileController projectile = collision.gameObject.GetComponent<ProjectileController>();
            TakeDamage(projectile.projectileDamage);
            projectile.ResetProjectile();
        }
    }

    private async void Awake()
    {
        dgProgress = await UIManager.Show<UIDungeonProgress>();
    }

    private void OnDestroy()
    {
        SaveCurLv();
        UIManager.Hide<UIDungeonProgress>();
        //보상 주기!
        Destroy(gameObject);
    }

    public void InitDgMonster(int index)
    {
        dgIndex = index;
        dgLv = 1;

        hpSlider = StageManager.Instance.GetBossSlider();
        hpTxt = StageManager.Instance.GetBossHpText();
        StartCoroutine(bossHealthSlider());
    }

    private void TakeDamage(float delta)
    {
        BigInteger damage = (BigInteger)Mathf.Max(0, delta);
        currentHealth = currentHealth - damage;
        while (currentHealth < 0)
        {
            BigInteger temp = currentHealth;
            currentHealth = maxHealth;
            currentHealth += temp;
            dgLv++;
        }

        PoolManager.Instance.SpawnFromPool<DamageText>("TXT00001").ShowDamageText((int)damage, gameObject.transform.position);
    }

    private void SaveCurLv()
    {
        int[] dgLvs = SaveManager.Instance.userData.bestDgLvs;
        dgLvs[dgIndex] = dgLv;
        SaveManager.Instance.SetFieldData(nameof(SaveManager.Instance.userData.bestDgLvs), dgLvs);
    }

    private IEnumerator bossHealthSlider()
    {
        while (true)
        {
            BigInteger hpValue = currentHealth * 10000 / maxHealth;
            hpSlider.value = (float)hpValue / 10000;
            hpTxt.text = ((int)(hpSlider.value * 100)).ToString() + "%";

            if (currentHealth <= 0)
            {
                hpSlider.value = 1;
                hpTxt.text = ((int)(hpSlider.value * 100)).ToString() + "%";
            }

            yield return null;
        }
    }
}