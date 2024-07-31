using UnityEngine;

public static class SequenceTool
{
    public static float SumDiffSeries(float startValue, int startLv, int reachLv, float commonDiff = 1)
    {
        return startValue + ((reachLv - startLv) * commonDiff);
    }

    public static float SumRateSeries(float startValue, int startLv, int reachLv, float multiplier = 1)
    {
        return startValue * (Mathf.Pow(multiplier, reachLv - startLv) - 1) / (multiplier - 1);
    }
}