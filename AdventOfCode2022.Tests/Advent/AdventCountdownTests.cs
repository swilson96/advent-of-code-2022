using AdventOfCode2022.Advent;

namespace AdventOfCode2022.Tests.Advent;

public class AdventCountdownTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(13)]
    [InlineData(24)]
    [InlineData(25)]
    public void DatesInAdvent2022Returned(int day)
    {
        var sut = new AdventCountdown(new CustomDateProvider(new DateOnly(2022, 12, day)));
        Assert.True(sut.IsAdventNow());
        Assert.Equal(day, sut.DayOfAdvent());
    }
    
    [Theory]
    [InlineData(2022, 3, 3)]
    [InlineData(2022, 11, 1)]
    [InlineData(2022, 11, 30)]
    [InlineData(2022, 12, 26)]
    [InlineData(2022, 12, 30)]
    [InlineData(2023, 1, 1)]
    [InlineData(2021, 12, 1)]
    [InlineData(2023, 12, 1)]
    public void DatesOutsideAdvent2022ReturnZero(int year, int month, int day)
    {
        var sut = new AdventCountdown(new CustomDateProvider(new DateOnly(year, month, day)));
        Assert.False(sut.IsAdventNow());
        Assert.Equal(0, sut.DayOfAdvent());
    }
}