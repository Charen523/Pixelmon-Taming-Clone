using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UILoading : Singleton<UILoading>
{
    [SerializeField] private Image bg;
    [SerializeField] private Slider slider;
    [SerializeField] private TMPro.TMP_Text desc;
    [SerializeField] private TMPro.TMP_Text progessNum;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);

        slider.onValueChanged.AddListener(delegate { UpdateSliderValue(); });
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

    public void OnClickOveraly()
    {
        GameManager.isInit = true;
    }

    public void HideProgress()
    {
        slider.gameObject.SetActive(false);
        progessNum.gameObject.SetActive(false);
        desc.text = "화면을 터치하여 시작하기";
    }

    private void UpdateSliderValue()
    {
        int percentage = Mathf.RoundToInt(slider.value * 100f);
        progessNum.text = $"{percentage}%";
    }

    public void SetProgress(float progress, string desc = "")
    {
        this.desc.text = desc;
        slider.value = progress;
    }

    #region Basic Async
    public void SetProgress(AsyncOperation op, string desc = "")
    {
        this.desc.text = desc;
        StartCoroutine(Progress(op));
    }

    public IEnumerator Progress(AsyncOperation op)
    {
        while (op.isDone)
        {
            slider.value = op.progress;
            yield return new WaitForEndOfFrame();
        }
        slider.value = 1;
    }
    #endregion

    #region Addressable Async
    public void SetProgress(AsyncOperationHandle op, string desc = "")
    {
        this.desc.text = desc;
    }

    public IEnumerator Progress(AsyncOperationHandle op)
    {
        while (op.IsDone)
        {
            slider.value = op.GetDownloadStatus().Percent;
            yield return new WaitForEndOfFrame();
        }
        slider.value = 1;
    }
    #endregion
}