using System.Text.RegularExpressions;

if (args.Length <= 0)
{
  Console.WriteLine("Missing argument.");
  return;
}

var str = args[0];

if(Regex.IsMatch(str, "^[A-Ż][a-ż]*( ([A-Źa-ż]+|[0-9]+)[,;]?)*[?!.]$")) {
  Console.ForegroundColor = ConsoleColor.Green;
  Console.WriteLine("The given string is a valid sentence.");
} else {
  Console.ForegroundColor = ConsoleColor.Red;
  Console.WriteLine("The given string is not a valid sentence.");
}