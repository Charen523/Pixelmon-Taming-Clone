using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] [Range(0.1f, 10f)] private float fadeTime;

    private WaitForSeconds waitFadeTime = new WaitForSeconds(0.5f);

    public void StartFade()
    {
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        yield return Fade(0, 1);
        yield return waitFadeTime;
        yield return Fade(1, 0);

        this.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float start, float end)
    {       
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;

            yield return null;
        }
    }
}
