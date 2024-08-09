using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] [Range(0.1f, 10f)] private float fadeTime;

    public WaitForSeconds waitFadeTime = new WaitForSeconds(0.5f);

    /// <summary>
    /// 일반적으로 사용
    /// 플레이어 죽음, 실패 애니메이션 중간쯤에 이벤트 메서드로 사용했음
    /// </summary>
    /// <예시>
    /// StageManager.Instance.fadeOut.gameObject.SetActive(true);
    /// StageManager.Instance.fadeOut.StartFadeInOut();
    /// </예시>
    public void StartFadeInOut()
    {
        StartCoroutine(FadeInOut());
    }

    /// <summary>
    /// 코루틴 안에서 사용! 
    /// </summary>
    /// <예시>
    /// fadeOut.gameObject.SetActive(true);
    /// yield return fadeOut.FadeInOut();
    /// </예시>
    /// <param name="time"> 페이드인과 아웃 사이에 검은 화면 지연시간</param>
    /// <returns></returns>
    public IEnumerator FadeInOut(WaitForSeconds time = null)
    {
        if (time == null)
            time = waitFadeTime;

        yield return Fade(0, 1);
        yield return time;
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
