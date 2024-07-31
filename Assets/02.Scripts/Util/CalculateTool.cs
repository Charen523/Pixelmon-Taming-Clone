using System.Numerics;
using UnityEngine;

public static class CalculateTool
{
    public static float SumDiffSeries(float startValue, int startLv, int reachLv, float commonDiff = 1)
    {
        return startValue + ((reachLv - startLv) * commonDiff);
    }

    public static float SumRateSeries(float startValue, int startLv, int reachLv, float multiplier)
    {
        return startValue * (Mathf.Pow(multiplier, reachLv - startLv) - 1) / (multiplier - 1);
    }

    #region Price Calculator
    public static BigInteger GetPrice(int level)
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

    public static BigInteger GetTotalPrice(int startLv, int endLv)
    {
        BigInteger totalPrice = 0;

        for (int i = startLv; i < endLv; i++)
        {
            totalPrice += GetPrice(i);
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