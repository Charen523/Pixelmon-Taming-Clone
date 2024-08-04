using System.Collections;
using System.Xml.Linq;
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

    public static IEnumerator AnimateSliderChange(Slider slider, float startValue, float endValue, float duration = 0.5f)
    {
        float elapsed = 0f;
        float start = Mathf.Clamp01(startValue);
        int numFullCycles = Mathf.FloorToInt(endValue);  // 전체 바퀴 수
        float fractionalEnd = endValue % 1f;  // 마지막 남은 소수 부분

        // 첫 번째 구간: start -> 1
        if (start < 1f)
        {
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                slider.value = Mathf.Lerp(start, 1f, elapsed / duration);
                yield return null;
            }
            slider.value = 1f;
        }

        // 중간의 전체 바퀴 수만큼: 0 -> 1
        for (int i = 0; i < numFullCycles; i++)
        {
            elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                slider.value = Mathf.Lerp(0f, 1f, elapsed / duration);
                yield return null;
            }
            slider.value = 1f;
        }

        // 마지막 구간: 0 -> fractionalEnd
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(0f, fractionalEnd, elapsed / duration);
            yield return null;
        }
        slider.value = fractionalEnd;
    }

    public static string TranslateRank(this PixelmonRank rank)
    {
        switch (rank)
        {
            case PixelmonRank.Common: return "일반";
            case PixelmonRank.Advanced: return "고급";
            case PixelmonRank.Rare: return "희귀";
            case PixelmonRank.Epic: return "영웅";
            case PixelmonRank.Legendary: return "전설";
        }
        return null;
    }

    public static string TranslateTraitEnum(this TraitType trait)
    {
        switch (trait)
        {
            case TraitType.AddDmg: return "일반 데미지";
            case TraitType.AddCriDmg: return "일반 치명타 데미지";
            case TraitType.AddSDmg: return "스킬 데미지";
            case TraitType.AddSCriDmg: return "스킬 치명타 데미지";
        }
        return null;
    }

    public static string TranslateTraitString(this string trait)
    {
        switch (trait)
        {
            case "AddDmg": return "일반 데미지";
            case "AddCriDmg": return "일반 치명타 데미지";
            case "AddSDmg": return "스킬 데미지";
            case "AddSCriDmg": return "스킬 치명타 데미지";
        }
        return null;
    }

    public static int GetEvolveValue(MyPixelmonData myData, PixelmonData data)
    {
        switch (myData.star)
        {
            case 0:
                return DataManager.Instance.GetData<EvolveData>(data.rank).star1;
            case 1:    
                return DataManager.Instance.GetData<EvolveData>(data.rank).star2;
            case 2:    
                return DataManager.Instance.GetData<EvolveData>(data.rank).star3;
            case 3:    
                return DataManager.Instance.GetData<EvolveData>(data.rank).star4;
            case 4:    
                return DataManager.Instance.GetData<EvolveData>(data.rank).star5;
            default:
                return 0;
        }
    }

    
    public static int GetEvolveValue(MyAtvData myData, ActiveData data)
    {
        string rankTxt = string.Format("{0}Rank", data.rank);
        switch (myData.lv / 10)
        {
            case 0:
                return DataManager.Instance.GetData<EvolveData>(rankTxt).star1;
            case 1:
                return DataManager.Instance.GetData<EvolveData>(rankTxt).star2;
            case 2:
                return DataManager.Instance.GetData<EvolveData>(rankTxt).star3;
            case 3:
                return DataManager.Instance.GetData<EvolveData>(rankTxt).star4;
            case 4:
                return DataManager.Instance.GetData<EvolveData>(rankTxt).star5;
            default:
                return 0;
        }
    }

    // TextMeshProUGUI의 색상을 색상 코드로 변경하는 확장 메서드
    public static void HexColor(this TextMeshProUGUI textElement, string hexColor)
    {
        Color newColor;
        if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
        {
            textElement.color = newColor;
        }
        else
        {
            Debug.LogError("색상 코드 변환에 실패했습니다: " + hexColor);
        }
    }
}
