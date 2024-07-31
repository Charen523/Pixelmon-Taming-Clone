using System.Numerics;
using UnityEngine;

public static class CalculateTool
{
    #region Atk Price Calculator
    public static BigInteger GetAtkPrice(int level)
    {
        if (level <= 0)
        {
            return 0;
        }

        int basePrice = 100;
        int interval = (level - 1) / 3;

        long intervalSum = 100 * (interval * (interval + 1));
        BigInteger price = basePrice + intervalSum + 300 * interval;

        return price;
    }

    public static BigInteger GetAtkTotalPrice(int startLv, int endLv)
    {
        BigInteger totalPrice = 0;

        for (int i = startLv; i < endLv; i++)
        {
            totalPrice += GetAtkPrice(i);
        }

        return totalPrice;
    }
    #endregion

    #region 등차수열
    public static float GetFloatDiffSeries(int lv, float startValue, float diff)
    {
        if (startValue <= 0)
        {
            return 0;
        }

        return startValue + diff * (lv - 1);
    }

    #endregion

    #region 등비수열
    public static BigInteger GetRatioPrice(int lv)
    {
        if (lv <= 0)
        {
            return 0;
        }

        int basePrice = 1000;
        BigInteger price = Mathf.RoundToInt( basePrice * Mathf.Pow(1.1f, lv - 1));
        return price;
    }

    public static BigInteger GetTotalRatioPrice(int startLv, int endLv)
    {
        BigInteger totalPrice = 0;

        for (int i = startLv; i < endLv; i++)
        {
            totalPrice += GetRatioPrice(i);
        }

        return totalPrice;
    }
    #endregion
    

    #region Translator
    /// <param name="number">float 값을 넣을 때는 Mathf.RoundToInt를 이용할 것.</param>
    public static string NumFormatter(BigInteger number)
    {
        if (number < 1000)
        {
            return number.ToString();
        }

        int alphabetIndex = 0;
        
        while (number >= 1000000 && alphabetIndex < 26)
        {
            number /= 1000;
            alphabetIndex++;
        }

        float distanceNum = (float)number / 1000;
        alphabetIndex++;

        char suffix = (char)('A' + alphabetIndex - 1);
        return distanceNum.ToString("0.##") + suffix;
    }
    #endregion
}