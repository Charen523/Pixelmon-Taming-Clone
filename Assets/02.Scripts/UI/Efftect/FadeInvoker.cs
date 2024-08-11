using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInvoker : MonoBehaviour
{
    [SerializeField] private Image image;
    public WaitForSeconds waitFadeTime = new WaitForSeconds(0.5f);
    private bool isUsing;

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        Debug.Log("1");
        if (!isUsing)
        {
            isUsing = true;
            image.color = new Color(0, 0, 0, 0);
            image.DOFade(1f, 0.5f);
            yield return waitFadeTime;
            isUsing = false;
        }
    }

    public IEnumerator FadeOut(WaitForSeconds time = null)
    {
        Debug.Log("2");
        if (!isUsing)
        {
            isUsing = true;

            if (time == null)
                time = waitFadeTime;

            image.color = Color.black;
            yield return time;
            image.DOFade(0f, 0.5f);
            yield return waitFadeTime;

            isUsing = false;
            gameObject.SetActive(false);
        }
    }
}
