namespace AdventOfCode2022.Solutions;

public class Day11 : IAdventSolution
{
    public object PartOne(string input)
    {
        var monkeys = input.Split(Environment.NewLine + Environment.NewLine).Select(ParseMonkey).ToList();

        var round = 0;
        while (round < 20)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.HasNextItem())
                {
                    var nextItemThrow = monkey.NextItemThrow();
                    monkeys[nextItemThrow[0]].CatchItem(nextItemThrow[1]);
                }
            }
            ++round;
        }

        var mostInspected = monkeys.Select(m => m.InspectedCount).OrderByDescending(x => x).Take(2).ToList();
        return mostInspected[0] * mostInspected[1];
    }

    private Monkey ParseMonkey(string input)
    {
        var lines = input.Split(Environment.NewLine).ToList();
        var items = lines[1][18..].Split(", ").Select(int.Parse);
        var operation = ParseOperationLine(lines[2]);
        var testDivisibleBy = int.Parse(lines[3][21..]);
        var nextPositive = int.Parse(lines[4][29..]);
        var nextNegative = int.Parse(lines[5][30..]);

        return new Monkey(items, operation, testDivisibleBy, nextPositive, nextNegative);
    }

    private Func<int, int> ParseOperationLine(string line)
    {
        var subject = line[25..];
        if (subject == "old")
        {
            return line[23] switch
            {
                '+' => x => x + x,
                '*' => x => x * x,
                _ => throw new ArgumentException($"unexpected operation symbol {line[23]}")
            };
        }
        var value = int.Parse(subject);
        return line[23] switch
        {
            '+' => x => x + value,
            '*' => x => x * value,
            _ => throw new ArgumentException($"unexpected operation symbol {line[23]}")
        };
    }

    public object PartTwo(string input) => 0;

    private class Monkey
    {
        private readonly Queue<int> _itemQueue;
        private readonly Func<int, int> _operation;
        private readonly Func<int, int> _throwTo;
        public Monkey(IEnumerable<int> items, Func<int, int> operation, int testDivisibleBy, int nextPositive, int nextNegative)
        {
            _itemQueue = new Queue<int>(items);
            _operation = operation;
            _throwTo = x => (x % testDivisibleBy) == 0 ? nextPositive : nextNegative;
        }

        public void CatchItem(int item) => _itemQueue.Enqueue(item);

        public bool HasNextItem() => _itemQueue.Count > 0;
        
        // id, value
        public int[] NextItemThrow()
        {
            ++InspectedCount;
            var value = _itemQueue.Dequeue();
            var afterInspection = _operation(value);
            var beforeThrow = afterInspection / 3;
            var throwTo = _throwTo(beforeThrow);
            return new [] { throwTo, beforeThrow };
        }
        
        public int InspectedCount { get; private set; }
    }
}