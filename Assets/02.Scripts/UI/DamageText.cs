using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : SerializedMonoBehaviour
{
    [SerializeField]
    Transform parent;
    Coroutine OnDamage;
    Camera cam;
    [SerializeField]
    TextMeshProUGUI damageTxt;
    [SerializeField]
    RectTransform rectTr;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void ShowDamageText(int damage, Vector3 pos)
    {
        if (OnDamage != null)
            StopCoroutine(ShowText(pos));
        damageTxt.text = string.Format("{0:#,###}", damage);
        OnDamage = StartCoroutine(ShowText(pos));
    }

    IEnumerator ShowText(Vector3 pos)
    {
        rectTr.anchoredPosition = RectTransformUtility.WorldToScreenPoint(cam, pos);
        yield return null;
        float time = 0;
        Color textColor = Color.white;
        damageTxt.color = Color.white;
        while (time < 3f)
        {
            time += Time.deltaTime;
            rectTr.anchoredPosition = RectTransformUtility.WorldToScreenPoint(cam, Vector3.Lerp(pos, pos + Vector3.up* 5, time / 3));
            textColor.a = Mathf.Lerp(damageTxt.color.a, 0, time / 3);
            damageTxt.color = textColor;
            yield return null;
        }
        damageTxt.text = null;
        gameObject.SetActive(false);
    }


    private void OnDisable()
    {
        if (OnDamage != null)
            StopCoroutine(OnDamage);
    }
}
