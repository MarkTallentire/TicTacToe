//Draw the board
//Ask the players for a choice
//Check if anyone has won (3 in a row) > Complete Game > Next Round

var game = new GameManager();
game.InitialiseGame();

public class GameManager()
{
    private const int BOARD_SIDE_LENGTH = 3;
    
    private Board _board = new(BOARD_SIDE_LENGTH);
    private Player _playerOne;
    private Player _playerTwo;

    private Player[] _players = new Player[2];
    private int _numberOfMoves = 0;
    
    public void InitialiseGame()
    {
        for (int i = 0; i < _players.Length; i++)
        {
            Console.Write($"Player {i} Name: ");
            _players[i] = new Player(Console.ReadLine(), Enum.GetValues<XO>()[i]);
        }
        
        RunGame();
    }

    private void RunGame()
    {
        _board.Render();
        while(true)
        {
            
            
            for (int i = 0; i < _players.Length; i++)
            {
                bool moveIsValid = false; 
                while(!moveIsValid)
                {
                    var move = _players[i].GetMove();
                    moveIsValid = _board.ProcessMove(move.Item1, move.Item2, _players[i].Xo);
                }
                _board.Render();
                _numberOfMoves++;
                IsFinished(_players[i].Name);
            }
            
            
        }
    }
    private void IsFinished(string currentPlayerName)
    {
        var isWinner = _board.CheckForWinner();
        if(isWinner)
        {
            Console.WriteLine($"{currentPlayerName} wins");
            Environment.Exit(0);
        }

        if(_numberOfMoves == BOARD_SIDE_LENGTH * BOARD_SIDE_LENGTH)
        {
            Console.WriteLine("Draw");
            Environment.Exit(0);
        }
    }
}

public class Board(int boardSize)
{
    private XO?[,] _spaces = new XO?[boardSize,boardSize];
    
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

    public bool CheckForWinner()
    {
        var horizontals = CheckHorizontals();
        var verticals = CheckVerticals();

        return horizontals || verticals;
    }
    
    private bool CheckHorizontals()
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

            if (xs == 3 || os == 3)
                return true;
        }
        return false;
    }
    
    private bool CheckVerticals()
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

            if (xs == 3 || os == 3)
                return true;

        }

        return false;
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