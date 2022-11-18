using AdventOfCode2022.Advent;

namespace AdventOfCode2022.Tests.Advent;

public class CustomDateProvider : IDateProvider
{
    private readonly DateOnly _customDate;
    
    public CustomDateProvider(DateOnly customDate)
    {
        _customDate = customDate;
    }

    public DateOnly GetCurrentDate() => _customDate;
}