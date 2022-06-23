IEnumerable<string> subset(string str)
  => (str.Length switch
  {
    1 => new[] { str },
    2 => new[] { str[..1], str[1..], str },
    _ => new[] { str[..1] }
                  .Concat(
                    subset(str[1..]).SelectMany(x => new[] { x, str[0] + x })
                  )
  }).OrderBy(x => x.Length);

Console.WriteLine(
  args.Length == 0
    ? "Missing argument."
    : String.Join("\n", subset(args[0]))
);