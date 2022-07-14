using System.Linq;

if (args.Length <= 0)
{
  Console.WriteLine("Missing arguments.");
  return;
}

var anagrams = args.GroupBy(str => string.Concat(str.ToLower().OrderBy(c => c)));
var formattedAnagramGroups = anagrams.Select(x => string.Join(", ", x));

Console.WriteLine(string.Join("\n", formattedAnagramGroups));