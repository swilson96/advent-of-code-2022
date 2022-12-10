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

    public object PartTwo(string input)
    {
        var screen = new [] { new char[40], new char[40], new char[40], new char[40], new char[40], new char[40] };
        
        var row = 0;
        var position = 0;
        var register = 1;
        foreach (var instruction in input.Split(Environment.NewLine))
        {
            screen[row][position] = Math.Abs(position - register) <= 1 ? '#' : '.';
            
            if (instruction.StartsWith("addx"))
            {
                ++position;
                if (position >= 40)
                {
                    position = 0;
                    ++row;
                }
                    
                screen[row][position] = Math.Abs(position - register) <= 1 ? '#' : '.';
                register += int.Parse(instruction[5..]);
            }
            
            ++position;
            if (position >= 40)
            {
                position = 0;
                ++row;
            }
        }

        return string.Join(Environment.NewLine, screen.Select(r => string.Join("", r)));
    }
}