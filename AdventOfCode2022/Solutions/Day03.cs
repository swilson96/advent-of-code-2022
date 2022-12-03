using MoreLinq;

namespace AdventOfCode2022.Solutions;

public class Day03 : IAdventSolution
{
    private const int GroupSize = 3;
    
    public int PartOne(string input) => input.Split(Environment.NewLine)
        .Select(IdentifyMispackedItemInPack)
        .Select(PrioritiseItem)
        .Sum();
    
    private char IdentifyMispackedItemInPack(string pack)
    {
        var sectionOne = pack.Take(pack.Length / 2);
        var sectionTwo = pack.Skip(pack.Length / 2);
        var lookup = new HashSet<char>(sectionOne.ToArray());
        return sectionTwo.First(item => lookup.Contains(item));
    }
    
    private int PrioritiseItem(char item)
    {
        var encoding = (int)item;
        return encoding > 96
            ? encoding - 96 // lowercase start at 97
            : encoding - 64 + 26; // uppercase start at 65
    }

    public int PartTwo(string input) => input.Split(Environment.NewLine)
        .Batch(GroupSize)
        .Select(IdentifyBadgeInGroup)
        .Select(PrioritiseItem)
        .Sum();

    private char IdentifyBadgeInGroup(IEnumerable<string> group)
    {
        var groupList = group.ToList();
        
        if (groupList.Count() != GroupSize)
        {
            throw new ArgumentException($"Group of size {groupList.Count()}, must be {GroupSize}");
        }
        
        var first = groupList[0];
        var lookups = groupList.Skip(1).Select(pack => new HashSet<char>(pack)).ToList();

        return first.ToCharArray().First(item => lookups.All(l => l.Contains(item)));
    }
}