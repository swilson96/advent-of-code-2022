using MoreLinq;

namespace AdventOfCode2022.Solutions;

public class Day08 : IAdventSolution
{
    public object PartOne(string input)
    {
        var forest = ParseForest(input);

        var width = forest[0].Length;
        var height = forest.Length;

        var visible = new bool[height][];
        Enumerable.Range(0, height).ForEach(i => visible[i] = new bool[width]);

        for (var x = 0; x < width; ++x)
        {
            var highestFromTop = -1;
            var highestFromBottom = -1;
            for (var y = 0; y < height; ++y)
            {
                if (forest[y][x] > highestFromTop)
                {
                    visible[y][x] = true;
                    highestFromTop = forest[y][x];
                }
                if (forest[height - y - 1][x] > highestFromBottom)
                {
                    visible[height - y - 1][x] = true;
                    highestFromBottom = forest[height - y - 1][x];
                }
            }
        }
        
        for (var y = 0; y < height; ++y)
        {
            var highestFromLeft = -1;
            var highestFromRight = -1;
            for (var x = 0; x < width; ++x)
            {
                if (forest[y][x] > highestFromLeft)
                {
                    visible[y][x] = true;
                    highestFromLeft = forest[y][x];
                }
                if (forest[y][width - x - 1] > highestFromRight)
                {
                    visible[y][width - x - 1] = true;
                    highestFromRight = forest[y][width - x - 1];
                }
            }
        }
        
        // count visible trees
        return visible.Select(row => row.Count(x => x)).Sum();
    }

    public int[][] ParseForest(string input) => input.Split(Environment.NewLine)
        .Select(l => l.ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray())
        .ToArray();

    public object PartTwo(string input)
    {
        var forest = ParseForest(input);
        var maxScore = 0;
        
        forest.ForEach((row, y) => row.ForEach((_, x) =>
        {
            var score = ScenicScore(forest, x, y);
            if (score > maxScore)
            {
                maxScore = score;
            }
        }));

        return maxScore;
    }

    public int ScenicScore(int[][] forest, int xStart, int yStart)
    {
        var treeHeight = forest[yStart][xStart];
        var width = forest[0].Length;
        var height = forest.Length;

        var lookRight = 0;
        if (xStart < width + 1)
        {
            for (var x = xStart + 1; x < width; ++x)
            {
                ++lookRight;
                if (forest[yStart][x] >= treeHeight)
                {
                    break;
                }
            }
        }

        var lookLeft = 0;
        if (xStart > 0)
        {
            for (var x = xStart - 1; x >= 0; --x)
            {
                ++lookLeft;
                if (forest[yStart][x] >= treeHeight)
                {
                    break;
                }
            }
        }

        var lookDown = 0;
        if (yStart < height + 1)
        {
            for (var y = yStart + 1; y < height; ++y)
            {
                ++lookDown;
                if (forest[y][xStart] >= treeHeight)
                {
                    break;
                }
            }
        }

        var lookUp = 0;
        if (yStart > 0)
        {
            for (var y = yStart - 1; y >= 0; --y)
            {
                ++lookUp;
                if (forest[y][xStart] >= treeHeight)
                {
                    break;
                }
            }
        }

        return lookUp * lookLeft * lookDown * lookRight;
    }
}