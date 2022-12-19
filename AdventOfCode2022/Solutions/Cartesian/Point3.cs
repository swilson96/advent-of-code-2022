namespace AdventOfCode2022.Solutions.Cartesian;

public class Point3
{
    public static Point3 Origin => new(0, 0, 0);
    
    public int X { get; }
    public int Y { get; }
    public int Z { get; }
    
    public Point3(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public HashSet<Point3> Neighbours => new (Enum.GetValues<Direction3>().Select(d => Add(d, 1)));

    public Point3 Add(Direction3 direction, int distance) => direction switch
    {
        Direction3.R => new (X + distance, Y, Z),
        Direction3.L => new (X - distance, Y, Z),
        Direction3.U => new (X, Y + distance, Z),
        Direction3.D => new (X, Y - distance, Z),
        Direction3.F => new (X, Y, Z + distance),
        Direction3.B => new (X, Y, Z - distance),
        _ => throw new ArgumentException("don't know this 3D direction " + direction)
    };
    
    public Point3 Add(Point3 other) => new(X + other.X, Y + other.Y, Z + other.Z);

    public Point3 Subtract(Point3 other) => new(X - other.X, Y - other.Y, Z - other.Z);

    public double Modulus => Math.Sqrt(X * X + Y * Y + Z * Z);

    public override bool Equals(object? obj) => Equals(obj as Point3);

    public bool Equals(Point3? other) => other != null && other.X == X && other.Y == Y && other.Z == Z;

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public override string ToString() => $"Point3({X}, {Y}, {Z})";
}