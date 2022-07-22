using System.Text.RegularExpressions;

if (args.Length <= 0)
{
  Console.WriteLine("Missing argument.");
  return;
}

var arr = Regex.Split(Regex.Replace(args[0], @"\[|\]", ""), ",").Select(x => int.Parse(x.Trim())).ToArray();
var added = arr.Select(_ => false).ToArray();

var sum = arr.Max((x) => x);

var maxId = Array.FindIndex(arr, x => x == sum);
added[maxId] = true;

for (var i = 0; i < arr.Length; i++)
{
  if (!added[i] && sum + arr[i] > sum)
  {
    sum += arr[i];
    added[i] = true;
  }
}

Console.WriteLine("[" + String.Join(", ", arr.Where((_, i) => added[i])) + "]");
Console.WriteLine($"Sum: {sum}");
