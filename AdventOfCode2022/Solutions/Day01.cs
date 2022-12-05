namespace AdventOfCode2022.Solutions;

public class Day01 : IAdventSolution
{
    public object PartOne(string input)
    {
        return ParseElfCalorieTotals(input)
            .Max();
    }

    private IEnumerable<int> ParseElfCalorieTotals(string input)
    {
        return input.Split(Environment.NewLine + Environment.NewLine)
            .Select(CountSingleElfCalories);
    }

    private int CountSingleElfCalories(string block)
    {
        return block.Split(Environment.NewLine)
            .Select(int.Parse)
            .Sum();
    }

    public object PartTwo(string input)
    {
        return ParseElfCalorieTotals(input)
            .OrderDescending()
            .Take(3)
            .Sum();
    }
}