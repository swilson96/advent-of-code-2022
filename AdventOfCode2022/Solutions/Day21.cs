using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions;

public class Day21 : IAdventSolution
{
    private static readonly Regex YellerRegex = new (@"(\S{4}): (\d+)");
    private static readonly Regex CalculatorRegex = new (@"(\S{4}): (\S{4}) (\S) (\S{4})");
    
    public object PartOne(string input)
    {
        var monkeys = input.Split(Environment.NewLine)
            .Select(ParseMonkey)
            .ToDictionary(m => m.Id, m => m);

        return monkeys["root"].Evaluate(monkeys);
    }

    public object PartTwo(string input)
    {
        var monkeys = input.Split(Environment.NewLine)
            .Select(ParseMonkey)
            .ToDictionary(m => m.Id, m => m);

        var root = (Calculator)monkeys["root"];
        var lhs = monkeys[root.LHS];
        var rhs = monkeys[root.RHS];
        var humn = (Yeller)monkeys["humn"];

        var guess = 1L;
        var timeout = int.MaxValue;
        while (guess < timeout)
        {
            humn.SetValue(guess);
            if (lhs.EvaluateWithCacheData(monkeys).Item1 == rhs.EvaluateWithCacheData(monkeys).Item1)
            {
                return guess;
            }
            ++guess;
        }

        return 0;
    }

    private Monkey ParseMonkey(string inputLine)
    {
        var yellerMatch = YellerRegex.Match(inputLine);
        if (yellerMatch.Success)
        {
            return new Yeller(yellerMatch.Groups[1].Value, long.Parse(yellerMatch.Groups[2].Value));
        }
        
        var calculatorMatch = CalculatorRegex.Match(inputLine);
        if (!calculatorMatch.Success)
        {
            throw new ArgumentException($"regex didn't match: {inputLine}");
        }
        
        return new Calculator(calculatorMatch.Groups[1].Value, calculatorMatch.Groups[2].Value, calculatorMatch.Groups[4].Value,
            calculatorMatch.Groups[3].Value);
    }

    private abstract class Monkey
    {
        public string Id { get; }

        protected Monkey(string id)
        {
            Id = id;
        }

        public abstract long Evaluate(Dictionary<string, Monkey> others);

        public abstract Tuple<long, bool> EvaluateWithCacheData(Dictionary<string, Monkey> others);
    }

    private class Yeller : Monkey
    {
        private long _value;
        private bool _isHuman;
        
        public Yeller(string id, long value) : base(id)
        {
            _value = value;
            _isHuman = id == "humn";
        }

        public override long Evaluate(Dictionary<string, Monkey> others) => _value;
        
        public override Tuple<long, bool> EvaluateWithCacheData(Dictionary<string, Monkey> others) => new (_value, !_isHuman);
        
        public void SetValue(long value)
        {
            _value = value;
        }
    }

    private class Calculator : Monkey
    {
        public string LHS { get; }
        public string RHS { get; }
        
        private readonly string _operation;

        private long? _lCache;
        private long? _rCache;
        
        public Calculator(string id, string lhs, string rhs, string operation) : base(id)
        {
            LHS = lhs;
            RHS = rhs;
            _operation = operation;
        }

        public override long Evaluate(Dictionary<string, Monkey> others)
        {
            var a = others[LHS].Evaluate(others);
            var b = others[RHS].Evaluate(others);
            return ExecuteOperation(a, b);
        }

        private long ExecuteOperation(long a, long b) => _operation switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => a / b,
            _ => throw new AggregateException($"Unknown monkey operation {_operation}")
        };

        public override Tuple<long, bool> EvaluateWithCacheData(Dictionary<string, Monkey> others)
        {
            long a;
            long b;

            var canCache = true;

            if (_lCache == null)
            {
                var (val, canCacheLeft) = others[LHS].EvaluateWithCacheData(others);
                a = val;
                if (canCacheLeft)
                {
                    _lCache = val;
                }
                else
                {
                    canCache = false;
                }
            }
            else
            {
                a = _lCache.Value;
            }
            
            if (_rCache == null)
            {
                var (val, canCacheRight) = others[RHS].EvaluateWithCacheData(others);
                b = val;
                if (canCacheRight)
                {
                    _rCache = val;
                }
                else
                {
                    canCache = false;
                }
            }
            else
            {
                b = _rCache.Value;
            }
            
            return new Tuple<long, bool>(ExecuteOperation(a, b), canCache);
        }
    }
}