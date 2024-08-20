using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnlockPopup : UIBase
{
    [SerializeField] private RawImage rawImg;
    private Material materialInstance;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI unlockMsg;

    [SerializeField] private float fadeDuration = 1f;

    public override void Opened(object[] param)
    {
        materialInstance = Instantiate(rawImg.material);
        rawImg.material = materialInstance;

        unlockMsg.text = param[0].ToString();
        StartCoroutine(HandlePopup());
    }

    private IEnumerator HandlePopup()
    {
        group.alpha = 1f;
        Vector2 offset = materialInstance.mainTextureOffset;
        float elapsedTime = 0f;
        float startAlpha = group.alpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            offset += new Vector2(0.1f, 0.1f) * Time.deltaTime;
            materialInstance.mainTextureOffset = offset;
            group.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        group.alpha = 0f;
        UIManager.Hide<UIUnlockPopup>();
    }

    private void OnDestroy()
    {
        if (materialInstance != null)
        {
            Destroy(materialInstance);
        }
    }
}

//public class ScrollingBackground : MonoBehaviour
//{
//    [SerializeField] private RawImage _img;
//    [SerializeField] private float _x, _y;

//    void Update()
//    {
//        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
//    }
//}