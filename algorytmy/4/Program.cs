using System.Text.RegularExpressions;
using System.Globalization;

if (args.Length <= 0)
{
  Console.WriteLine("Missing argument.");
  return;
}

try
{
  var input = args[0];
  var parsed = double.Parse(input, NumberStyles.Any, CultureInfo.InvariantCulture);
  Console.WriteLine(parsed.ToString().Where(char.IsDigit).Count());
}
catch (Exception)
{
  Console.WriteLine("The given argument is not a number.");
}