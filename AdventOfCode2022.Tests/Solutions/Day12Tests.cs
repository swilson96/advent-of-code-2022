using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day12Tests
{
    private const string ExampleInput = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day12().PartOne(ExampleInput);
        
        Assert.Equal(31, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day12().PartTwo(ExampleInput);
        
        Assert.Equal(29, result);
    }
}