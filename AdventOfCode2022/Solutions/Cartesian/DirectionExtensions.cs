namespace AdventOfCode2022.Solutions.Cartesian;

public static class DirectionExtensions
{
    public static Direction Opposite(this Direction direction) => direction switch
    {
        Direction.U => Direction.D,
        Direction.R => Direction.L,
        Direction.D => Direction.U,
        Direction.L => Direction.R,
        _ => throw new ArgumentOutOfRangeException($"No known direction {direction}")
    };
}