using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode2022.Solutions;

public class Day05 : IAdventSolution
{
    public object PartOne(string input)
    {
        var inputBits = input.Split(Environment.NewLine + Environment.NewLine);
        var ship = new Ship(inputBits[0], new PartsMover9000());
        inputBits[1].Split(Environment.NewLine).ForEach(ship.ExecuteMove);
        return ship.TopsOfStacks;
    }

    public object PartTwo(string input)
    {
        var inputBits = input.Split(Environment.NewLine + Environment.NewLine);
        var ship = new Ship(inputBits[0], new PartsMover9001()); // Upgrade!
        inputBits[1].Split(Environment.NewLine).ForEach(ship.ExecuteMove);
        return ship.TopsOfStacks;
    }

    private class Ship
    {
        private readonly Regex _moveRegex = new Regex(@"move (\d+) from (.) to (.)");
        
        private readonly Dictionary<char, Stack<char>> _stacks = new();
        private readonly ICrane _crane;
        
        public Ship(string input, ICrane crane)
        {
            _crane = crane;
            
            using var lineEnumerator = input.Split(Environment.NewLine).Reverse().GetEnumerator();
            lineEnumerator.MoveNext();
            lineEnumerator.Current.ToCharArray().Where(c => c != ' ').ForEach(c => _stacks.Add(c, new Stack<char>()));
            while (lineEnumerator.MoveNext())
            {
                lineEnumerator.Current.Batch(4)
                    .Select(s => s.ToList()[1])
                    .Zip(_stacks.Keys)
                    .Where(zipped => zipped.First != ' ')
                    .ForEach(zipped => _stacks[zipped.Second].Push(zipped.First));
            }
        }

        public void ExecuteMove(string move)
        {
            var match = _moveRegex.Match(move);
            var toMove = int.Parse(match.Groups[1].Value);
            var from = match.Groups[2].Value[0];
            var to = match.Groups[3].Value[0];

            var stackFrom = _stacks[from];
            var stackTo = _stacks[to];

            _crane.Move(toMove, stackFrom, stackTo);
        }

        public string TopsOfStacks => string.Join("", _stacks.Values.Select(s => s.Peek()));
    }

    private interface ICrane
    {
        void Move(int toMove, Stack<char> stackFrom, Stack<char> stackTo);
    }

    private class PartsMover9000 : ICrane
    {
        public void Move(int leftToMove, Stack<char> stackFrom, Stack<char> stackTo)
        {
            while (leftToMove > 0)
            {
                stackTo.Push(stackFrom.Pop());
                --leftToMove;
            }
        }
    }
    
    private class PartsMover9001 : ICrane
    {
        public void Move(int leftToMove, Stack<char> stackFrom, Stack<char> stackTo)
        {
            var crates = new Stack<char>();
            while (leftToMove > 0)
            {
                crates.Push(stackFrom.Pop());
                --leftToMove;
            }

            while (crates.Count > 0)
            {
                stackTo.Push(crates.Pop());
            }
        }
    }
}