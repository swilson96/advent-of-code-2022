namespace AdventOfCode2022.Solutions;

public class Day04 : IAdventSolution
{
    public object PartOne(string input) => input.Split(Environment.NewLine)
        .Select(s => s.Split(","))
        .Where(OneRangeCompletelyContainsAnother)
        .Count();

    private bool OneRangeCompletelyContainsAnother(string[] arg)
    {
        var area1 = new CampArea(arg[0]);
        var area2 = new CampArea(arg[1]);
        return area1.Contains(area2) || area2.Contains(area1);
    }

    public object PartTwo(string input) => input.Split(Environment.NewLine)
        .Select(s => s.Split(","))
        .Where(RangesOverlap)
        .Count();
    
    private bool RangesOverlap(string[] arg)
    {
        var area1 = new CampArea(arg[0]);
        var area2 = new CampArea(arg[1]);
        return area1.Overlaps(area2);
    }

    private class CampArea
    {
        private int Start { get; }
        private int End { get; }
        
        public CampArea(string serialised)
        {
            var bits = serialised.Split("-");
            Start = int.Parse(bits[0]);
            End = int.Parse(bits[1]);
        }

        public bool Contains(CampArea other)
        {
            return Start <= other.Start && other.End <= End;
        }

        public bool Overlaps(CampArea other)
        {
            return (other.Start <= Start && Start <= other.End)
                || (Start <= other.Start && other.Start <= End);
        }
    }
}