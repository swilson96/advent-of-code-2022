using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day02Tests
{
    private const string ExampleInput = @"";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day02().PartOne(ExampleInput);
        
        Assert.Equal(1, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day02().PartTwo(ExampleInput);
            
        Assert.Equal(1, result);
}
}