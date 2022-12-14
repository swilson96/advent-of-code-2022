using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day02Tests
{
    private const string ExampleInput = @"A Y
B X
C Z";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day02().PartOne(ExampleInput);
        
        Assert.Equal(15, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day02().PartTwo(ExampleInput);
            
        Assert.Equal(12, result);
    }
}