using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnlockPopup : UIBase
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI unlockMsg;

    [SerializeField] private float fadeDuration = 0.3f;

    public override void Opened(object[] param)
    {
        unlockMsg.text = param[0].ToString();
        StartCoroutine(HandlePopup());
    }

    private IEnumerator HandlePopup()
    {
        group.alpha = 1f;
        float elapsedTime = 0f;
        float startAlpha = group.alpha;
        yield return new WaitForSeconds(0.5f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        group.alpha = 0f;
        UIManager.Hide<UIUnlockPopup>();
    }
}

