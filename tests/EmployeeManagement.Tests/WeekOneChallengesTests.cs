using EmployeeManagement.Domain.Algorithms;
using Xunit;

namespace EmployeeManagement.Tests;

public sealed class WeekOneChallengesTests
{
    [Fact]
    public void GetConcatenation_ReturnsArrayTwice()
        => Assert.Equal(new[] { 1, 2, 1, 2 }, WeekOneChallenges.GetConcatenation(new[] { 1, 2 }));

    [Theory]
    [InlineData(14, 6)]
    [InlineData(0, 0)]
    public void NumberOfSteps_HandlesNormalAndBoundaryInputs(int value, int expected)
        => Assert.Equal(expected, WeekOneChallenges.NumberOfSteps(value));

    [Fact]
    public void NumberOfSteps_RejectsNegativeInput()
        => Assert.Throws<ArgumentOutOfRangeException>(() => WeekOneChallenges.NumberOfSteps(-1));

    [Fact]
    public void ContainsDuplicate_DetectsRepeatedValues()
        => Assert.True(WeekOneChallenges.ContainsDuplicate(new[] { 1, 2, 1 }));

    [Fact]
    public void ContainsDuplicate_ReturnsFalseForEmptyInput()
        => Assert.False(WeekOneChallenges.ContainsDuplicate(Array.Empty<int>()));

    [Fact]
    public void MaxProfit_UsesBestEarlierBuyPrice()
        => Assert.Equal(5, WeekOneChallenges.MaxProfit(new[] { 7, 1, 5, 3, 6, 4 }));

    [Fact]
    public void MaxProfit_ReturnsZeroWhenNoProfitIsPossible()
        => Assert.Equal(0, WeekOneChallenges.MaxProfit(new[] { 7, 6, 4, 3, 1 }));

    [Theory]
    [InlineData("anagram", "nagaram", true)]
    [InlineData("rat", "car", false)]
    [InlineData("", "", true)]
    public void IsAnagram_ComparesCharacterCounts(string first, string second, bool expected)
        => Assert.Equal(expected, WeekOneChallenges.IsAnagram(first, second));
}
