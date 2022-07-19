using System.Text.RegularExpressions;

try
{
  string filePath = readArg("Input route request (from, to, max_transfers): ", "Missing argument.", 0);
  var routeRequest = parseRouteRequest(readArg("Input file path: ", "File doesn't exist.", 1));

  if (routeRequest.maxTransfers < 0)
  {
    throw new Exception("Wrong arguments.");
  }

  var result = findShortestPath(parseFileContent(filePath), routeRequest);
  Console.WriteLine(result);
}
catch (Exception e)
{
  Console.WriteLine(e.Message);
}

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

IEnumerable<Connection> parseFileContent(string filePath)
{
  var fileContent = File.ReadAllText(filePath);

  fileContent = Regex.Replace(fileContent, @"[\[\]'\(\n\r]", "", RegexOptions.Multiline);

  return Regex.Split(fileContent, @"\)\w*,")
              .Where(x => x.Length > 0)
              .Select(x => Regex.Split(x, ",").Select(x => x.Trim()).ToArray())
              .Select(arr => new Connection(start: arr[0], end: arr[1], price: double.Parse(arr[2])));
}

RouteRequest parseRouteRequest(string input)
{
  var tempArr = Regex.Split(Regex.Replace(input, @"[\(\)]", ""), @",\w*").Select(x => x.Trim()).ToArray();
  return new RouteRequest(from: tempArr[0], to: tempArr[1], maxTransfers: int.Parse(tempArr[2]));
}

string findShortestPath(in IEnumerable<Connection> connections, RouteRequest requestInput)
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

  var stationNeighbours = connections.GroupBy(x => x.start)
                                      .ToDictionary(x => x.Key, x => x.Select(y => (name: y.end, price: y.price)));

  var matrix = stations.Select(x => (name: x.Key, visited: false, routeCost: double.PositiveInfinity, prevVertexId: -1, transfers: 0))
                       .ToArray();

  matrix[0].routeCost = 0;

  for (var i = 0; i < matrix.Length; i++)
  {
    var minRouteCost = matrix.Min(x => x.visited ? double.PositiveInfinity : x.routeCost);
    var vertexId = Array.FindIndex(matrix, x => !x.visited && x.routeCost == minRouteCost);

    matrix[vertexId].visited = true;
    var neighbours = stationNeighbours.GetValueOrDefault(matrix[vertexId].name);

    if (matrix[vertexId].transfers >= requestInput.maxTransfers || neighbours is null)
    {
      continue;
    }

    foreach (var neighbour in neighbours)
    {
      var neighbourId = stations[neighbour.name];
      var neighbourCost = matrix[vertexId].routeCost + neighbour.price;

      if (neighbourCost < matrix[neighbourId].routeCost)
      {
        matrix[neighbourId].routeCost = neighbourCost;
        matrix[neighbourId].prevVertexId = vertexId;
        matrix[neighbourId].transfers = matrix[vertexId].transfers + 1;
      }
    }
  }

  var resultVertexId = stations[requestInput.to];

  var resultPrice = matrix[resultVertexId].routeCost;
  var resultPath = matrix[resultVertexId].name;

  if (resultPrice == double.PositiveInfinity)
  {
    throw new Exception("Route doesn't exist.");
  }

  while ((resultVertexId = matrix[resultVertexId].prevVertexId) != -1)
  {
    resultPath = matrix[resultVertexId].name + " -> " + resultPath;
  }

  return resultPath + "\nPrice: " + resultPrice;
}

record struct RouteRequest(string from, string to, int maxTransfers);

record struct Connection(string start, string end, double price);