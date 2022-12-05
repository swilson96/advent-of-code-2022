using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day05Tests
{
    private const string ExampleInput = @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day05().PartOne(ExampleInput);
        
        Assert.Equal("CMZ", result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day05().PartTwo(ExampleInput);
            
        Assert.Equal("MCD", result);
    }
}