
using System.Reflection.Emit;
using System.Text.RegularExpressions;

string readArg(string message, string errorMessage, int argsIndex)
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
  string filePath = readArg("Input route request (from, to, max_transfers): ", "Missing argument.", 0),
         routeRequest = readArg("Input file path: ", "File doesn't exist.", 1);

  var fileContent = File.ReadAllText(filePath);
  fileContent = Regex.Replace(fileContent, @"[\[\]'\(\n\r]", "", RegexOptions.Multiline);

  var connections = Regex.Split(fileContent, @"\)\w*,")
    .Where(x => x.Length > 0)
    .Select(x => Regex.Split(x, ",").Select(x => x.Trim()).ToArray())
    .Select(arr => new Connection(start: arr[0], end: arr[1], price: double.Parse(arr[2])));

  var tempArr = Regex.Split(Regex.Replace(routeRequest, @"[\(\)]", ""), @",\w*").Select(x => x.Trim()).ToArray();
  var requestInput = new RoteRequest(from: tempArr[0], to: tempArr[1], maxTransfers: int.Parse(tempArr[2]));

  if (requestInput.maxTransfers < 0)
  {
    throw new Exception("Wrong arguments.");
  }

  Console.WriteLine(findShortestPath(connections, requestInput));
}
catch (Exception e)
{
  Console.WriteLine(e.Message);
}

string findShortestPath(IEnumerable<Connection> connections, RoteRequest requestInput)
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

  var matrix = stations.Select(x => (name: x.Key, wasChecked: false, minCost: double.PositiveInfinity, prevStationId: -1, pathCount: 0))
                       .ToArray();

  matrix[0].minCost = 0;

  for (var i = 0; i < matrix.Length; i++)
  {
    var minCost = matrix.Min(x => x.wasChecked ? double.PositiveInfinity : x.minCost);
    var vertexId = Array.FindIndex(matrix, x => !x.wasChecked && x.minCost == minCost);

    matrix[vertexId].wasChecked = true;
    var neighbours =  groupedConnections.GetValueOrDefault(matrix[vertexId].name);

    if (matrix[vertexId].pathCount >= requestInput.maxTransfers || neighbours is null)
    {
      continue;
    }

    foreach (var neighbour in neighbours)
    {
      var neighbourId = stations[neighbour.name];
      var neighbourCost = matrix[vertexId].minCost + neighbour.price;

      if (neighbourCost < matrix[neighbourId].minCost)
      {
        matrix[neighbourId].minCost = neighbourCost;
        matrix[neighbourId].prevStationId = vertexId;
        matrix[neighbourId].pathCount = matrix[vertexId].pathCount + 1;
      }
    }
  }

  var resultVertexId = stations[requestInput.to];

  var resultPrice = matrix[resultVertexId].minCost;
  var resultPath = matrix[resultVertexId].name;

  if (resultPrice == double.PositiveInfinity)
  {
    throw new Exception("Route doesn't exist.");
  }

  while ((resultVertexId = matrix[resultVertexId].prevStationId) != -1)
  {
    resultPath = matrix[resultVertexId].name + " -> " + resultPath;
  }

  return resultPath + "\nPrice: " + resultPrice;
}

record struct RoteRequest(string from, string to, int maxTransfers);

record struct Connection(string start, string end, double price);