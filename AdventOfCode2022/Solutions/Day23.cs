using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day23 : IAdventSolution
{
    public object PartOne(string input)
    {
        var elves = ParseElves(input);

        FindSpace(elves, 10);

        return MeasureRectangle(elves);
    }

    private int FindSpace(List<Elf> elves, int maxRounds)
    {
        var round = 1;
        var startDirection = Direction.D; // N
        var anyMoved = true;
        while (anyMoved && round <= maxRounds)
        {
            var elfPositions = new HashSet<Point>(elves.Select(e => e.Position));

            foreach (var elf in elves)
            {
                if (!EightNeighbouringPoints(elf.Position).Any(elfPositions.Contains))
                {
                    continue;
                }
                
                var directionToPropose = startDirection;
                var tries = 1;
                while (tries <= 4)
                {
                    var canPropose = !PlacesToCheck(directionToPropose, elf.Position).Any(elfPositions.Contains);

                    if (canPropose)
                    {
                        elf.Propose(directionToPropose);
                        break;
                    }

                    directionToPropose = NextDirection(directionToPropose);
                    ++tries;
                }
            }

            anyMoved = false;
            foreach (var elf in elves)
            {
                if (elf.Proposed == null)
                {
                    continue;
                }

                var elvesProposing = elves.Where(e => elf.Proposed.Equals(e.Proposed)).ToList();

                if (elvesProposing.Count == 1)
                {
                    elf.ExecuteProposal();
                    anyMoved = true;
                }
                else
                {
                    elvesProposing.ForEach(e => e.CancelProposal());
                }
            }
            
            startDirection = NextDirection(startDirection);
            ++round;
        }

        return round - 1;
    }

    private Direction NextDirection(Direction last) => last switch
    {
        Direction.D => Direction.U,
        Direction.U => Direction.L,
        Direction.L => Direction.R,
        Direction.R => Direction.D,
        _ => throw new ArgumentException($"not a direction {last}")
    };

    private IEnumerable<Point> PlacesToCheck(Direction d, Point p)
    {
        var directStep = p.Add(d, 1);
        yield return directStep;
        
        if (d is Direction.D or Direction.U)
        {
            yield return directStep.Add(Direction.L, 1);
            yield return directStep.Add(Direction.R, 1);
        }
        
        if (d is Direction.L or Direction.R)
        {
            yield return directStep.Add(Direction.U, 1);
            yield return directStep.Add(Direction.D, 1);
        }
    }

    private IEnumerable<Point> EightNeighbouringPoints(Point p)
    {
        var up = p.Add(Direction.U, 1);
        yield return up;
        yield return up.Add(Direction.L, 1);
        yield return up.Add(Direction.R, 1);
        var down = p.Add(Direction.D, 1);
        yield return down;
        yield return down.Add(Direction.L, 1);
        yield return down.Add(Direction.R, 1);
        yield return p.Add(Direction.L, 1);
        yield return p.Add(Direction.R, 1);
    }
    
    private int MeasureRectangle(List<Elf> elves)
    {
        var top = elves.Max(e => e.Position.Y);
        var bottom = elves.Min(e => e.Position.Y);
        var left = elves.Max(e => e.Position.X);
        var right = elves.Min(e => e.Position.X);
        return (top - bottom + 1) * (left - right + 1) - elves.Count;
    }

    public object PartTwo(string input)
    {
        var elves = ParseElves(input);

        return FindSpace(elves, 1000);
    }
    
    private List<Elf> ParseElves(string input) => input.Split(Environment.NewLine).SelectMany(
            (l, y) => l.ToCharArray().Select<char, Point?>((c, x) => c == '#' ? new Point(x, y) : null)
                .Where(p => p != null))
        .Where(p => p != null)
        .Select(p => new Elf(p))
        .ToList();

    private class Elf
    {
        public Point Position { get; private set; }
        public Point? Proposed { get; private set; }

        public Elf(Point position)
        {
            Position = position;
        }

        public void Propose(Direction d)
        {
            Proposed = Position.Add(d, 1);
        }

        public void ExecuteProposal()
        {
            Position = Proposed ?? throw new Exception("tried to confirm but no proposal");
            Proposed = null;
        }

        public void CancelProposal()
        {
            Proposed = null;
        }

        public override string ToString() => $"Day23.Elf{{at ({Position}, proposing {Proposed}}}";
    }
}