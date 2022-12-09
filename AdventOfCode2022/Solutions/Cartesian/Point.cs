namespace AdventOfCode2022.Solutions.Cartesian;

public class Vector
{
    public int X { get; }
    public int Y { get; }

    public Vector(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Vector Add(Direction direction, int distance) => direction switch
    {
        Direction.U => new Vector(X, Y + distance),
        Direction.R => new Vector(X + distance, Y),
        Direction.D => new Vector(X, Y - distance),
        Direction.L => new Vector(X - distance, Y),
        _ => throw new ArgumentOutOfRangeException($"Direction {direction} unexpected")
    };

    public Vector Add(Vector other) => new (this.X + other.X, this.Y + other.Y);

    public static Vector Origin => new (0, 0);

    public override bool Equals(object? obj)
    {
        return Equals(obj as Vector);
    }

    public bool Equals(Vector? other)
    {
        return other != null && other.X == X && other.Y == Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}