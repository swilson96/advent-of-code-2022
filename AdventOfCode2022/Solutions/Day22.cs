using System.Runtime.Intrinsics.X86;
using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day22 : IAdventSolution
{
    public object PartOne(string input)
    {
        var bits = input.Split(Environment.NewLine + Environment.NewLine);

        var mapLines = bits[0].Split(Environment.NewLine);
        var grid = mapLines
            .Select(l => l.ToCharArray())
            .ToArray();

        var position = new Point(mapLines[0].IndexOf('.'), 0);
        var direction = Direction.R;

        foreach (var instruction in ParseInstructions(bits[1]))
        {
            (position, direction) = instruction.Apply(position, direction, grid);
        }
        
        return 1000 * (position.Y + 1) + 4 * (position.X + 1) + direction switch
        {
            Direction.R => 0,
            Direction.U => 1,
            Direction.L => 2,
            Direction.D => 3,
            _ => throw new ArgumentOutOfRangeException($"not a direction: {direction}")
        };
    }

    public object PartTwo(string input)
    {
        return 0;
    }

    private IEnumerable<Instruction> ParseInstructions(string input)
    {
        var stringSoFar = "";
        foreach (var nextChar in input.ToCharArray())
        {
            if (nextChar > 57) // letter
            {
                yield return new Move(int.Parse(stringSoFar));
                yield return new Turn(nextChar == 'R' ? Hand.R : Hand.L);
                stringSoFar = "";
            }
            else // number
            {
                stringSoFar += nextChar;
            }
        }

        if (!string.IsNullOrEmpty(stringSoFar))
        {
            yield return new Move(int.Parse(stringSoFar));
        }
    }

    private enum Hand
    {
        L, R
    }

    private abstract class Instruction
    {
        public abstract Tuple<Point, Direction> Apply(Point p, Direction d, char[][] grid);
    }

    private class Move : Instruction
    {
        private readonly int _distance;

        public Move(int distance)
        {
            _distance = distance;
        }

        public override Tuple<Point, Direction> Apply(Point p, Direction d, char[][] grid)
        {
            var toGo = _distance;
            var next = p;
            while (toGo > 0)
            {
                var n = next.Add(d, 1);
                
                if (d == Direction.L && (n.X < 0 || grid[n.Y][n.X] == ' '))
                {
                    n = new Point(Math.Max(Array.LastIndexOf(grid[n.Y], '.'), Array.LastIndexOf(grid[n.Y], '#')), n.Y);
                }

                if (d == Direction.D && (n.Y < 0 || grid[n.Y][n.X] == ' '))
                {
                    if (n.Y < 0)
                    {
                        n = new Point(n.X, n.X < 100 ? 149 : 49);
                    }
                    else
                    {
                        n = new Point(n.X,
                            Math.Max(Array.LastIndexOf(grid.Select(c => c[n.X]).ToArray(), '.'),
                                Array.LastIndexOf(grid.Select(c => c[n.X]).ToArray(), '#')));
                    }
                }

                if (d == Direction.R && (n.X >= grid[n.Y].Length || grid[n.Y][n.X] == ' '))
                {
                    n = new Point(Math.Min(Array.IndexOf(grid[n.Y], '.'), Array.IndexOf(grid[n.Y], '#')), n.Y);
                }

                if (d == Direction.U && (n.Y >= grid.Length || n.X >= grid[n.Y].Length || grid[n.Y][n.X] == ' '))
                {
                    if (n.Y < grid.Length && n.X >= grid[n.Y].Length)
                    {
                        n = new Point(n.X, 0); // hack based on our input
                    }
                    else
                    {
                        n = new Point(n.X,
                            Math.Min(Array.IndexOf(grid.Select(c => c[n.X]).ToArray(), '.'),
                                Array.IndexOf(grid.Select(c => c[n.X]).ToArray(), '#')));
                    }
                }

                if (grid[n.Y][n.X] == '#')
                {
                    return new Tuple<Point, Direction>(next, d);
                }

                next = n;
                --toGo;
            }

            return new Tuple<Point, Direction>(next, d);
        }

        public override string ToString() => $"Day22.Move{{{_distance}}}";
    }
    
    private class Turn : Instruction
    {
        private readonly Hand _hand;

        public Turn(Hand hand)
        {
            _hand = hand;
        }

        public override Tuple<Point, Direction> Apply(Point p, Direction d, char[][] grid) =>
            new (p, d switch
            {
                // note: Direction.U is the question's down!
                Direction.R => _hand == Hand.R ? Direction.U : Direction.D,
                Direction.D => _hand == Hand.R ? Direction.R : Direction.L,
                Direction.L => _hand == Hand.R ? Direction.D : Direction.U,
                Direction.U => _hand == Hand.R ? Direction.L : Direction.R,
                _ => throw new ArgumentOutOfRangeException(nameof(d), d, "not a direction")
            });
        
        public override string ToString() => $"Day22.Turn{{{_hand}}}";
    }
}