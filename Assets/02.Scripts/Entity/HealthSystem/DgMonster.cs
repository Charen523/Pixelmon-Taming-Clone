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
    private int baseHealth = 1000;
    private int d1Health = 4000;
    private int d2Health = 5000;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpTxt;

    private int goldRwdBNum = 100000;
    private int goldRwdD1 = 300000;
    private int goldRwdD2 = 100000;

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
        PoolManager.Instance.SpawnFromPool<DamageText>("TXT00001").ShowDamageText((int)damage, gameObject.transform.position);
        currentHealth -= damage;

        while (damage > 0)
        {
            if (damage >= currentHealth)
            {
                damage -= currentHealth;
                dgLv++;
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth -= damage;
                damage = 0;
            }
        }

    }

    private void SaveCurLv()
    {
        int[] dgLvs = SaveManager.Instance.userData.bestDgLvs;
        dgLvs[dgIndex] = dgLv;
        SaveManager.Instance.SetFieldData(nameof(SaveManager.Instance.userData.bestDgLvs), dgLvs);
        
        BigInteger myReward = Calculater.CalPrice(dgLv - 1, goldRwdBNum, goldRwdD1, goldRwdD2);
        SaveManager.Instance.SetFieldData(nameof(SaveManager.Instance.userData.gold), myReward, true);
    }

    private IEnumerator bossHealthSlider()
    {
        float hpValue;
        while (true)
        {
            BigInteger ratio = currentHealth * 10000 / maxHealth;
            hpValue = (float)ratio / 10000;
            hpSlider.value = Mathf.Clamp01(hpValue);
            hpTxt.text = ((int)(hpSlider.value * 100)).ToString() + "%";

            if (currentHealth <= 0)
            {
                hpSlider.value = 1;
                hpTxt.text = "100%";
            }

            yield return null;
        }
    }

    public void DisableDgMonster()
    {
        StartCoroutine(KillDgMonster());
    }

    public IEnumerator KillDgMonster()
    {
        yield return new WaitForSeconds(1f);
        if (StageManager.Instance.isDungeonClear)
        {
            SaveCurLv();
            StageManager.Instance.isDungeonClear = false;
        }
        dgProgress.SetActive(false);
        Destroy(gameObject);
    }
}