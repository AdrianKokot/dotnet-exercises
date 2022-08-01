using System.Text.RegularExpressions;

if (args.Length <= 0)
{
  Console.WriteLine("Missing argument.");
  return;
}

var str = args[0];

var regexes = new (string pattern, bool expectedResult)[] {
    (@"[^a-zA-Z0-9?!;,\. ]", false),
    (@" [,;]", false),
    (@"[,;]([^ ]|$)", false),
    (@"[^ ][?!\.]$", true),
    (@"^[A-Z][a-z ]", true),
    (@"  +", false),
    (@"[a-zA-Z][0-9]", false),
    (@"[0-9][a-zA-Z]", false)
  };

if (regexes.Any(reg => Regex.IsMatch(str, reg.pattern) != reg.expectedResult))
{
  Console.ForegroundColor = ConsoleColor.Red;
  Console.WriteLine("The given string is not a valid sentence.");
}
else
{
  Console.ForegroundColor = ConsoleColor.Green;
  Console.WriteLine("The given string is a valid sentence.");
}
