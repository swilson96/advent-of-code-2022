namespace AdventOfCode2022.Solutions;

public class Day13 : IAdventSolution
{
    public object PartOne(string input) => input.Split(Environment.NewLine + Environment.NewLine)
        .Select(pair => pair.Split(Environment.NewLine))
        .Select((pair, i) => Compare(pair[0], pair[1]) <= 0 ? i + 1 : 0)
        .Sum();

    // a < b : -1
    // a == b : 0
    // a > b : 1
    public int Compare(string a, string b)
    {
        var al = SplitList(a);
        using var bli = SplitList(b).GetEnumerator();
        
        foreach (var an in al)
        {
            if (!bli.MoveNext())
            {
                return 1;
            }

            var bn = bli.Current;

            if (an.StartsWith("[") || bn.StartsWith("["))
            {
                var ac = an;
                var bc = bn;
                
                if (!an.StartsWith("["))
                {
                    if (string.IsNullOrEmpty(an))
                    {
                        return -1;
                    }
                    ac = an.StartsWith("[") ? an : "[" + an + "]";
                }
                if (!bn.StartsWith("["))
                {
                    bc = bn.StartsWith("[") ? bn : "[" + bn + "]";
                    if (string.IsNullOrEmpty(bn))
                    {
                        return 1;
                    }
                }
                
                var result = Compare(ac, bc);

                if (result != 0)
                {
                    return result;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(an) || string.IsNullOrEmpty(bn))
                {
                    return string.Compare(an, bn, StringComparison.Ordinal);
                }
                var result = int.Parse(an).CompareTo(int.Parse(bn));
                if (result != 0)
                {
                    return result;
                }
            }
        }
        
        return bli.MoveNext() ? -1 : 0;
    }

    public IEnumerable<string> SplitList(string input)
    {
        var level = 0;

        var current = new List<char>();
        foreach (var next in input.ToCharArray())
        {
            switch (next)
            {
                case '[':
                    ++level;
                    if (level > 1)
                    {
                        current.Add(next);
                    }
                    break;
                case ']':
                    --level;
                    if (level == 0)
                    {
                        yield return string.Join("", current);
                    }
                    else
                    {
                        current.Add(next);
                    }
                    break;
                case ',':
                    if (level == 1)
                    {
                        yield return string.Join("", current);
                        current.Clear();
                    }
                    else
                    {
                        current.Add(next);
                    }
                    break;
                default:
                    current.Add(next);
                    break;
            }
        }
    }

    public object PartTwo(string input)
    {
        var packets = input.Split(Environment.NewLine).Where(s => !string.IsNullOrEmpty(s)).ToList();
        packets.Add("[[2]]");
        packets.Add("[[6]]");
        packets.Sort(Compare);
        return (packets.IndexOf("[[2]]") + 1) * (packets.IndexOf("[[6]]") + 1);
    }
}