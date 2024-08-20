using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnlockPopup : UIBase
{
    [SerializeField] private RawImage rawImg;
    private Material material;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI unlockMsg;

    [SerializeField] private float fadeDuration = 1f;

    public override void Opened(object[] param)
    {
        material = rawImg.material;
        unlockMsg.text = param[0].ToString();
        StartCoroutine(HandlePopup());
    }

    private IEnumerator HandlePopup()
    {
        group.alpha = 1f;
        Vector2 offset = material.mainTextureOffset;
        float elapsedTime = 0f;
        float startAlpha = group.alpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            offset += new Vector2(0.1f, 0.1f) * Time.deltaTime;
            material.mainTextureOffset = offset;
            group.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        group.alpha = 0f;
        UIManager.Hide<UIUnlockPopup>();
    }
}
