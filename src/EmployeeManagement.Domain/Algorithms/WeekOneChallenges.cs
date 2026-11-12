namespace EmployeeManagement.Domain.Algorithms;

/// <summary>Week 1 Easy practice problems, implemented with simple, linear-time approaches.</summary>
public static class WeekOneChallenges
{
    // Concatenation of Array: O(n) time and O(n) output space.
    public static int[] GetConcatenation(IReadOnlyList<int> numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);
        var result = new int[numbers.Count * 2];
        for (var i = 0; i < numbers.Count; i++)
        {
            result[i] = numbers[i];
            result[i + numbers.Count] = numbers[i];
        }
        return result;
    }

    // Number of Steps to Reduce a Number to Zero: O(log n) time and O(1) space.
    public static int NumberOfSteps(int number)
    {
        if (number < 0) throw new ArgumentOutOfRangeException(nameof(number), "Number must be non-negative.");
        var steps = 0;
        while (number > 0)
        {
            number = number % 2 == 0 ? number / 2 : number - 1;
            steps++;
        }
        return steps;
    }

    // Contains Duplicate: O(n) expected time and O(n) space.
    public static bool ContainsDuplicate(IReadOnlyList<int> numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);
        var seen = new HashSet<int>();
        foreach (var number in numbers)
            if (!seen.Add(number)) return true;
        return false;
    }

    // Best Time to Buy and Sell Stock: O(n) time and O(1) space.
    public static int MaxProfit(IReadOnlyList<int> prices)
    {
        ArgumentNullException.ThrowIfNull(prices);
        var lowestPrice = int.MaxValue;
        var bestProfit = 0;
        foreach (var price in prices)
        {
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(prices), "Prices must be non-negative.");
            lowestPrice = Math.Min(lowestPrice, price);
            bestProfit = Math.Max(bestProfit, price - lowestPrice);
        }
        return bestProfit;
    }

    // Valid Anagram: O(n) time and O(k) space for the character counts.
    public static bool IsAnagram(string first, string second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        if (first.Length != second.Length) return false;
        var counts = new Dictionary<char, int>();
        foreach (var character in first) counts[character] = counts.GetValueOrDefault(character) + 1;
        foreach (var character in second)
        {
            if (!counts.TryGetValue(character, out var count)) return false;
            if (count == 1) counts.Remove(character);
            else counts[character] = count - 1;
        }
        return counts.Count == 0;
    }
}
