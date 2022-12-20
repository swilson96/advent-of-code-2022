using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day20Tests
{
    private const string ExampleInput = @"1
2
-3
3
-2
0
4";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day20().PartOne(ExampleInput);
        
        Assert.Equal(3L, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day20().PartTwo(ExampleInput);
        
        Assert.Equal(1623178306L, result);
    }
}