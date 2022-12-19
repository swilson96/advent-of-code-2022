using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class TemplateTests
{
    private const string ExampleInput = @"";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Template().PartOne(ExampleInput);
        
        Assert.Equal(0, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Template().PartTwo(ExampleInput);
        
        Assert.Equal(0, result);
    }
}