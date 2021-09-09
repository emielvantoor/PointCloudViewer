using System.Linq.Expressions;
using UnityEngine;

public struct Point
{
    public Vector3 Location { get; }
    public Color Color { get; }

    public Point(Vector3 location)
        : this(location, Color.green)
    {

    }

    public Point(Vector3 location, Color color)
    {
        Location = location;
        Color = color;
    }
}