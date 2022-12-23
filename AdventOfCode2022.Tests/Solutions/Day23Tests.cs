using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day23Tests
{
    private const string ExampleInput = @"....#..
..###.#
#...#.#
.#...##
#.###..
##.#.##
.#..#..";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day23().PartOne(ExampleInput);
        
        Assert.Equal(110, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day23().PartTwo(ExampleInput);
        
        Assert.Equal(20, result);
    }
}