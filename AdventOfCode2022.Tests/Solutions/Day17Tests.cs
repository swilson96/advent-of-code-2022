using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day17Tests
{
    private const string ExampleInput = @">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day17().PartOne(ExampleInput);
        
        Assert.Equal(3068L, result);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 4)]
    [InlineData(3, 6)]
    [InlineData(4, 7)]
    [InlineData(5, 9)]
    [InlineData(6, 10)]
    [InlineData(7, 13)]
    [InlineData(8, 15)]
    [InlineData(9, 17)]
    [InlineData(10, 17)]
    public void PartOneStepByStep(int totalRocks, long expectedHeight)
    {
        var result = new Day17().RockTower(ExampleInput, totalRocks);
        
        Assert.Equal(expectedHeight, result);
    }
    
    [Fact]
    public void PartTwoExample()
    {
        var result = new Day17().PartTwo(ExampleInput);
        
        Assert.Equal(1514285714288, result);
    }
}