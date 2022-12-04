using System.Transactions;
using MoreLinq;

namespace AdventOfCode2022.Solutions;

public class Day04 : IAdventSolution
{
    public int PartOne(string input) => input.Split(Environment.NewLine)
        .Select(s => s.Split(","))
        .Where(OneRangeCompletelyContainsAnother)
        .Count();

    private bool OneRangeCompletelyContainsAnother(string[] arg)
    {
        var area1 = new CampArea(arg[0]);
        var area2 = new CampArea(arg[1]);
        return area1.Contains(area2) || area2.Contains(area1);
    }

    public int PartTwo(string input)
    {
        return 0;
    }

    private class CampArea
    {
        private int Start { get; set; }
        private int End { get; set; }
        
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
    }
}