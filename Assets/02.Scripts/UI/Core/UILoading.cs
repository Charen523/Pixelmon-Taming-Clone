using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UILoading : Singleton<UILoading>
{
    [SerializeField] private Image bg;
    [SerializeField] private Slider slider;
    [SerializeField] private TMPro.TMP_Text desc;

    private float curProgress;
    private float asyncOperationProgress;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public static void Show(Sprite bg = null)
    {
        Instance.SetBG(bg);
        Instance.gameObject.SetActive(true);
    }

    public void SetBG(Sprite bg = null)
    {
        if (bg != null)
            this.bg.sprite = bg;
    }

    public static void Hide()
    {
        Instance.gameObject.SetActive(false);
    }


    private void UpdateSliderValue()
    {
        slider.value = (curProgress + asyncOperationProgress) / 10f;
        int percentage = Mathf.RoundToInt(slider.value * 100f);
        desc.text = $"{percentage}%";
    }

    public void SetProgress(float progress)
    {
        curProgress += progress;
        UpdateSliderValue();
    }

    #region Addressable Async
    public void SetProgress(AsyncOperationHandle op)
    {
        StartCoroutine(Progress(op));
    }

    public IEnumerator Progress(AsyncOperationHandle op)
    {
        while (!op.IsDone)
        {
            asyncOperationProgress = op.GetDownloadStatus().Percent;
            UpdateSliderValue();
            yield return new WaitForEndOfFrame();
        }
        UpdateSliderValue();
    }
    #endregion
}