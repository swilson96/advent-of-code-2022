namespace AdventOfCode2022.Solutions;

public class Day06 : IAdventSolution
{
    public object PartOne(string input) => PositionOfFirstMarker(input, 4);
    
    private int PositionOfFirstMarker(string input, int markerLength)
    {
        var position = markerLength;
        while (position < input.Length)
        {
            if (IsMarker(input[(position-markerLength)..position], markerLength))
            {
                return position;
            }
            ++position;
        }

        throw new ArgumentException("no marker in input");
    }

    private bool IsMarker(string candidate, int length)
    {
        var uniqueChars = new HashSet<char>(candidate.ToCharArray());
        return uniqueChars.Count == length;
    }

    public object PartTwo(string input) => PositionOfFirstMarker(input, 14);
}