//Draw the board
//Ask the players for a choice
//Check if anyone has won (3 in a row) > Complete Game > Next Round

var gameManager = new GameManager();

public class GameManager
{
    private const int BoardSideLength = 3;
    
    private Board _board = new(BoardSideLength);
    
    private Player[] _players = new Player[2];
    private int _numberOfMoves;

    public GameManager()
    {
        for (int i = 0; i < _players.Length; i++)
        {
            Console.Write($"Player {i + 1} Name: ");
            _players[i] = new Player(Console.ReadLine(), Enum.GetValues<Xo>()[i]);
        }
        
        RunGame();
    }

    private void RunGame()
    {
        _board.Render();
        while(true)
        {
            foreach (var player in _players)
            {
                bool moveIsValid = false; 
                while(!moveIsValid)
                {
                    var move = player.GetMove();
                    moveIsValid = _board.ProcessMove(move.Item1, move.Item2, player.Xo);
                }
                _board.Render();
                _numberOfMoves++;
                IsFinished(player.Name);
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

        if(_numberOfMoves == BoardSideLength * BoardSideLength)
        {
            Console.WriteLine("Draw");
            Environment.Exit(0);
        }
    }
}

public class Board(int boardSize)
{
    private readonly Xo?[,] _spaces = new Xo?[boardSize,boardSize];
    
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

    public bool ProcessMove(int x, int y, Xo xo)
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
        
        return CheckHorizontals() || CheckVerticals() || CheckDiagonalsDown() || CheckDiagonalsUp();
    }
    
    private bool CheckHorizontals()
    {
        for(int y = 0; y < _spaces.GetLength(0); y++)
        {
            var xs = 0;
            var os = 0;
            
            for (int x = 0; x < _spaces.GetLength(1); x++)
            {
                if (_spaces[y, x] == Xo.X)
                    xs++;
                if (_spaces[y, x] == Xo.O)
                    os++;
            }

            if (xs == 3 || os == 3)
                return true;
        }
        return false;
    }

    private bool CheckDiagonalsDown()
    {
            var xs = 0;
            var os = 0;

            for (int i = 0; i < _spaces.GetLength(0); i++)
            {
                if (_spaces[i, i] == Xo.X)
                    xs++;
                if (_spaces[i, i] == Xo.O)
                    os++;
            }

            return xs == 3 || os == 3;
    }
    
    private bool CheckDiagonalsUp()
    {
        var xs = 0;
        var os = 0;

        for (int i = 0; i < _spaces.GetLength(0); i++)
        {
            if (_spaces[i, boardSize - i - 1] == Xo.X)
                xs++;
            if (_spaces[i, boardSize - i - 1] == Xo.O)
                os++;
        }

        return xs == 3 || os == 3;
    }
    
    private bool CheckVerticals()
    {
        for(int x = 0; x < _spaces.GetLength(1); x++)
        {
            var xs = 0;
            var os = 0;

            for (int y = 0; y < _spaces.GetLength(0); y++)
            {
      
                if (_spaces[y, x] == Xo.X)
                    xs++;
                if (_spaces[y, x] == Xo.O)
                    os++;
            }

            if (xs == 3 || os == 3)
                return true;

        }

        return false;
    }
}

public class Player(string name, Xo xo)
{
    public string Name { get; init; } = name;
    public Xo Xo { get; init; } = xo;

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

public enum Xo
{
    X,
    O
}