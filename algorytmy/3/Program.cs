var rotatedNumbers = new Dictionary<char, char> { { '0', '0' }, { '1', '1' }, { '6', '9' }, { '8', '8' }, { '9', '6' } };

bool isPrime(long n)
{
  for (int i = 2; i * i <= n; i++)
  {
    if (n % i == 0)
    {
      return false;
    }
  }

  return true;
}

bool isStrobogrammatic(string str)
{
  for (var i = 0; i < str.Length / 2; i++)
  {
    if (!rotatedNumbers.ContainsKey(str[i]) || rotatedNumbers[str[i]] != str[str.Length - i - 1])
    {
      return false;
    }
  }

  return true;
}

if (args.Length <= 0)
{
  Console.WriteLine("Missing argument.");
  return;
}

var input = args[0];

if(!input.All(x => Char.IsNumber(x))) {
  Console.WriteLine("Input must be a positive number");
  return;
}

string resultMessage = "n't strobogrammatic";

if (isStrobogrammatic(input))
{
  resultMessage = " strobogrammatic";

  try
  {
    resultMessage += isPrime(long.Parse(input)) ? " and prime" : "";
  }
  catch (Exception)
  {
    resultMessage += " and is too big to check whether is prime";
  }
}


Console.WriteLine("The given number is" + resultMessage);