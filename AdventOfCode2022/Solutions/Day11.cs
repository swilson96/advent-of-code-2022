namespace AdventOfCode2022.Solutions;

public class Day11 : IAdventSolution
{
    public object PartOne(string input) => MonkeyBusinessAfterThisManyRounds(input, 20, x => x / 3);
    
    private long MonkeyBusinessAfterThisManyRounds(string input, long rounds, Func<long, long> worryStrategy)
    {
        var monkeys = input.Split(Environment.NewLine + Environment.NewLine).Select(m => ParseMonkey(m, worryStrategy)).ToList();

        var round = 0;
        while (round < rounds)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.HasNextItem())
                {
                    var nextItemThrow = monkey.NextItemThrow();
                    monkeys[nextItemThrow.Recipient].CatchItem(nextItemThrow.Value);
                }
            }
            ++round;
        }

        var mostInspected = monkeys.Select(m => m.InspectedCount).OrderByDescending(x => x).Take(2).ToList();
        return mostInspected[0] * mostInspected[1];
    }

    private Monkey ParseMonkey(string input, Func<long, long> worryLevelStrategy)
    {
        var lines = input.Split(Environment.NewLine).ToList();
        var items = lines[1][18..].Split(", ").Select(long.Parse);
        var operation = ParseOperationLine(lines[2]);
        var testDivisibleBy = long.Parse(lines[3][21..]);
        var nextPositive = int.Parse(lines[4][29..]);
        var nextNegative = int.Parse(lines[5][30..]);

        return new Monkey(items, operation, testDivisibleBy, nextPositive, nextNegative, worryLevelStrategy);
    }

    private Func<long, long> ParseOperationLine(string line)
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
        var value = long.Parse(subject);
        return line[23] switch
        {
            '+' => x => x + value,
            '*' => x => x * value,
            _ => throw new ArgumentException($"unexpected operation symbol {line[23]}")
        };
    }

    public object PartTwo(string input)
    {
        var monkeys = input.Split(Environment.NewLine + Environment.NewLine).Select(m => ParseMonkey(m, x => x)).ToList();
        var multiple = monkeys.Select(m => m.TestDivisor).Aggregate(1L, (acc, x) => acc * x);
        return MonkeyBusinessAfterThisManyRounds(input, 10000, x => x % multiple);
    }

    private class Monkey
    {
        private readonly Queue<long> _itemQueue;
        private readonly Func<long, long> _operation;
        private readonly Func<long, int> _throwTo;
        private readonly Func<long, long> _worryLevelStrategy;
        
        public long TestDivisor { get; }

        public Monkey(IEnumerable<long> items, Func<long, long> operation, long testDivisibleBy, int nextPositive,
            int nextNegative, Func<long, long> worryLevelStrategy)
        {
            _itemQueue = new Queue<long>(items);
            _operation = operation;
            _throwTo = x => (x % testDivisibleBy) == 0 ? nextPositive : nextNegative;
            _worryLevelStrategy = worryLevelStrategy;
            TestDivisor = testDivisibleBy;
        }

        public void CatchItem(long item) => _itemQueue.Enqueue(item);

        public bool HasNextItem() => _itemQueue.Count > 0;
        
        // id, value
        public MonkeyThrow NextItemThrow()
        {
            ++InspectedCount;
            var value = _itemQueue.Dequeue();
            var afterInspection = _operation(value);
            var beforeThrow = _worryLevelStrategy(afterInspection);
            var throwTo = _throwTo(beforeThrow);
            return new MonkeyThrow(throwTo, beforeThrow);
        }
        public long InspectedCount { get; private set; }
    }

    private class MonkeyThrow
    {
        public int Recipient { get; }
        public long Value { get; }
        public MonkeyThrow(int recipient, long value)
        {
            Recipient = recipient;
            Value = value;
        }
    }
}