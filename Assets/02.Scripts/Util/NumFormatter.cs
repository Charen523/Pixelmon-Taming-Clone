public static class NumberFormatter
{
    public static string FormatIntNum(int number)
    {
        if (number < 1000)
        {
            return number.ToString();
        }

        int alphabetIndex = 0;
        while (number >= 1000 && alphabetIndex < 26)
        {
            number /= 1000;
            alphabetIndex++;
        }

        char suffix = (char)('A' + alphabetIndex - 1);
        return number.ToString("0.##") + suffix;
    }

    public static int FormatIntStr(string number)
    {
        char suffix = number[number.Length - 1]; // 마지막 문자.
        if (suffix < 'A' || suffix > 'Z')
        {
            return int.Parse(number);
        }

        string numericPart = number.Substring(0, number.Length - 1);
        float baseNumber = float.Parse(numericPart);

        int alphabetIndex = suffix - 'A' + 1;
        int multiplier = 1;
        for (int i = 0; i < alphabetIndex; i++)
        {
            multiplier *= 1000;
        }

        return (int)(baseNumber * multiplier);
    }
}