using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIUtils
{
    public static IEnumerator AnimateTextChange(TextMeshProUGUI textElement, int startValue, int endValue, float duration = 0.5f)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            int currentValue = (int)Mathf.Lerp(startValue, endValue, elapsed / duration);
            textElement.text = currentValue.ToString();
            yield return null;
        }

        textElement.text = endValue.ToString();
    }

    public static IEnumerator AnimateSliderChange(Slider slider, int startValue, int endValue, int maxValue, float duration = 0.5f)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            int currentValue = (int)Mathf.Lerp(startValue, endValue, elapsed / duration);
            slider.value = (float)currentValue / maxValue;
            yield return null;
        }

        slider.value = (float)endValue / maxValue;
    }
}
