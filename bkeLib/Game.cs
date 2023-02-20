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
	private bool FieldIsEmpty(int col, int row)
	{
		return _board[col, row] == -3;
	}

	// play the next move
	// if it is illegal exception is thrown.
	public void PlayMove(int move, int col, int row)
	{
		if (_lastMove == move)
		{
			var msg = "Same payer cannot  play twice.";
			throw new InvalidOperationException(msg);
		}
		else
		{
			_lastMove = move;
		}

		if (!FieldIsEmpty(col, row))
		{
			var msg = $"Field[{col}, {row}] is already filled.";
			throw new InvalidOperationException(msg);
		}
		Board[col, row] = move;
	}

	public bool DoWeHave_a_Winner()
	{
		bool result = false;
		var total = 0; 
		// sum rows
		for (var i = 0; i < 3; ++i)
		{
			total += _board[i, 0];
		}
		result = (total == 0 || total == 3) ? true : false;
		// sum cols
		// sum diagonals
		return result;
	}
}
