namespace AdventOfCode2022.Solutions;

public class Day10 : IAdventSolution
{
    public object PartOne(string input)
    {
        var result = 0;
        var cycle = 1;
        var register = 1;
        foreach (var instruction in input.Split(Environment.NewLine))
        {
            if (CycleIsInteresting(cycle))
            {
                result += cycle * register;
            }
            
            if (instruction.StartsWith("addx"))
            {
                if (CycleIsInteresting(cycle + 1))
                {
                    result += (cycle + 1) * register;
                }
                ++cycle;
                register += int.Parse(instruction[5..]);
            }
            
            ++cycle;
        }

        return result;
    }

    private bool CycleIsInteresting(int cycle) => (cycle + 20) % 40 == 0;

    public object PartTwo(string input) => 0;
}