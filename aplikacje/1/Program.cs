using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

try
{
  string? filePath = args.Length > 0 ? args[0] : null;

  if (filePath is null)
  {
    Console.Write("JSON file path: ");
    filePath = Console.ReadLine();
  }

  if (!File.Exists(filePath))
  {
    throw new FileNotFoundException("File doesn't exist.");
  }

  var parsedJson = parseJson(File.ReadAllText(filePath));
  Console.WriteLine(formatJson(parsedJson));
}
catch (Exception e)
{
  Console.WriteLine(e.Message);
}

IEnumerable<IDictionary<string, string>> parseJson(string jsonContent)
{
  var inputString = jsonContent.Trim().TrimStart('[').TrimEnd(']');

  if (Regex.IsMatch(inputString, "\\[|\\]|(\"\\s*:\\s*{)", RegexOptions.Multiline))
  {
    throw new Exception("The file content is not a valid JSON.");
  }

  inputString = Regex.Replace(inputString, "{|}[^,]|\"", "", RegexOptions.Multiline);

  return Regex.Split(inputString, "},")
              .Select(jsonObj =>
                 Regex.Split(jsonObj, ",")
                      .Select(objectField => objectField.Split(":"))
                      .ToDictionary(x => x[0].Trim(), x => x[1].Trim())
              );
}

string formatJson(in IEnumerable<IDictionary<string, string>> objects)
{
  const int fieldMargin = 2;

  var columns = objects.SelectMany(x => x)
                      .GroupBy(x => x.Key, x => x.Value.Length)
                      .ToDictionary(x => x.Key, x => Math.Max(x.Key.Length, x.Max()) + fieldMargin);

  var divider = new String('-', columns.Values.Count() + 1 + columns.Values.Sum());

  return objects.Select(obj => columns.Keys.Select(column => obj.GetValue(column, "-")))
          .Prepend(columns.Keys.Select(column => column.ToUpper()))
          .Select(row => row.Select((col, i) => col.PadCenter(columns.ElementAt(i).Value)))
          .Select(x => $"|{x.ToJoinedString("|")}|\n{divider}")
          .ToJoinedString("\n")
          .Insert(0, $"{divider}\n");
}

public static class Extensions
{
  public static string PadCenter(this String str, int length)
    => str.PadLeft((length - str.Length) / 2 + str.Length).PadRight(length);

  public static string ToJoinedString(this IEnumerable<String> enumerable, string joiner)
    => String.Join(joiner, enumerable);

  public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue whenNull)
    => dict.ContainsKey(key) ? dict[key] : whenNull;
}