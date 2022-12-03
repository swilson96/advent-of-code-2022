using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day03Tests
{
    private const string ExampleInput = @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day03().PartOne(ExampleInput);
        
        Assert.Equal(157, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day03().PartTwo(ExampleInput);
            
        Assert.Equal(70, result);
    }
}