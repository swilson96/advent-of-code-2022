using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day14Tests
{
    private const string ExampleInput = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day14().PartOne(ExampleInput);
        
        Assert.Equal(24, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day14().PartTwo(ExampleInput);
        
        Assert.Equal(93, result);
    }
}