using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day01Tests
{
    private const string ExampleInput = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day01().PartOne(ExampleInput);
        
        Assert.Equal(24000, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day01().PartTwo(ExampleInput);
            
        Assert.Equal(45000, result);
}
}