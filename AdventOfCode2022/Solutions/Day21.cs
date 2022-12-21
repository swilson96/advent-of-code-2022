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

    public object PartTwo(string input) => 0;

    private abstract class Monkey
    {
        public string Id { get; }

        protected Monkey(string id)
        {
            Id = id;
        }

        public abstract long Evaluate(Dictionary<string, Monkey> others);
    }

    private class Yeller : Monkey
    {
        private readonly long _value;
        
        public Yeller(string id, long value) : base(id)
        {
            _value = value;
        }

        public override long Evaluate(Dictionary<string, Monkey> others) => _value;
    }

    private class Calculator : Monkey
    {
        private readonly string _a;
        private readonly string _b;
        private readonly string _operation;
        
        public Calculator(string id, string a, string b, string operation) : base(id)
        {
            _a = a;
            _b = b;
            _operation = operation;
        }

        public override long Evaluate(Dictionary<string, Monkey> others)
        {
            var a = others[_a].Evaluate(others);
            var b = others[_b].Evaluate(others);
            return _operation switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => a / b,
                _ => throw new AggregateException($"Unknown monkey operation {_operation}")
            };
        }
    }
}