using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInvoker : MonoBehaviour
{
    [SerializeField] private Image image;
    public WaitForSeconds waitFadeTime = new WaitForSeconds(1f);
    private bool isUsing;

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        if (!isUsing)
        {
            isUsing = true;
            image.color = new Color(0, 0, 0, 0);
            yield return waitFadeTime;
            image.DOFade(1f, 0.5f).OnComplete(() => Player.Instance.gameObject.transform.position = Vector3.zero);
            yield return waitFadeTime;    
            isUsing = false;
        }
    }

    public IEnumerator FadeOut(WaitForSeconds time = null)
    {
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
