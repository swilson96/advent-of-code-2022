using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day18Tests
{
    private const string ExampleInput = @"2,2,2
1,2,2
3,2,2
2,1,2
2,3,2
2,2,1
2,2,3
2,2,4
2,2,6
1,2,5
3,2,5
2,1,5
2,3,5";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day18().PartOne(ExampleInput);
        
        Assert.Equal(64, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day18().PartTwo(ExampleInput);
        
        Assert.Equal(58, result);
    }
}