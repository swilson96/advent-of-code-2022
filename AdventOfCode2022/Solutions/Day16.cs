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
                current.IsOn = false;
                return (timeRemaining - 1) * current.Flow;
            }
            // can go back
            best = TopScore(startLocation, null, valves, timeRemaining - 1) + (timeRemaining - 1) * current.Flow;
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

    public object PartTwo(string input)
    {
        var valves = ParseValves(input);

        return TopScoreWithElephant("AA", "AA", null, null, valves, 26);
    }
    
    private int TopScoreWithElephant(string protagonistStart, string elephantStart, string? pPrev, string? ePrev, Dictionary<string, Valve> valves, int timeRemaining)
    {
        if (timeRemaining == 0)
        {
            return 0;
        }
        
        var pCurrent = valves[protagonistStart];
        var eCurrent = valves[elephantStart];
        var bestBothSwitch = 0;
        var bestProtagonistSwitches = 0;
        var bestElephantSwitches = 0;
        if (!pCurrent.IsOn && pCurrent.Flow > 0)
        {
            pCurrent.IsOn = true;
            if (valves.All(v => v.Value.IsOn || v.Value.Flow <= 0))
            {
                pCurrent.IsOn = false;
                return (timeRemaining - 1) * pCurrent.Flow;
            }
            if (elephantStart != protagonistStart && !eCurrent.IsOn && eCurrent.Flow > 0)
            {
                eCurrent.IsOn = true;
                if (valves.All(v => v.Value.IsOn || v.Value.Flow <= 0))
                {
                    eCurrent.IsOn = false;
                    pCurrent.IsOn = false;
                    return (timeRemaining - 1) * (pCurrent.Flow + eCurrent.Flow);
                }
                bestBothSwitch = TopScoreWithElephant(protagonistStart, elephantStart, null, null, valves, timeRemaining - 1) + (timeRemaining - 1) * (pCurrent.Flow + eCurrent.Flow);
                eCurrent.IsOn = false;
                
                if (pCurrent.Flow > 3 && eCurrent.Flow > 3)
                {
                    pCurrent.IsOn = false;
                    return bestBothSwitch;
                }
            }
            
            // elephant moves instead of switching
            bestProtagonistSwitches = eCurrent.Neighbours
                .Where(neighbour => neighbour != pPrev && neighbour != ePrev)
                .Select(eNeighbour => TopScoreWithElephant(protagonistStart, valves[eNeighbour].Name, null, elephantStart, valves, timeRemaining - 1))
                .Prepend(0)
                .Max() + (timeRemaining - 1) * pCurrent.Flow;

            pCurrent.IsOn = false;
            
            if (pCurrent.Flow > 3)
            {
                return bestProtagonistSwitches;
            }
        } 
        else if (elephantStart != protagonistStart && !eCurrent.IsOn && eCurrent.Flow > 0)
        {
            eCurrent.IsOn = true;
            if (valves.All(v => v.Value.IsOn || v.Value.Flow <= 0))
            {
                eCurrent.IsOn = false;
                return (timeRemaining - 1) * eCurrent.Flow;
            }
            bestElephantSwitches = pCurrent.Neighbours
                .Where(neighbour => neighbour != pPrev && neighbour != ePrev)
                .Select(neighbour => TopScoreWithElephant(valves[neighbour].Name, elephantStart, protagonistStart, null, valves, timeRemaining - 1))
                .Prepend(0)
                .Max() + (timeRemaining - 1) * eCurrent.Flow;
            eCurrent.IsOn = false;
                    
            if (eCurrent.Flow > 3)
            {
                return bestElephantSwitches;
            }
        }

        return pCurrent.Neighbours
            .Where(neighbour => neighbour != pPrev && neighbour != ePrev)
            .SelectMany(neighbour => eCurrent.Neighbours
                .Where(eNeighbour => eNeighbour != ePrev)
                .Where(eNeighbour => elephantStart != protagonistStart || string.Compare(eNeighbour, neighbour, StringComparison.Ordinal) > 0) // diagonal
                .Select(eNeighbour => TopScoreWithElephant(valves[neighbour].Name, valves[eNeighbour].Name, protagonistStart, elephantStart, valves, timeRemaining - 1)))
            .Prepend(bestBothSwitch)
            .Prepend(bestProtagonistSwitches)
            .Prepend(bestElephantSwitches)
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

    private class Valve
    {
        public string Name { get; }
        public int Flow { get; }
        public List<string> Neighbours { get; }

        public bool IsOn;

        public Valve(string name, int flow, List<string> neighbours)
        {
            Name = name;
            Flow = flow;
            Neighbours = neighbours;
        }
    }
}