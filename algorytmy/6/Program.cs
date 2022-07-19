
using System.Text.RegularExpressions;

string askForInputIfArgDoesntExist(string message, string errorMessage, int argsIndex)
{
  string? input = args.Length > argsIndex ? args[argsIndex] : null;

  if (input is null)
  {
    Console.Write(message);
    input = Console.ReadLine();
  }

  if (input is null)
  {
    throw new Exception(errorMessage);
  }

  return input;
}

try
{
  string filePath = askForInputIfArgDoesntExist("Input file path: ", "File doesn't exist.", 0);

  var fileContent = File.ReadAllText(filePath);
  fileContent = Regex.Replace(fileContent, @"[\[\]'\(\n\r]", "", RegexOptions.Multiline);

  var connections = Regex.Split(fileContent, @"\)\w*,")
    .Where(x => x.Length > 0)
    .Select(x => Regex.Split(x, ",").Select(x => x.Trim()).ToArray())
    .Select(arr => (start: arr[0], end: arr[1], price: double.Parse(arr[2])));

  var routeRequest = askForInputIfArgDoesntExist("Input file path: ", "File doesn't exist.", 1);

  var tempArr = Regex.Split(Regex.Replace(routeRequest, @"[\(\)]", ""), @",\w*").Select(x => x.Trim()).ToArray();
  var requestInput = (from: tempArr[0], to: tempArr[1], maxTransfers: int.Parse(tempArr[2]));

  if (requestInput.maxTransfers < 0)
  {
    throw new Exception("Wrong arguments.");
  }

  var result = findShortestPath(connections, requestInput);

  Console.WriteLine(String.Join(" -> ", result.path));
  Console.WriteLine(result.price);
}
catch (Exception e)
{
  Console.WriteLine(e.Message);
}

(IEnumerable<string> path, double price) findShortestPath(IEnumerable<(string start, string end, double price)> connections, (string from, string to, int maxTransfers) requestInput)
{
  var dictId = 0;
  var stations = connections.SelectMany(x => new string[] { x.start, x.end })
                            .Distinct()
                            .OrderBy(x => x != requestInput.from)
                            .ToDictionary(x => x, _ => dictId++);

  if (!stations.ContainsKey(requestInput.from) || !stations.ContainsKey(requestInput.to))
  {
    throw new Exception("The given station doesn't exist.");
  }

  var groupedConnections = connections.GroupBy(x => x.start)
                                      .ToDictionary(x => x.Key, x => x.Select(y => (name: y.end, price: y.price)));

  var matrix = stations.Select(x => (id: x.Value, name: x.Key, wasChecked: false, minCost: double.PositiveInfinity, prevStationId: -1, pathCount: 0))
                       .ToArray();

  matrix[0].minCost = 0;
  double minCost = 0;

  for (var i = 0; i < matrix.Length; i++)
  {
    var vertexId = Array.FindIndex(matrix, x => !x.wasChecked && x.minCost == minCost);

    matrix[vertexId].wasChecked = true;

    var neighbours = groupedConnections.ContainsKey(matrix[vertexId].name) ? groupedConnections[matrix[vertexId].name] : null;

    if (neighbours is not null && matrix[vertexId].pathCount + 1 <= requestInput.maxTransfers)
    {
      foreach (var neighbour in neighbours)
      {
        var neighbourId = stations[neighbour.name];
        if (matrix[vertexId].minCost + neighbour.price < matrix[neighbourId].minCost)
        {
          matrix[neighbourId].minCost = matrix[vertexId].minCost + neighbour.price;
          matrix[neighbourId].prevStationId = vertexId;
          matrix[neighbourId].pathCount = matrix[vertexId].pathCount + 1;
        }
      }
    }

    minCost = matrix.Where(x => !x.wasChecked).DefaultIfEmpty().Min(x => x.minCost);
  }

  var resultPath = new List<string>();

  var resultVertexId = stations[requestInput.to];

  var result = (path: new List<string>(), price: matrix[resultVertexId].minCost);

  do
  {
    result.path.Insert(0, matrix[resultVertexId].name);
    resultVertexId = matrix[resultVertexId].prevStationId;
  } while (resultVertexId != -1);

  if (result.price == double.PositiveInfinity)
  {
    throw new Exception("Route doesn't exist.");
  }

  return result;
}