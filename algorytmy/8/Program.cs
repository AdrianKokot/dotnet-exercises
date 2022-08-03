using System.Text.RegularExpressions;

if (args.Length <= 0)
{
  Console.WriteLine("Missing argument.");
  return;
}

var valid = Regex.IsMatch(args[0], @"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*( ((?i)[a-ząćęłńóśźż]+|[0-9]+)[,;]?)*[?!.]$");

Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
Console.WriteLine("The given string is " + (valid ? "" : "not ") + "a valid sentence.");