using System.Text.RegularExpressions;

try
{
  string? filePath = args.Length > 0 ? args[0] : "./example.json";

  if (filePath is null)
  {
    Console.Write("JSON file path: ");
    filePath = Console.ReadLine();
  }

  if (!File.Exists(filePath))
  {
    throw new FileNotFoundException("File doesn't exist.");
  }

  var formatter = new Formatter(parseJson(File.ReadAllText(filePath)));

  Console.WriteLine(formatter.Format());
}
catch (Exception e)
{
  Console.WriteLine(e.Message + " " + e.StackTrace);
}

IDictionary<string, string[]> parseJson(string json)
{
  var inputString = json.Trim().TrimStart('[').TrimEnd(']');

  if (Regex.IsMatch(inputString, "\\[|\\]|(\"\\s*:\\s*{)", RegexOptions.Multiline))
  {
    throw new Exception("The file content is not a valid JSON.");
  }

  inputString = Regex.Replace(inputString, "{|}[^,]|\"", "", RegexOptions.Multiline);

  var objects = Regex.Split(inputString, "},")
                     .Select(jsonObj =>
                       Regex.Split(jsonObj, ",")
                            .Select(objectField =>
                            {
                              var pair = objectField.Split(":");
                              return (Key: pair[0].Trim(), Value: pair[1].Trim());
                            })
                     );

  var dict = objects.SelectMany(x => x.Select(y => y.Key))
                    .Distinct()
                    .ToDictionary(x => x, (_) => new List<string>());

  for (var i = 0; i < objects.Count(); i++)
  {
    foreach (var field in objects.ElementAt(i))
    {
      var dictLen = dict[field.Key].Count();

      if (dictLen < i)
      {
        dict[field.Key].AddRange(Enumerable.Range(0, i - dictLen).Select(x => "-"));
      }

      dict[field.Key].Add(field.Value);
    }
  }

  return dict.ToDictionary(x => x.Key, x => x.Value.ToArray());
}

public class Formatter
{
  private readonly int fieldMargin = 2;
  private readonly IEnumerable<IEnumerable<string>> preparedJson;
  private readonly int[] columnLengths;
  private readonly string divider;

  private string centerString(string text, int length)
    => text.PadLeft((length - text.Length) / 2 + text.Length).PadRight(length);

  private string wrapRowWithDividers(IEnumerable<string> arr)
    => $"|{String.Join("|", arr)}|\n{divider}";

  private string formatRow(IEnumerable<string> arr)
    => wrapRowWithDividers(arr.Select((str, i) => centerString(str, columnLengths[i])));

  private IEnumerable<IEnumerable<string>> prepareJsonForFormatting(in IDictionary<string, string[]> json)
  {
    var lists = new List<List<string>>();

    var colCount = json.Keys.Count();
    var rowCount = json.Values.Max(x => x.Count());

    for (var i = 0; i < rowCount; i++)
    {
      lists.Add(new List<string>());
      for (var j = 0; j < colCount; j++)
      {
        lists[i].Add(json.Values.ElementAt(j).ElementAt(i));
      }
    }

    return lists.Prepend(json.Keys.Select(str => str.ToUpper()));
  }
  public Formatter(in IDictionary<string, string[]> json)
  {
    this.preparedJson = prepareJsonForFormatting(json);

    this.columnLengths =
        json.Select(x => Math.Max(x.Key.Length, x.Value.Max(entry => entry.Length)) + fieldMargin).ToArray();

    var dividerLength = this.columnLengths.Count() + 1 + this.columnLengths.Sum();
    this.divider = new String('-', dividerLength);
  }

  public string Format() => divider + "\n" + String.Join("\n", preparedJson.Select(x => formatRow(x)));
}