using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day09 : IAdventSolution
{
    public object PartOne(string input)
    {
        var rope = new Rope();
        var visited = new HashSet<Point> { rope.TailPosition };
        
        foreach (var s in input.Split(Environment.NewLine).Select(l => l.Split(" ")))
        {
            var direction = Enum.Parse<Direction>(s[0]);
            var distance = int.Parse(s[1]);
            while (distance > 0)
            {
                rope.MoveHead(direction);
                visited.Add(rope.TailPosition);
                --distance;
            }
        }

        return visited.Count();
    }

    public object PartTwo(string input) => 0;

    private class Rope
    {
        private Point _headPosition = Point.Origin;
        private Point _tailOffset = Point.Origin;
        
        public void MoveHead(Direction direction)
        {
            _headPosition = _headPosition.Add(direction, 1);
            _tailOffset = _tailOffset.Add(direction.Opposite(), 1);

            var newOffsetY = direction switch
            {
                Direction.U => int.Max(-1, _tailOffset.Y),
                Direction.R => _tailOffset.X < -1 ? 0 : _tailOffset.Y,
                Direction.D => int.Min(1, _tailOffset.Y),
                Direction.L => _tailOffset.X > 1 ? 0 : _tailOffset.Y,
                _ => throw new ArgumentOutOfRangeException($"No known direction {direction}")
            };

            var newOffsetX = direction switch
            {
                Direction.U => _tailOffset.Y < -1 ? 0 : _tailOffset.X,
                Direction.R => int.Max(-1, _tailOffset.X),
                Direction.D => _tailOffset.Y > 1 ? 0 : _tailOffset.X,
                Direction.L => int.Min(1, _tailOffset.X),
                _ => throw new ArgumentOutOfRangeException($"No known direction {direction}")
            };

            _tailOffset = new Point(newOffsetX, newOffsetY);
        }

        public Point TailPosition => _headPosition.Add(_tailOffset);
    }

}