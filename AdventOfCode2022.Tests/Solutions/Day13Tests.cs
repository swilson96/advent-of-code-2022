using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day13Tests
{
    private const string ExampleInput = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day13().PartOne(ExampleInput);
        
        Assert.Equal(13, result);
    }

    [Theory]
    [InlineData("[1,1,3,1,1]", "[1,1,5,1,1]")]
    [InlineData("[[1],[2,3,4]]", "[[1],4]")]
    [InlineData("[[4,4],4,4]", "[[4,4],4,4,4]")]
    [InlineData("[]", "[3]")]
    [InlineData("[[],9]", "[[8,7,6]]")]
    public void ComparisonTestsLessThan(string a, string b)
    {
        var result = new Day13().Compare(a, b);
        
        Assert.True(result < 0);
    }
    
    [Theory]
    [InlineData("[9]", "[[8,7,6]]")]
    [InlineData("[7,7,7,7]", "[7,7,7]")]
    [InlineData("[[[]]]", "[[]]")]
    [InlineData("[1,[2,[3,[4,[5,6,7]]]],8,9]", "[1,[2,[3,[4,[5,6,0]]]],8,9]")]
    [InlineData("[[[[10,8],[9,6,10]]]]", "[[0,9],[0,[[5,7,5],[1,8],7,[6,5,8,7]],[[6]],[]],[],[]]")]
    [InlineData("[10]", "[2]")]
    public void ComparisonTestsGreaterThan(string a, string b)
    {
        var result = new Day13().Compare(a, b);
        
        Assert.True(result > 0);
    }
    
    [Fact]
    public void SplitPlainList()
    {
        var result = new Day13().SplitList("[1,1,3,1,1]").ToList();
        
        Assert.Equal(5, result.Count);
        Assert.Equal("1", result[0]);
    }
    
    [Fact]
    public void SplitMixedList()
    {
        var result = new Day13().SplitList("[1,[],3,[1,[4]],5]").ToList();
        
        Assert.Equal(5, result.Count);
        Assert.Equal("1", result[0]);
        Assert.Equal("[]", result[1]);
        Assert.Equal("3", result[2]);
        Assert.Equal("[1,[4]]", result[3]);
        Assert.Equal("5", result[4]);
    }
    
    [Fact]
    public void SplitNestedList()
    {
        var result = new Day13().SplitList("[[]]").ToList();
        
        Assert.Equal(1, result.Count);
        Assert.Equal("[]", result[0]);
    }
    
    [Fact]
    public void SplitNestedNestedList()
    {
        var result = new Day13().SplitList("[[[],3],[3,[]]]").ToList();
        
        Assert.Equal(2, result.Count);
        Assert.Equal("[[],3]", result[0]);
        Assert.Equal("[3,[]]", result[1]);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day13().PartTwo(ExampleInput);
        
        Assert.Equal(0, result);
    }
}