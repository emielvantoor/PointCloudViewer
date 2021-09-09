using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class PointCloudReader
{
    public PointCloud Read(string path)
    {
        var lines = File.ReadAllLines(path).ToList();
        if (!lines.Any(l => l.Contains("FIELDS x y z")))
        {
            throw new InvalidDataException("PCD file is not in expected format");
        }

        var points = new List<Point>();

        var parsingData = false;

        lines.ForEach(line =>
        {
            if (line.Contains("DATA ascii"))
            {
                parsingData = true;
                return;
            }

            if (!parsingData)
                return;

            points.Add(ParsePoint(line));
        });

        return new PointCloud(points);
    }

    private Point ParsePoint(string line)
    {
        var floats = line.Split(' ')
            .Select(c => {
                var coordinate = Regex.Replace(c, "[/,/.]", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                return float.Parse(coordinate);
            }).ToArray();
        var location = new Vector3(floats[0], floats[1], floats[2]);
        var color = Color.blue;
        return new Point(location, color);
    }

    // public void FromPointCloudToPcd(PointCloud pointCloud, string outputFile, bool addRgb = true, bool drawZeroGrid = false)
    // {
    //     if (pointCloud == null || !pointCloud.Points.Any())
    //         return;
    //
    //     var b = new StringBuilder();
    //
    //     IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    //     var zeroGrid = drawZeroGrid ? DrawGrid(addRgb, FormatProvider) : new List<string>();
    //
    //     b.AppendLine("VERSION .7");
    //     if (addRgb)
    //     {
    //         b.AppendLine("FIELDS x y z rgb");
    //         b.AppendLine("SIZE 4 4 4 4");
    //         b.AppendLine("TYPE F F F F");
    //     }
    //     else
    //     {
    //         b.AppendLine("FIELDS x y z");
    //         b.AppendLine("SIZE 4 4 4");
    //         b.AppendLine("TYPE F F F");
    //     }
    //
    //     b.AppendFormat("WIDTH {0}", 1);
    //     b.AppendLine();
    //     b.AppendFormat("HEIGHT {0}", pointCloud.Points.Count + zeroGrid.Count);
    //     b.AppendLine();
    //     b.AppendLine("VIEWPOINT 0 0 0 1 0 0 0");
    //     b.AppendFormat("POINTS {0}", pointCloud.Points.Count + zeroGrid.Count);
    //     b.AppendLine();
    //     b.Append("DATA ascii");
    //
    //     pointCloud.Points.ForEach(p =>
    //     {
    //         b.AppendLine();
    //         if (addRgb)
    //         {
    //             const int FRONT_PLANE_COLOR = 4000000;
    //             b.AppendFormat(FormatProvider, "{0} {1} {2} {3}", p.X, p.Y, p.Z, p.Rgb == 0 ? FRONT_PLANE_COLOR : p.Rgb);
    //         }
    //         else
    //         {
    //             b.AppendFormat(FormatProvider, "{0} {1} {2}", p.X, p.Y, p.Z);
    //         }
    //     });
    //
    //
    //     if (drawZeroGrid)
    //     {
    //         b.AppendLine();
    //         zeroGrid.ForEach(p => b.AppendLine(p));
    //     }
    //
    //     File.WriteAllText(outputFile, b.ToString());
    // }
    //
    // private static List<string> DrawGrid(bool addRgb, IFormatProvider FormatProvider)
    // {
    //     const int MINX = -500;
    //     const int MAXX = 500;
    //     const int MINY = -800;
    //     const int MAXY = 800;
    //     const int GRID_STEPS = 100;
    //
    //     var gridStrings = new List<string>();
    //     for (int x = MINX; x < MAXX; x += GRID_STEPS)
    //     {
    //         for (int y = MINY; y < MAXY; y += GRID_STEPS)
    //         {
    //             if (addRgb)
    //             {
    //                 const int GREEN_GRID_COLOR = 65535;
    //                 gridStrings.Add(string.Format(FormatProvider, "{0} {1} {2} {3}", x, y, 0, GREEN_GRID_COLOR));
    //             }
    //             else
    //             {
    //                 gridStrings.Add(string.Format(FormatProvider, "{0} {1} {2}", x, y, 0));
    //             }
    //         }
    //     }
    //
    //     return gridStrings;
    // }
}