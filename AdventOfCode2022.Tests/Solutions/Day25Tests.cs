using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day25Tests
{
    private const string ExampleInput = @"1=-0-2
12111
2=0=
21
2=01
111
20012
112
1=-1=
1-12
12
1=
122";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day25().PartOne(ExampleInput);
        
        Assert.Equal("2=-1=0", result);
    }

    [Theory]
    [InlineData(1, "1")]
    [InlineData(2, "2")]
    [InlineData(3, "1=")]
    [InlineData(4, "1-")]
    [InlineData(5, "10")]
    [InlineData(6, "11")]
    [InlineData(7, "12")]
    [InlineData(8, "2=")]
    [InlineData(9, "2-")]
    [InlineData(10, "20")]
    [InlineData(15, "1=0")]
    [InlineData(20, "1-0")]
    [InlineData(2022, "1=11-2")]
    [InlineData(12345, "1-0---0")]
    [InlineData(314159265, "1121-1110-1=0")]
    public void SnafuConversion(long dec, string snafu)
    {
        var parsed = Day25.Snafu.Parse(snafu);
        var fromVal = (Day25.Snafu)dec;
        
        Assert.Equal(dec, (long)parsed);
        Assert.Equal(snafu, fromVal.ToString());
    }

    [Theory]
    [InlineData("1=-0-2", 1747)]
    [InlineData("12111", 906)]
    [InlineData("2=0=", 198)]
    [InlineData("21", 11)]
    [InlineData("2=01", 201)]
    [InlineData("111", 31)]
    [InlineData("20012", 1257)]
    [InlineData("112", 32)]
    [InlineData("1=-1=", 353)]
    [InlineData("1-12", 107)]
    [InlineData("12", 7)]
    [InlineData("1=", 3)]
    [InlineData("122", 37)]
    public void PartOneFuelReadings(string snafu, long dec)
    {
        var parsed = Day25.Snafu.Parse(snafu);
        var fromVal = (Day25.Snafu)dec;
        
        Assert.Equal(dec, (long)parsed);
        Assert.Equal(snafu, fromVal.ToString());
    }
}