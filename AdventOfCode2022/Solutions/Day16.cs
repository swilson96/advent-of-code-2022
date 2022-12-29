using System.Text.RegularExpressions;
// using MoreLinq;
// using MoreLinq.Extensions;

namespace AdventOfCode2022.Solutions;

public class Day16 : IAdventSolution
{
    private readonly Regex _inputRegex = new(@"Valve (\S+) has flow rate=(\d+); tunnels? leads? to valves? (.+)$");

    public object PartOne(string input)
    {
        var valves = ParseValves(input);

        // return TopScore("AA", null, valves, 30);

        var cave = new Cave(valves);
        var score = cave.TopScore("AA", 30);
        return score;
    }
    
    public object PartTwo(string input)
    {
        var valves = ParseValves(input);

        // return TopScoreWithElephant("AA", "AA", null, null, valves, 26);
        
        var cave = new Cave(valves);
        var score = cave.TopScoreWithElephant("AA", 26);
        return score;
    }

    /**
     * Naive depth-first search, includes a large number of duff routes. Too slow.
     */
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

        return Enumerable.Prepend(current.Neighbours
                .Where(neighbour => neighbour != previous)
                .Select(neighbour => TopScore(valves[neighbour].Name, startLocation, valves, timeRemaining - 1)), best)
            .Max();
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
            bestProtagonistSwitches = Enumerable.Prepend(eCurrent.Neighbours
                    .Where(neighbour => neighbour != pPrev && neighbour != ePrev)
                    .Select(eNeighbour => TopScoreWithElephant(protagonistStart, valves[eNeighbour].Name, null, elephantStart, valves, timeRemaining - 1)), 0)
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
            bestElephantSwitches = Enumerable.Prepend(pCurrent.Neighbours
                    .Where(neighbour => neighbour != pPrev && neighbour != ePrev)
                    .Select(neighbour => TopScoreWithElephant(valves[neighbour].Name, elephantStart, protagonistStart, null, valves, timeRemaining - 1)), 0)
                .Max() + (timeRemaining - 1) * eCurrent.Flow;
            eCurrent.IsOn = false;
                    
            if (eCurrent.Flow > 3)
            {
                return bestElephantSwitches;
            }
        }

        return Enumerable.Prepend(pCurrent.Neighbours
                .Where(neighbour => neighbour != pPrev && neighbour != ePrev)
                .SelectMany(neighbour => eCurrent.Neighbours
                    .Where(eNeighbour => eNeighbour != ePrev)
                    .Where(eNeighbour => elephantStart != protagonistStart || string.Compare(eNeighbour, neighbour, StringComparison.Ordinal) > 0) // diagonal
                    .Select(eNeighbour => TopScoreWithElephant(valves[neighbour].Name, valves[eNeighbour].Name, protagonistStart, elephantStart, valves, timeRemaining - 1)))
                .Prepend(bestBothSwitch)
                .Prepend(bestProtagonistSwitches), bestElephantSwitches)
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

    /*
     * Faster approach than above, caching distances first then searching through valves to operate
     * rather than valves to travel through
     */
    private class Cave
    {
        readonly Dictionary<string, Valve> _valves;

        private readonly Dictionary<string, Dictionary<string, int>> _distances = new();
        
        public Cave(Dictionary<string, Valve> valves)
        {
            _valves = valves;

            foreach (var valve in _valves)
            {
                _distances[valve.Key] = new();
                var unvisited = new HashSet<string>(valve.Value.Neighbours);
                var distance = 1;
                while (unvisited.Count > 0)
                {
                    var nextUnvisited = new HashSet<string>();
                    foreach (var current in unvisited)
                    {
                        if (_distances[valve.Key].ContainsKey(current) || current == valve.Key)
                        {
                            continue;
                        }

                        _distances[valve.Key][current] = distance;
                        nextUnvisited.UnionWith(_valves[current].Neighbours);
                    }

                    ++distance;
                    unvisited = nextUnvisited;
                }
            }
        }

        public List<List<string>> AllRoutes(string start, HashSet<string> visited, HashSet<string> toVisit, int timeLeft, bool alwaysFinish)
        {
            if (timeLeft < 0)
            {
                throw new Exception("oh dear we went over time");
            }
            
            var ret = new List<List<string>>();
            foreach (var next in toVisit
                         .Where(next => !visited.Contains(next))
                         .Where(next => timeLeft - _distances[start][next] - 1 > 0)) // extra one to turn on the valve on arrival
            {
                visited.Add(next);
                var routesFromNext = AllRoutes(next, visited, toVisit, timeLeft - _distances[start][next] - 1, alwaysFinish)
                    .Select(r => r.Prepend(start).ToList()).ToList();
                visited.Remove(next);
                ret.AddRange(routesFromNext);
            }

            if (!alwaysFinish || ret.Count == 0)
            {
                ret.Add(new List<string> { start });
            }

            return ret;
        }

        public int TopScore(string start, int timeLeft)
        {
            // find all routes that fit into the time
            var visited = new HashSet<string> { start };
            var toVisit = new HashSet<string>(_valves.Keys.Where(k => _valves[k].Flow != 0));
            var routes = AllRoutes(start, visited, toVisit, timeLeft, true);
            // score them
            var scores = routes.Select(r => ScoreRoute(r, timeLeft));
            
            return scores.Max();
        }
        
        public int ScoreRoute(List<string> route, int timeLeft)
        {
            var score = 0;
            var prev = route[0];
            foreach (var next in route.Skip(1))
            {
                timeLeft -= _distances[prev][next] + 1;
                score += _valves[next].Flow * timeLeft;
                prev = next;
            }

            return score;
        }

        public int TopScoreWithElephant(string start, int timeLeft)
        {
            var visited = new HashSet<string> { start };
            var toVisit = new HashSet<string>(_valves.Keys.Where(k => _valves[k].Flow != 0));
            var routes = AllRoutes(start, visited, toVisit, timeLeft, false);
            var scores = routes.Select(r => ScoreRouteWithElephant(r, timeLeft));
            
            return scores.Max();
        }

        public int ScoreRouteWithElephant(List<string> route, int timeLeft)
        {
            var start = route[0];
            var visited = new HashSet<string> { start };
            var toVisit = new HashSet<string>(_valves.Keys.Where(k => _valves[k].Flow != 0).Where(k => !route.Contains(k) && k != start));
            var routes = AllRoutes(start, visited, toVisit, timeLeft, true);

            var scores = routes.Select(r => ScoreRoute(r, timeLeft));
            
            return ScoreRoute(route, timeLeft) + scores.Max();
        }
    }
}