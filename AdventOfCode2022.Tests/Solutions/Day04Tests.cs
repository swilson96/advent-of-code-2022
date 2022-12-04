using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day04Tests
{
    private const string ExampleInput = @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day04().PartOne(ExampleInput);
        
        Assert.Equal(2, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day04().PartTwo(ExampleInput);
            
        Assert.Equal(0, result);
}
}