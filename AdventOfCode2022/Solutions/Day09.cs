using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day09 : IAdventSolution
{
    public object PartOne(string input)
    {
        var rope = new Rope();
        return CountTailVisits(rope, input);
    }

    private int CountTailVisits(Rope rope, string input)
    {
        var visited = new HashSet<Point> { rope.TailPosition };
        
        foreach (var s in input.Split(Environment.NewLine).Select(l => l.Split(" ")))
        {
            var direction = Enum.Parse<Direction>(s[0]);
            var distance = int.Parse(s[1]);
            while (distance > 0)
            {
                rope.MoveHeadTo(rope.HeadPosition.Add(direction, 1));
                visited.Add(rope.TailPosition);
                --distance;
            }
        }

        return visited.Count();
    }

    public object PartTwo(string input)
    {
        var rope = new Rope(
            new Rope(
                new Rope(
                    new Rope(
                        new Rope(
                            new Rope(
                                new Rope(
                                    new Rope(
                                        new Rope()))))))));
        return CountTailVisits(rope, input);
    }

    private class Rope
    {
        private Point _headPosition = Point.Origin;
        private Point _tailOffset = Point.Origin;

        private readonly Rope? _tail;

        public Rope()
        {
        }
        
        public Rope(Rope tail)
        {
            _tail = tail;
        }
        
        public void MoveHeadTo(Point newHead)
        {
            var move = newHead.Subtract(_headPosition);
            _headPosition = newHead;

            _tailOffset = _tailOffset.Subtract(move);
            
            var newOffsetX = _tailOffset.X;
            var newOffsetY = _tailOffset.Y;

            if (Math.Abs(_tailOffset.X) == 2 && Math.Abs(_tailOffset.Y) == 2)
            {
                // Diagonal
                newOffsetX = _tailOffset.X / 2;
                newOffsetY = _tailOffset.Y / 2;
            }
            else if (_tailOffset.Y > 1)
            {
                // Up
                newOffsetX = 0;
                newOffsetY = 1;
            }
            else if (_tailOffset.Y < -1)
            {
                // Down
                newOffsetX = 0;
                newOffsetY = -1;
            }
            else if (_tailOffset.X > 1)
            {
                // Right
                newOffsetX = 1;
                newOffsetY = 0;
            } 
            else if (_tailOffset.X < -1)
            {
                // Left
                newOffsetX = -1;
                newOffsetY = 0;
            }
            
            _tailOffset = new Point(newOffsetX, newOffsetY);
            
            _tail?.MoveHeadTo(_headPosition.Add(_tailOffset));
        }

        public Point HeadPosition => _headPosition;

        public Point TailPosition => _tail != null ? _tail.TailPosition : _headPosition.Add(_tailOffset);
    }
}