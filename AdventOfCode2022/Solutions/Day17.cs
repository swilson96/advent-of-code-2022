using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day17 : IAdventSolution
{
    private const int Width = 7;

    public object PartOne(string input) => RockTower(input, 2022);
    
    public long RockTower(string jets, long totalRocks)
    {
        var jetLength = jets.Length;
        var jetIndex = 0;
        var loopedHeight = 0L;
        
        using var push = PushGenerator(jets).GetEnumerator();
        var rocks = new HashSet<Point>(Enumerable.Range(0, Width).Select(x => new Point(x, 0)));
        var heights = Enumerable.Range(0, Width).Select(_ => 0).ToArray();

        var rockNum = 1;
        var previousStates = Enumerable.Range(0, jetLength).ToDictionary(
            i => i,
            _ => Enumerable.Range(0, 5).ToDictionary(i => i, _ => new Dictionary<int[], Tuple<int, int>>())
        );
        previousStates[0][0].Add(heights, new (0, 0));
        var looped = false;
        
        while (rockNum <= totalRocks)
        {
            var rock = Rockfall.ForNumber(rockNum, rocks);

            while (!rock.Landed)
            {
                push.MoveNext();
                ++jetIndex;
                rock.Move(push.Current);
                rock.Move(Direction.D); // might land rock instead
            }

            rocks = rock.RocksWithSelf();
            heights = NormaliseHeights(rock.AddSelfToHeights(heights));

            // check for a loop
            if (!looped)
            {
                if (previousStates[jetIndex % jetLength][rockNum % 5].Keys.Contains(heights))
                {
                    var (rockNumAtStart, heightAtStart) = previousStates[jetIndex % jetLength][rockNum % 5][heights];
                    var loopLength = rockNum - rockNumAtStart;
                    var heightInLoop = rocks.Max(r => r.Y) - heightAtStart;
                    var numLoopsAfterThisOne = (totalRocks - rockNum) / loopLength;
                    loopedHeight = 1L * heightInLoop * numLoopsAfterThisOne;
                    totalRocks -= loopLength * numLoopsAfterThisOne;
                    looped = true;
                }
                else
                {
                    previousStates[jetIndex % jetLength][rockNum % 5].Add(heights, new(rockNum, rocks.Max(r => r.Y)));
                }
            }

            ++rockNum;
        }
        
        return rocks.Max(r => r.Y) + loopedHeight;
    }

    public IEnumerable<Direction> PushGenerator(string input)
    {
        var directions = input.ToCharArray()
            .Select(c => c switch
            {
                '>' => Direction.R,
                '<' => Direction.L,
                _ => throw new ArgumentException("unexpected input " + c)
            })
            .ToList();

        while (true)
        {
            foreach (var direction in directions)
            {
                yield return direction;
            }
        }
    }

    private int[] NormaliseHeights(int[] heights)
    {
        var min = heights.Min();
        return heights.Select(h => h - min).ToArray();
    }

    public object PartTwo(string input) => RockTower(input, 1000000000000);

    private class Rockfall
    {
        private readonly HashSet<Point> _shape;
        private readonly HashSet<Point> _rocksBelow;
        private readonly int _width;
            
        private Point _position;
        
        public bool Landed { get; private set; }

        public Point Position => _position;
        
        private Rockfall(HashSet<Point> shape, HashSet<Point> rocksBelow)
        {
            _shape = shape;
            _position = new Point(2, rocksBelow.Max(p => p.Y) + 4);
            _rocksBelow = rocksBelow;
            _width = shape.Select(p => p.X).Max() + 1;
        }

        public void Move(Direction direction)
        {
            // check can go in given direction
            if (_shape.Select(part => _position.Add(part).Add(direction, 1)).Any(_rocksBelow.Contains))
            {
                if (direction == Direction.D)
                {
                    Landed = true;
                }

                return;
            }

            if (direction == Direction.L && _position.X == 0)
            {
                return;
            }

            if (direction == Direction.R && _position.X + _width == Width)
            {
                return;
            }
            
            _position = _position.Add(direction, 1);
        }

        public HashSet<Point> RocksWithSelf()
        {
            _rocksBelow.UnionWith(_shape.Select(_position.Add));
            return _rocksBelow;
        }

        public int[] AddSelfToHeights(int[] heights)
        {
            for (int x = 0; x < _width; ++x)
            {
                // watch out, mutating the input
                heights[x + _position.X] = _shape.Where(p => p.X == x).Max(p => p.Y) + _position.Y;
            }

            return heights;
        }

        public static Rockfall ForNumber(int rockNumber, HashSet<Point> rocks)
        {
            if (rockNumber % 5 == 1)
            {
                return new Rockfall(new HashSet<Point>
                {
                    new(0, 0), new(1, 0), new(2, 0), new(3, 0)
                }, rocks);
            }

            if (rockNumber % 5 == 2)
            {
                return new Rockfall(new HashSet<Point>
                {
                    new(0, 1), new(1, 0), new(1, 2), new (2, 1)
                }, rocks);
            }
            
            if (rockNumber % 5 == 3)
            {
                return new Rockfall(new HashSet<Point>
                {
                    new(0, 0), new(1, 0), new(2, 0), new(2, 1), new (2, 2)
                }, rocks);
            }
            
            if (rockNumber % 5 == 4)
            {
                return new Rockfall(new HashSet<Point>
                {
                    new(0, 0), new(0, 1), new(0, 2), new(0, 3)
                }, rocks);
            }
            
            if (rockNumber % 5 == 0) {
                return new Rockfall(new HashSet<Point>
                {
                    new(0, 0), new(0, 1), new(1, 0), new(1, 1)
                }, rocks);
            }

            throw new ArgumentException("not how modulus works!?");
        }
    }
}