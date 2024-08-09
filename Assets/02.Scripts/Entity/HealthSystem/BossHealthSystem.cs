using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthSystem : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Slider bossHpBar;
    [SerializeField] private TextMeshProUGUI bossHpTxt;

    [SerializeField] private float currentHealth => enemy.healthSystem.currentHealth;
    [SerializeField] private float maxHealth => enemy.healthSystem.maxHealth;

    Coroutine bossCoroutine;

    public void InvokeBossHp()
    {
        bossHpBar = StageManager.Instance.GetBossSlider();
        bossHpTxt = StageManager.Instance.GetBossHpText();
        bossCoroutine = StartCoroutine(bossHealthSlider());
    }

    private IEnumerator bossHealthSlider()
    {
        while(true)
        {
            bossHpBar.value = currentHealth / maxHealth;
            bossHpTxt.text = ((int)(bossHpBar.value * 100)).ToString() + "%";

            if (currentHealth <= 0)
                StopCoroutine(bossCoroutine);

            yield return null;
        }
    }
}