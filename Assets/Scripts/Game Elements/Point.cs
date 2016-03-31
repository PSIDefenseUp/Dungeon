using System;

public class Point
{
    public int x, y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // Returns the number of spaces between this point and another
    public int distanceTo(Point other)
    {
        return Math.Abs(x - other.x) + Math.Abs(y - other.y);
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }
}
