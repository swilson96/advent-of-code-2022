namespace AdventOfCode2022.Solutions.Cartesian;

public class Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point Add(Direction direction, int distance) => direction switch
    {
        Direction.U => new Point(X, Y + distance),
        Direction.R => new Point(X + distance, Y),
        Direction.D => new Point(X, Y - distance),
        Direction.L => new Point(X - distance, Y),
        _ => throw new ArgumentOutOfRangeException($"Direction {direction} unexpected")
    };

    public Point Add(Point other) => new (X + other.X, Y + other.Y);

    public Point Subtract(Point other) => new(X - other.X, Y - other.Y);

    public static Point Origin => new (0, 0);

    public override bool Equals(object? obj)
    {
        return Equals(obj as Point);
    }

    public bool Equals(Point? other)
    {
        return other != null && other.X == X && other.Y == Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString() => $"Point({X}, {Y})";
}