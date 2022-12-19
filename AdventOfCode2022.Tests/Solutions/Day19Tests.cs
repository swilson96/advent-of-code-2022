using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day19Tests
{
    private const string ExampleInput = @"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day19().PartOne(ExampleInput);
        
        Assert.Equal(33, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day19().PartTwo(ExampleInput);
        
        Assert.Equal(0, result);
    }
}