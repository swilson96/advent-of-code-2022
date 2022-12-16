using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day16Tests
{
    private const string ExampleInput = @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day16().PartOne(ExampleInput);
        
        Assert.Equal(1651, result);
    }

    [Fact]
    public void PartOneSimpleExample()
    {
        var result = new Day16().PartOne(@"Valve AA has flow rate=0; tunnels lead to valves BB, CC
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves BB, AA");
        
        Assert.Equal(416, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day16().PartTwo(ExampleInput);
        
        Assert.Equal(0, result);
    }
}