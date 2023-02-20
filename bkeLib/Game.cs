namespace bkeLib;

public class Game
{
	private int[,] _board = new int[3,3];
	private int _lastMove = -1; // init to non used value

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
		var initialValue = -3;
		for (int i = 0; i < _board.GetLength(0); i++)
		{
			for (int x = 0; x < _board.GetLength(1); x++)
			{
				_board[i,x] = initialValue;
			}
		}
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
	}

	public bool DoWeHave_a_Winner(int move, int row, int col)
	{
		if (IsWinningRow(ro))
		{
			return true;
		}

		if (IsWinningColumn(col))
		{
			return true;
		}

		// sum diagonals
		return false;
	}

	private bool IsWinningRow( int row)
	{
		var total = 0;
		for ( var i = 0; i < 3; ++i )
		{
			total += _board[row, i];
		}
		return (total == 0 || total == 3);
	}

	private bool IsWinningColumn( int col)
	{
		var total = 0;
		for ( var i = 0; i < 3; ++i )
		{
			total += _board[i, col];
		}
		return (total == 0 || total == 3);
	}

	private bool IsWinningLeftDiagonal( int row, int col)
	{
		return true;
	}

	private bool IsWinningRightDiagonal( int row, int col)
	{
		return true;
	}
}
