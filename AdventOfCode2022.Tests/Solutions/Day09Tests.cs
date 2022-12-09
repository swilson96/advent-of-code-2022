
using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day09Tests
{
    private const string ExampleInput = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day09().PartOne(ExampleInput);
        
        Assert.Equal(13, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day09().PartTwo(ExampleInput);
        
        Assert.Equal(1, result);
    }
    
    [Fact]
    public void PartTwoSecondExample()
    {
        var result = new Day09().PartTwo(@"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20");
        
        Assert.Equal(36, result);
    }
}