if (args.Length <= 0)
{
  Console.WriteLine("Missing argument.");
  return;
}

try
{
  Console.WriteLine(Math.Abs(double.Parse(args[0])).ToString().Length);
}
catch (Exception)
{
  Console.WriteLine("The given argument is not a number.");
}