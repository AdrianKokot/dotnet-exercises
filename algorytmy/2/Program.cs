using System;
using System.Text.RegularExpressions;

if (args.Length <= 0)
{
  Console.WriteLine("Missing argument.");
  return;
}

var str = args[0].Substring(2, args[0].Length - 4);


var intervals = Regex.Split(str, @"\),.?\(")
                     .Select(x => x.Split(',').Select(x => Int16.Parse(x)).ToArray());

var points = intervals.SelectMany(x => x).Distinct().OrderBy(x => x).ToArray();
var quantities = Enumerable.Repeat(0, points.Length).ToArray();

foreach (var interval in intervals)
{
  var startIdx = Array.IndexOf(points, interval[0]);
  while (startIdx < points.Length && points[startIdx] <= interval[1])
    quantities[startIdx++]++;
}

Console.WriteLine(quantities.Max());