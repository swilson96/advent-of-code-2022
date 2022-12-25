using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions;

public class Day25 : IAdventSolution
{ 
    public object PartOne(string input) => new Snafu(input.Split(Environment.NewLine)
            .Select(Snafu.Parse)
            .Select(s => (long)s)
            .Sum())
        .ToString();

    public class Snafu
    {
        private readonly long _value;

        public Snafu(long value)
        {
            _value = value;
        }

        public static Snafu Parse(string input)
        {
            var val = 0L;
            var power = 1L;
            foreach (var c in input.ToCharArray().Reverse())
            {
                var charVal = c switch
                {
                    '2' => 2,
                    '1' => 1,
                    '0' => 0,
                    '-' => -1,
                    '=' => -2,
                    _ => throw new ArgumentOutOfRangeException($"snafu char {c} not recognised")
                };
                val += charVal * power;
                power *= 5;
            }

            return new Snafu(val);
        }

        public override string ToString()
        {
            var chars = new List<char>();
            var tail = _value;
            while (tail > 0) // negative not yet supported
            {
                var remainder = tail % 5;
                var c = remainder switch
                {
                    2 => '2',
                    1 => '1',
                    0 => '0',
                    4 => '-',
                    3 => '=',
                    _ => throw new ArgumentOutOfRangeException($"remainder of 5 was {remainder} wot")
                };
                
                chars.Add(c);
                tail /= 5;
                
                if (remainder > 2)
                {
                    ++tail;
                }
            }

            chars.Reverse();
            return string.Join("", chars);
        }
        
        public static implicit operator long(Snafu s) => s._value;
        public static explicit operator Snafu(long l) => new (l);
    }

    public object PartTwo(string input) => "Merry Christmas!";
}