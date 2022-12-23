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

        foreach (var instruction in ParseInstructions(bits[1], d => new Move(d)))
        {
            (position, direction) = instruction.Apply(position, direction, grid);
        }
        
        return ScoreFinalPosition(position, direction);
    }

    private static int ScoreFinalPosition(Point position, Direction direction) => 1000 * (position.Y + 1) +
        4 * (position.X + 1) + direction switch
        {
            Direction.R => 0,
            Direction.U => 1,
            Direction.L => 2,
            Direction.D => 3,
            _ => throw new ArgumentOutOfRangeException($"not a direction: {direction}")
        };

    public object PartTwo(string input)
    {
        var bits = input.Split(Environment.NewLine + Environment.NewLine);

        var mapLines = bits[0].Split(Environment.NewLine);
        var grid = mapLines
            .Select(l => l.ToCharArray())
            .ToArray();

        var cubeSize = 50;

        var position = new Point(mapLines[0].IndexOf('.'), 0);
        var direction = Direction.R;

        foreach (var instruction in ParseInstructions(bits[1], d => new CubeMove(d, cubeSize)))
        {
            (position, direction) = instruction.Apply(position, direction, grid);
        }
        
        return ScoreFinalPosition(position, direction);
    }

    private class CubeMove : Instruction
    {
        private readonly int _distance;
        private readonly int _cubeSize;

        public CubeMove(int distance, int cubeSize)
        {
            _distance = distance;
            _cubeSize = cubeSize;
        }

        public override Tuple<Point, Direction> Apply(Point pStart, Direction dStart, char[][] grid)
        {
            var toGo = _distance;
            var next = pStart;
            var nextDir = dStart;
            while (toGo > 0)
            {
                var n = next.Add(nextDir, 1);
                var d = nextDir;
                
                //  12
                //  3
                // 45
                // 6
                if (nextDir == Direction.L)
                {
                    if (n.X < 0)
                    {
                        Assert(n.X == -1);
                        Assert(n.Y >= 2 * _cubeSize);
                        if (n.Y < _cubeSize * 3)
                        {
                            // left of 4 into left of 1 upside down
                            n = new Point(_cubeSize, 3 * _cubeSize - n.Y - 1);
                            d = Direction.R;
                        }
                        else
                        {
                            // left of 6 into top of 1
                            Assert(n.Y < 4 * _cubeSize);
                            n = new Point(n.Y - 2 * _cubeSize, 0);
                            d = Direction.U;
                        }
                    }
                    else if (grid[n.Y][n.X] == ' ')
                    {
                        Assert(n.X == _cubeSize - 1);
                        Assert(n.Y < 2 * _cubeSize);
                        Assert(n.Y > 0);
                        if (n.Y < _cubeSize)
                        {
                            // left of 1 into left of 4 upside down
                            n = new Point(0, _cubeSize * 3 - n.Y - 1);
                            d = Direction.R;
                        }
                        else
                        {
                            // left of 3 into top of 4
                            n = new Point(n.Y - _cubeSize, _cubeSize);
                            d = Direction.U;
                        }
                    }
                        
                    n = new Point(Math.Max(Array.LastIndexOf(grid[n.Y], '.'), Array.LastIndexOf(grid[n.Y], '#')), n.Y);
                }

                if (nextDir == Direction.D)
                {
                    if (n.Y < 0)
                    {
                        Assert(n.Y == -1);
                        if (n.X < 2 * _cubeSize)
                        {
                            // top of 1 into left of 6
                            Assert(n.X >= _cubeSize);
                            n = new Point(0, 2 * _cubeSize + n.X);
                            d = Direction.R;
                        }
                        else
                        {
                            // top of 2 into bottom of 6
                            Assert(n.X < 3 * _cubeSize);
                            n = new Point(n.X - 2 * _cubeSize, 4 * _cubeSize - 1);
                        }
                    }
                    else if (grid[n.Y][n.X] == ' ')
                    {
                        // top of 4 into left of 3
                        Assert(n.Y == 2 * _cubeSize - 1);
                        Assert(n.X < _cubeSize);
                        n = new Point(_cubeSize, _cubeSize + n.X);
                        d = Direction.R;
                    }
                }

                if (nextDir == Direction.R)
                {
                    //  12
                    //  3
                    // 45
                    // 6
                    if (n.X >= grid[n.Y].Length)
                    {
                        if (n.Y < _cubeSize) {
                            // right of two into right of 5 upside down
                            Assert(n.X == 3 * _cubeSize);
                            n = new Point(2 * _cubeSize - 1, 3 * _cubeSize - n.Y - 1);
                            d = Direction.L;
                        }
                        else if (n.Y < 2 * _cubeSize)
                        {
                            // right of 3 into bottom of 2
                            Assert(n.X == 2 * _cubeSize);
                            n = new Point( _cubeSize + n.Y, _cubeSize - 1);
                            d = Direction.D;
                        }
                        else if (n.Y < 3 * _cubeSize)
                        {
                            // right of 5 into right of 2 upside down
                            Assert(n.X == 2 * _cubeSize);
                            n = new Point( 3 * _cubeSize - 1, 3 * _cubeSize - n.Y - 1);
                            d = Direction.L;
                        }
                        else
                        {
                            // right of 6 into bottom of 5
                            Assert(n.X == _cubeSize);
                            n = new Point( n.Y - 2 * _cubeSize, 3 * _cubeSize - 1);
                            d = Direction.D;
                        }
                    }
                }

                if (nextDir == Direction.U)
                {
                    //  12
                    //  3
                    // 45
                    // 6
                    if (n.Y < grid.Length && n.X >= grid[n.Y].Length)
                    {
                        if (n.X < 2 * _cubeSize)
                        {
                            Assert(n.Y == 3 * _cubeSize);
                            Assert(n.X >= _cubeSize);
                            // bottom of 5 into right of 6
                            n = new Point( _cubeSize - 1, 2 * _cubeSize + n.X);
                            d = Direction.L;
                        }
                        else
                        {
                            Assert(n.Y == _cubeSize);
                            // bottom of 2 into right of 3
                            n = new Point( 2 * _cubeSize - 1, n.X - _cubeSize);
                            d = Direction.L;
                        }
                    }
                    else if (n.Y >= grid.Length)
                    {
                        Assert(n.Y == 4 * _cubeSize);
                        // bottom of 6 into top of two
                        n = new Point( n.X + 2 * _cubeSize, 0);
                    }
                }

                if (grid[n.Y][n.X] == '#')
                {
                    return new Tuple<Point, Direction>(next, nextDir);
                }

                if (grid[n.Y][n.X] != '.')
                {
                    throw new Exception("off the cube");
                }

                next = n;
                nextDir = d;
                --toGo;
            }

            return new Tuple<Point, Direction>(next, nextDir);
        }

        private void Assert(bool condition)
        {
            if (!condition)
            {
                throw new Exception("wrong!");
            }
        }

        public override string ToString() => $"Day22.Move{{{_distance}}}";
    }
    
    private class Move : Instruction
    {
        private readonly int _distance;

        public Move(int distance)
        {
            _distance = distance;
        }

        public override Tuple<Point, Direction> Apply(Point pStart, Direction dStart, char[][] grid)
        {
            var toGo = _distance;
            var next = pStart;
            while (toGo > 0)
            {
                var n = next.Add(dStart, 1);
                
                if (dStart == Direction.L && (n.X < 0 || grid[n.Y][n.X] == ' '))
                {
                    n = new Point(Math.Max(Array.LastIndexOf(grid[n.Y], '.'), Array.LastIndexOf(grid[n.Y], '#')), n.Y);
                }

                if (dStart == Direction.D && (n.Y < 0 || grid[n.Y][n.X] == ' '))
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

                if (dStart == Direction.R && (n.X >= grid[n.Y].Length || grid[n.Y][n.X] == ' '))
                {
                    n = new Point(Math.Min(Array.IndexOf(grid[n.Y], '.'), Array.IndexOf(grid[n.Y], '#')), n.Y);
                }

                if (dStart == Direction.U && (n.Y >= grid.Length || n.X >= grid[n.Y].Length || grid[n.Y][n.X] == ' '))
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
                    return new Tuple<Point, Direction>(next, dStart);
                }

                next = n;
                --toGo;
            }

            return new Tuple<Point, Direction>(next, dStart);
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

        public override Tuple<Point, Direction> Apply(Point pStart, Direction dStart, char[][] grid) =>
            new (pStart, dStart switch
            {
                // note: Direction.U is the question's down!
                Direction.R => _hand == Hand.R ? Direction.U : Direction.D,
                Direction.D => _hand == Hand.R ? Direction.R : Direction.L,
                Direction.L => _hand == Hand.R ? Direction.D : Direction.U,
                Direction.U => _hand == Hand.R ? Direction.L : Direction.R,
                _ => throw new ArgumentOutOfRangeException(nameof(dStart), dStart, "not a direction")
            });
        
        public override string ToString() => $"Day22.Turn{{{_hand}}}";
    }
    
    
    private IEnumerable<Instruction> ParseInstructions(string input, Func<int, Instruction> createMove)
    {
        var stringSoFar = "";
        foreach (var nextChar in input.ToCharArray())
        {
            if (nextChar > 57) // letter
            {
                yield return createMove(int.Parse(stringSoFar));
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
        public abstract Tuple<Point, Direction> Apply(Point pStart, Direction dStart, char[][] grid);
    }
}