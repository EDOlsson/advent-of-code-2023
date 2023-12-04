var input = await File.ReadAllLinesAsync("day01-input");

Console.WriteLine($"Read {input.Length} lines from input file.");

var sum = input.Select(MapToCalibrationValuePart1)
               .Sum();

Console.WriteLine($"Sum of calibration values: {sum}");

var calibrationValues = input.Select(MapToCalibrationValuePart2);

Console.WriteLine($"Sum of calibration values: {calibrationValues.Sum()}");

static int MapToCalibrationValuePart1(string s)
{
    int firstValue = s.SkipWhile(c => !('0' <= c && c <= '9'))
                     .Select(c => Convert.ToInt32(c) - Convert.ToInt32('0'))
                     .First();

    int lastValue = s.Reverse()
                     .SkipWhile(c => !('0' <= c && c <= '9'))
                     .Select(c => Convert.ToInt32(c) - Convert.ToInt32('0'))
                     .First();

    return firstValue * 10 + lastValue;
}

static int MapToCalibrationValuePart2(string s)
{
    var firstValue = FindFirstCalibrationValue(s);
    var lastValue = FindSecondCalibrationValue(s);

    Console.WriteLine($"\t{s} : {firstValue * 10 + lastValue}");
    return firstValue * 10 + lastValue;

    static int FindFirstCalibrationValue(string s)
    {
        if (s.Length == 0)
            throw new InvalidOperationException();

        if (s[0] == '0')
            return 0;

        if (s[0] == '1')
            return 1;

        if (s[0] == '2')
            return 2;

        if (s[0] == '3')
            return 3;
        
        if (s[0] == '4')
            return 4;

        if (s[0] == '5')
            return 5;
        
        if (s[0] == '6')
            return 6;

        if (s[0] == '7')
            return 7;

        if (s[0] == '8')
            return 8;

        if (s[0] == '9')
            return 9;

        if (s.Length >=4 && s[..4].Equals("ZERO", StringComparison.InvariantCultureIgnoreCase))
            return 0;

        if (s.Length >= 3 && s[..3].Equals("ONE", StringComparison.InvariantCultureIgnoreCase))
            return 1;

        if (s.Length >= 3 && s[..3].Equals("TWO", StringComparison.InvariantCultureIgnoreCase))
            return 2;

        if (s.Length >= 5 && s[..5].Equals("THREE", StringComparison.InvariantCultureIgnoreCase))
            return 3;

        if (s.Length >= 4 && s[..4].Equals("FOUR", StringComparison.InvariantCultureIgnoreCase))
            return 4;

        if (s.Length >= 4 && s[..4].Equals("FIVE", StringComparison.InvariantCultureIgnoreCase))
            return 5;

        if (s.Length >= 3 && s[..3].Equals("SIX", StringComparison.InvariantCultureIgnoreCase))
            return 6;

        if (s.Length >= 5 && s[..5].Equals("SEVEN", StringComparison.InvariantCultureIgnoreCase))
            return 7;

        if (s.Length >= 5 && s[..5].Equals("EIGHT", StringComparison.InvariantCultureIgnoreCase))
            return 8;

        if (s.Length >= 4 && s[..4].Equals("NINE", StringComparison.InvariantCultureIgnoreCase))
            return 9;

        return FindFirstCalibrationValue(s[1..]);
    }

    static int FindSecondCalibrationValue(string s)
    {
        if (s.Length == 0)
            throw new InvalidOperationException();

        var singleChar = s[^1];

        if ('0' <= singleChar && singleChar <= '9')
            return Convert.ToInt32(singleChar) - Convert.ToInt32('0');

        if (s.Length < 3)
            return FindSecondCalibrationValue(s[..^1]);

        var threeChars = s[^3..];

        if (threeChars.Equals("ONE", StringComparison.InvariantCultureIgnoreCase))
            return 1;

        if (threeChars.Equals("TWO", StringComparison.InvariantCultureIgnoreCase))
            return 2;

        if (threeChars.Equals("SIX", StringComparison.InvariantCultureIgnoreCase))
            return 6;

        if (s.Length < 4)
            return FindSecondCalibrationValue(s[..^1]);

        var fourChars = s[^4..];

        if (fourChars.Equals("ZERO", StringComparison.InvariantCultureIgnoreCase))
            return 0;

        if (fourChars.Equals("FOUR", StringComparison.InvariantCultureIgnoreCase))  
            return 4;

        if (fourChars.Equals("FIVE", StringComparison.InvariantCultureIgnoreCase))
            return 5;

        if (fourChars.Equals("NINE", StringComparison.InvariantCultureIgnoreCase))
            return 9;

        if (s.Length < 5)
            return FindSecondCalibrationValue(s[..^1]);

        var fiveChars = s[^5..];

        if (fiveChars.Equals("THREE", StringComparison.InvariantCultureIgnoreCase))
            return 3;

        if (fiveChars.Equals("SEVEN", StringComparison.InvariantCultureIgnoreCase))
            return 7;

        if (fiveChars.Equals("EIGHT", StringComparison.InvariantCultureIgnoreCase))
            return 8;

        return FindSecondCalibrationValue(s[..^1]);
    }
}