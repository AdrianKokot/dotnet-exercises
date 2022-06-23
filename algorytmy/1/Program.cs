IEnumerable<string> subset(string str)
{
  if (str.Length == 1)
  {
    return new[] { str };
  }

  var temp = subset(str[1..]);
  return new[] { str[..1] }.Concat(temp)
                           .Concat(temp.Select(x => str[0] + x))
                           .Distinct();
}


Console.WriteLine(
  args.Length == 0
    ? "Missing argument."
    : String.Join("\n", subset(args[0]).OrderBy(x => x.Length))
);