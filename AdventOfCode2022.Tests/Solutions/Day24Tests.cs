using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day24Tests
{
    private const string ExampleInput = @"#.######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day24().PartOne(ExampleInput);
        
        Assert.Equal(18, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day24().PartTwo(ExampleInput);
        
        Assert.Equal(0, result);
    }
}