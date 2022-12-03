namespace AdventOfCode2022.Solutions;

public class Day02 : IAdventSolution
{
    public int PartOne(string input) => input.Split(Environment.NewLine)
        .Select(ScoreRound)
        .Sum();

    private int ScoreRound(string round) => round switch
    {
        "A X" => 4, // rock rock
        "A Y" => 8,
        "A Z" => 3,
        "B X" => 1,
        "B Y" => 5,
        "B Z" => 9,
        "C X" => 7, // 6 + 1
        "C Y" => 2, // 0 + 2
        "C Z" => 6, // 3 + 3
        _ => 0
    };

    public int PartTwo(string input) => input.Split(Environment.NewLine)
        .Select(ScoreRoundFromResult)
        .Sum();

    private int ScoreRoundFromResult(string round) => round switch
    {
        "A X" => 3, // rock, lose: play scissors: 0 + 3
        "A Y" => 4, // 3 + 1
        "A Z" => 8, // 6 + 2
        "B X" => 1, // 0 + 1
        "B Y" => 5, // 3 + 2
        "B Z" => 9, // 6 + 3
        "C X" => 2, // 0 + 2
        "C Y" => 6, // 3 + 3
        "C Z" => 7, // 6 + 1
        _ => 0
    };
}