using TMPro;
using UnityEngine.UI;

public class BossHealthSystem : EnemyHealthSystem
{
    private Slider bossHpBar;
    private TextMeshProUGUI bossHpTxt;

    private void Start()
    {
        bossHpBar = StageManager.Instance.GetBossSlider();
        bossHpTxt = StageManager.Instance.GetBossHpText();
    }

    protected override void Update()
    {
        bossHpBar.value = currentHealth / maxHealth;
        bossHpTxt.text = (bossHpBar.value * 100).ToString("F2") + "%";
    }
}