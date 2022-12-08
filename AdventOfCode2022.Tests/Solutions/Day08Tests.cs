using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day08Tests
{
    private const string ExampleInput = @"30373
25512
65332
33549
35390";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day08().PartOne(ExampleInput);
        
        Assert.Equal(21, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day08().PartTwo(ExampleInput);
        
        Assert.Equal(8, result);
    }
    
    [Theory]
    [InlineData(2, 1, 4)] // example in question
    [InlineData(2, 3, 8)] // 2nd example in question
    [InlineData(4, 4, 0)]
    public void PartTwoSpecificTrees(int x, int y, int expected)
    {
        var ut = new Day08();
        var forest = ut.ParseForest(ExampleInput);
        var result = ut.ScenicScore(forest, x, y);
        Assert.Equal(expected, result);
    }
}