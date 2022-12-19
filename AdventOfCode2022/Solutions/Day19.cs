using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions;

public class Day19 : IAdventSolution
{
    public object PartOne(string input) => input.Split(Environment.NewLine)
        .Select(Blueprint.Parse)
        .Select(b => ScoreBlueprint(b, 24))
        .Select((s, i) => s * (i + 1))
        .Sum();

    public int ScoreBlueprint(Blueprint blueprint, int minutes)
    {
        var robots = Enum.GetValues<Material>().ToDictionary(v => v, _ => 0);
        robots[Material.Ore] = 1;
        var materials = Enum.GetValues<Material>().ToDictionary(v => v, _ => 0);

        return Math.Max(ScoreBlueprint(blueprint, robots, materials, Material.Ore, minutes),
            ScoreBlueprint(blueprint, robots, materials, Material.Clay, minutes));
    }

    private int ScoreBlueprint(Blueprint blueprint, Dictionary<Material, int> robots, Dictionary<Material, int> materials, Material nextToBuild, int timeLeft)
    {
        var geodesThisMinute = robots.ContainsKey(Material.Geode) ? robots[Material.Geode] : 0;
        
        if (timeLeft == 1)
        {
            return geodesThisMinute;
        }
        
        var nextMaterials = materials.ToDictionary(e => e.Key, e => e.Value + robots[e.Key]);
        if (blueprint.CanBuildWith(nextToBuild, materials))
        {
            ++robots[nextToBuild];
            nextMaterials = blueprint.SubtractCost(nextToBuild, nextMaterials);
            var score = geodesThisMinute + blueprint.ViableBuilds(robots).Select(r => ScoreBlueprint(blueprint, robots, nextMaterials, r, timeLeft - 1)).Max();
            --robots[nextToBuild];
            return score;
        }
        
        return geodesThisMinute + ScoreBlueprint(blueprint, robots, nextMaterials, nextToBuild, timeLeft - 1);
    }
    
    public object PartTwo(string input) => input.Split(Environment.NewLine)
        .Take(3)
        .Select(Blueprint.Parse)
        .Select(b => ScoreBlueprint(b, 32))
        .Aggregate(1, (acc, next) => acc * next);

    public enum Material
    {
        Ore, Clay, Obsidian, Geode
    }
    
    public class Blueprint
    {
        private static readonly Regex InputRegex =
            new(
                @"Blueprint \d+: Each ore robot costs (\d+) ore\. Each clay robot costs (\d+) ore\. Each obsidian robot costs (\d+) ore and (\d+) clay\. Each geode robot costs (\d+) ore and (\d+) obsidian\."); 
        
        // material to build => material to spend => amount
        private readonly Dictionary<Material, Dictionary<Material, int>> _costs = new();

        private readonly int _maxOreCost;

        private Blueprint(int oreCost, int clayCost, int obsCostOre, int obsCostClay, int geoCostOre, int geoCostObs)
        {
            _costs[Material.Ore] = new Dictionary<Material, int> { { Material.Ore, oreCost } };
            _costs[Material.Clay] = new Dictionary<Material, int> { { Material.Ore, clayCost } };
            _costs[Material.Obsidian] = new Dictionary<Material, int> { { Material.Ore, obsCostOre }, { Material.Clay, obsCostClay } };
            _costs[Material.Geode] = new Dictionary<Material, int> { { Material.Ore, geoCostOre }, { Material.Obsidian, geoCostObs } };

            _maxOreCost = new List<int> { oreCost, clayCost, obsCostOre, geoCostOre }.Max();
        }

        private int Cost(Material toBuild, Material toSpend) => _costs[toBuild][toSpend];

        public bool CanBuildWith(Material toBuild, Dictionary<Material, int> materials)
        {
            var costs = _costs[toBuild];
            return costs.All(e => materials[e.Key] >= e.Value);
        }
        
        public Dictionary<Material, int> SubtractCost(Material toBuild, Dictionary<Material, int> materials)
        {
            var costs = _costs[toBuild];
            return materials.ToDictionary(e => e.Key, e => e.Value - (costs.ContainsKey(e.Key) ? costs[e.Key] : 0));
        }
        
        public IEnumerable<Material> ViableBuilds(Dictionary<Material, int> robots)
        {
            if (robots[Material.Ore] < _maxOreCost)
            {
                yield return Material.Ore;
            }

            if (robots[Material.Clay] < _costs[Material.Obsidian][Material.Clay])
            {
                yield return Material.Clay;
            }

            if (robots[Material.Clay] > 0 && robots[Material.Obsidian] < _costs[Material.Geode][Material.Obsidian])
            {
                yield return Material.Obsidian;
            }

            if (robots[Material.Obsidian] > 0)
            {
                yield return Material.Geode;
            }
        }

        public static Blueprint Parse(string inputLine)
        {
            var match = InputRegex.Match(inputLine);
            return new Blueprint(
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value),
                int.Parse(match.Groups[4].Value),
                int.Parse(match.Groups[5].Value),
                int.Parse(match.Groups[6].Value));
        }
    }
}