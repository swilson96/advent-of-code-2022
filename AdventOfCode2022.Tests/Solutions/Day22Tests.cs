using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day22Tests
{
    private const string ExampleInput = @"        ...#
        .#..
        #...
        ....
...#.......#
........#...
..#....#....
..........#.
        ...#....
        .....#..
        .#......
        ......#.

10R5L5R10L4R5L5";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day22().PartOne(ExampleInput);
        
        Assert.Equal(6032, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day22().PartTwo(ExampleInput);
        
        Assert.Equal(0, result);
    }
}