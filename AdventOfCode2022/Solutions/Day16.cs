using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions;

public class Day16 : IAdventSolution
{
    private readonly Regex _inputRegex = new(@"Valve (\S+) has flow rate=(\d+); tunnels? leads? to valves? (.+)$");

    public object PartOne(string input)
    {
        var valves = ParseValves(input);

        return TopScore("AA", null, valves, 30);
    }

    private int TopScore(string startLocation, string? previous, Dictionary<string, Valve> valves, int timeRemaining)
    {
        if (timeRemaining == 0)
        {
            return 0;
        }
        
        var current = valves[startLocation];
        var best = 0;
        if (!current.IsOn && current.Flow > 0)
        {
            current.IsOn = true;
            if (valves.All(v => v.Value.IsOn || v.Value.Flow <= 0))
            {
                best = (timeRemaining - 1) * current.Flow;
            }
            else
            {
                // can go back
                best = TopScore(startLocation, null, valves, timeRemaining - 1) + (timeRemaining - 1) * current.Flow;
            }

            current.IsOn = false;
            
            if (current.Flow > 3) // Randomly guessed this cutoff
            {
                return best;
            }
        }

        return current.Neighbours
            .Where(neighbour => neighbour != previous)
            .Select(neighbour => TopScore(valves[neighbour].Name, startLocation, valves, timeRemaining - 1))
            .Prepend(best)
            .Max();
    }

    private Dictionary<string, Valve> ParseValves(string input) => input.Split(Environment.NewLine)
        .Select(l => _inputRegex.Match(l))
        .Select(m =>
        {
            var name = m.Groups[1].Value;
            var flow = int.Parse(m.Groups[2].Value);
            var neighbours = m.Groups[3].Value.Split(", ").ToList();
            return new Valve(name, flow, neighbours);
        })
        .ToDictionary(v => v.Name, v => v);
    
    public object PartTwo(string input)
    {
        var sensors = ParseValves(input);

        return 0;
    }

    private class Valve
    {
        public string Name { get; }
        public int Flow { get; }
        public List<string> Neighbours { get; }

        public bool IsOn = false;

        public Valve(string name, int flow, List<string> neighbours)
        {
            Name = name;
            Flow = flow;
            Neighbours = neighbours;
        }
    }
}