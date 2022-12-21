using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day21Tests
{
    private const string ExampleInput = @"root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day21().PartOne(ExampleInput);
        
        Assert.Equal(152L, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day21().PartTwo(ExampleInput);
        
        Assert.Equal(301L, result);
    }
}