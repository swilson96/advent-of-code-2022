namespace AdventOfCode2022.Advent;

public class AdventCountdown
{
    private readonly IDateProvider _dateProvider;
    
    public AdventCountdown(IDateProvider dateProvider)
    {
        _dateProvider = dateProvider;
    }
    
    public Boolean IsAdventNow()
    {
        DateOnly today = _dateProvider.GetCurrentDate();
        return today >= new DateOnly(2022, 12, 1) && today <= new DateOnly(2022, 12, 25);
    }

    public int DayOfAdvent() => IsAdventNow() ? _dateProvider.GetCurrentDate().Day : 0;
}