namespace bkeLib;

public class Game
{
	private readonly int[,] _board = new int[3,3];
	private int _lastMove = -1; // init to non used value
	private int _movesCount = 0;

	// Constructors
	//
	public Game()
	{
		ClearBoard();
	}

	// properties
	//
	public int[,] Board => _board;

	// Do we have an empty  playing field?
	public bool BoardIsEmpty()
	{
		return  BoardTotalSum() == -27;
	}

	// initialize board to play a new game
	// playing fields are  -3, totals are -9
	//
	public void ClearBoard()
	{
		const int _initialValue = -3;
		for (var i = 0; i < _board.GetLength(0); i++)
		{
			for (var x = 0; x < _board.GetLength(1); x++)
			{
				_board[i,x] = _initialValue;
			}
		}

		_movesCount = 0;
	}

	// determine Board sum
	//
	private int BoardTotalSum()
	{
		var total = 0;
		for (var i = 0; i < _board.GetLength(0); i++)
		{
			for (var x = 0; x < _board.GetLength(1); x++)
			{
				total +=  _board[i,x] ;
			}
		}
		return total;
	}

	// determine if  the field can be played
	//
	private bool FieldIsEmpty(int row, int col)
	{
		return _board[row, col] == -3;
	}

	// play the next move
	// if it is illegal exception is thrown.
	//
	public void PlayMove(int move, int row, int col)
	{
		if (_lastMove == move)
		{
			var msg = "Same player cannot  play twice.";
			throw new InvalidOperationException(msg);
		}

		if (!FieldIsEmpty(row, col))
		{
			var msg = $"Field[{row}, {col}] is already filled.";
			throw new InvalidOperationException(msg);
		}
		Board[row, col] = move;
		_lastMove = move;
		++_movesCount;
	}

	// determine if last move was a winning move by checking if there
	// is a row , column or diagonal on the board
	public bool IsWinningMove(int row, int col)
	{
		return IsWinningRow(row) |
		       IsWinningColumn(col) |
		       IsWinningLeftDiagonal() |
		       IsWinningRightDiagonal();
	}

	private bool IsWinningRow( int row)
	{
		var total = 0;
		for ( var i = 0; i < 3; ++i )
		{
			total += _board[row, i];
		}
		return total == 0 | total == 3;
	}

	private bool IsWinningColumn( int col)
	{
		var total = 0;
		for ( var i = 0; i < 3; ++i )
		{
			total += _board[i, col];
		}
		return total == 0 | total == 3;
	}

	private bool IsWinningLeftDiagonal()
	{
		var total = 0;
		var col = 0;
		for ( var row = 0; row < 3; ++row  )
		{
			total += _board[row, col];
			++col;
		}
		return total == 0 | total == 3;
	}

	private bool IsWinningRightDiagonal()
	{
		var total = 0;
		var col = 2;
		for ( var row = 0; row < 3; ++row  )
		{
			total += _board[row, col];
			--col;
		}
		return total == 0 | total == 3;
	}

	public bool IsGameCompleted()
	{
		return _movesCount == 9;
	}
}
