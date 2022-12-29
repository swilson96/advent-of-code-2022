using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions;

public class Day16 : IAdventSolution
{
    private readonly Regex _inputRegex = new(@"Valve (\S+) has flow rate=(\d+); tunnels? leads? to valves? (.+)$");

    public object PartOne(string input)
    {
        var valves = ParseValves(input);

        var cave = new Cave(valves);
        var score = cave.TopScore("AA", 30);
        return score;
    }
    
    public object PartTwo(string input)
    {
        var valves = ParseValves(input);
        
        var cave = new Cave(valves);
        var score = cave.TopScoreWithElephant("AA", 26);
        return score;
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

        public Valve(string name, int flow, List<string> neighbours)
        {
            Name = name;
            Flow = flow;
            Neighbours = neighbours;
        }
    }

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
            var routes = AllRoutes(start, visited, toVisit, timeLeft, false)
                .Where(r => r.Count > 1).ToList();
            
            Console.WriteLine($"found {routes.Count} routes to score");

            // Cutoff is a bit of a guess
            var topRoutes = routes.Where(r => ScoreRoute(r, timeLeft) > 800).ToList();
            Console.WriteLine($"found {topRoutes.Count} routes with a good score");
            var scores = topRoutes.AsParallel().Select(r => ScoreRouteWithElephant(r, timeLeft));
            
            return scores.Max();
        }

        public int ScoreRouteWithElephant(List<string> route, int timeLeft)
        {
            var start = route[0];
            var first = route[1];
            var visited = new HashSet<string> { start };
            var toVisit = new HashSet<string>(_valves.Keys.Where(k => _valves[k].Flow != 0).Where(k => !route.Contains(k) && k != start));
            var routes = AllRoutes(start, visited, toVisit, timeLeft, true)
                .Where(r => r.Count > 1 && string.Compare(r[1], first, StringComparison.Ordinal) > 0);

            var scores = routes.Select(r => ScoreRoute(r, timeLeft));
            
            return ScoreRoute(route, timeLeft) + scores.Append(0).Max();
        }
    }
}