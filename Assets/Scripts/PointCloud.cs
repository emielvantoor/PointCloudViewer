using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointCloud: IEnumerable<Point>, IReadOnlyList<Point>
{
    private readonly List<Point> _points = new List<Point>();

    public float Width => GetDistance(p => p.Location.x);
    public float Length => GetDistance(p => p.Location.y);
    public float Height => GetDistance(p => p.Location.z);

    public int Count => _points.Count;

    public Point this[int index] => _points[index];

    public PointCloud(IEnumerable<Point> points)
    {
        _points.AddRange(points);
    }

    public IEnumerator<Point> GetEnumerator()
    {
        return _points.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private float GetDistance(Func<Point, float> selector)
    {
        var min = _points.Min(selector);
        var max = _points.Max(selector);
        return Mathf.Abs(max - min);
    }
}