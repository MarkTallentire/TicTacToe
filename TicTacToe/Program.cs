//Draw the board
//Ask the players for a choice
//Check if anyone has won (3 in a row) > Complete Game > Next Round

var game = new GameManager();
game.InitialiseGame();

public class GameManager()
{
    private Board _board = new();
    private Player _playerOne;
    private Player _playerTwo;
    
    public void InitialiseGame()
    {
        Console.Write("Player 1 Name: ");
        _playerOne = new Player(Console.ReadLine(), XO.X);
        Console.Write("Player 2 Name: ");
        _playerTwo = new Player(Console.ReadLine(), XO.O);

        RunGame();
    }

    private void RunGame()
    {
        _board.Render();
        while(true)
        {
            bool moveIsValid = false; 
            while(!moveIsValid)
            {
                var p1Move = _playerOne.GetMove();
                moveIsValid = _board.ProcessMove(p1Move.Item1, p1Move.Item2, _playerOne.Xo);
            }
            _board.Render();
            IsFinished();
           
            bool moveIsValidP2 = false;
            while (!moveIsValidP2)
            {
                var p2Move = _playerTwo.GetMove();
                moveIsValidP2 = _board.ProcessMove(p2Move.Item1, p2Move.Item2, _playerTwo.Xo);
            }
            _board.Render();
            IsFinished();
        }
    }

    private void IsFinished()
    {
        var isWinner = _board.CheckForWinner();
        if(isWinner is not null)
        {
            Console.WriteLine($"{isWinner.ToString()} wins");
            Environment.Exit(0);
        }

        if (_board.IsDraw())
        {
            Console.WriteLine("Draw");
            Environment.Exit(0);
        }
    }
}

public class Board()
{
    private XO?[,] _spaces = new XO?[3,3];
    
    public void Render()
    {
        Console.Clear();
        for(int y = 0; y < _spaces.GetLength(0); y++)
        {
            for (int x = 0; x < _spaces.GetLength(1); x++)
            {
               
                Console.Write(_spaces[y, x].ToString());
                if (x < _spaces.GetLength(1) - 1)
                {
                    Console.Write(",");
                }
            }
            
            Console.WriteLine();
        }
        
        Console.WriteLine("====================================");
    }

    public bool ProcessMove(int x, int y, XO xo)
    {
        var isValid = IsValidMove(x, y);
        if (isValid)
        {
            _spaces[x, y] = xo;
        }

        return isValid;
    }

    private bool IsValidMove(int x, int y)
    {
        return _spaces[x, y] == null;
    }

    public XO? CheckForWinner()
    {
        var horizontals = CheckHorizontals();
        var verticals = CheckVerticals();

        if (horizontals is not null)
            return horizontals;

        if (verticals is not null)
            return verticals;

        return null;
    }

    public bool IsDraw()
    {
        for(int y = 0; y < _spaces.GetLength(0); y++)
        {
            for (int x = 0; x < _spaces.GetLength(1); x++)
            {
                if (_spaces[y, x] == null)
                    return false;
            }
        }

        return true;
    }

    private XO? CheckHorizontals()
    {
        for(int y = 0; y < _spaces.GetLength(0); y++)
        {
            var xs = 0;
            var os = 0;
            
            for (int x = 0; x < _spaces.GetLength(1); x++)
            {
                if (_spaces[y, x] == XO.X)
                    xs++;
                if (_spaces[y, x] == XO.O)
                    os++;
            }

            if (xs == 3)
                return XO.X;
            if (os == 3)
                return XO.O;
        }
        return null;
    }
    
    private XO? CheckVerticals()
    {
        for(int x = 0; x < _spaces.GetLength(1); x++)
        {
            var xs = 0;
            var os = 0;

            for (int y = 0; y < _spaces.GetLength(0); y++)
            {
      
                if (_spaces[y, x] == XO.X)
                    xs++;
                if (_spaces[y, x] == XO.O)
                    os++;
            }
            
            if (xs == 3)
                return XO.X;
            if (os == 3)
                return XO.O;

        }

        return null;
    }
}

public class Player(string name, XO xo)
{
    public string Name { get; init; } = name;
    public XO Xo { get; init; } = xo;

    public (int, int) GetMove()
    {
        Console.WriteLine($"{Name} it is your move (1-9)");
        var key = Console.ReadKey().KeyChar;

        return key switch
        {
            '1' => (0, 0),
            '2' => (0, 1),
            '3' => (0, 2),
            '4' => (1, 0),
            '5' => (1, 1),
            '6' => (1, 2),
            '7' => (2, 0),
            '8' => (2, 1),
            '9' => (2, 2),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum XO
{
    X,
    O
}