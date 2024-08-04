using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthSystem : EnemyHealthSystem
{
    [SerializeField] private Slider bossHpBar;
    [SerializeField] private TextMeshProUGUI bossHpTxt;
    Coroutine bossCoroutine;

    public void InvokeBossHp()
    {
        bossHpBar = StageManager.Instance.GetBossSlider();
        bossHpTxt = StageManager.Instance.GetBossHpText();

        initEnemyHealth(enemy.statHandler.enemyMaxHp);
        bossCoroutine = StartCoroutine(bossHealthSlider());
    }

    protected override void Update()
    {
        //상위 클래스의 update문 없애기 위함.
    }

    private IEnumerator bossHealthSlider()
    {
        while(true)
        {
            bossHpBar.value = currentHealth / maxHealth;
            bossHpTxt.text = (bossHpBar.value * 100).ToString("F2") + "%";

            if (currentHealth <= 0)
                StopCoroutine(bossCoroutine);

            yield return null;
        }
    }
}