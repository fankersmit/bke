namespace bkeLib;

// this is BKE playing field class
// ii can be of sizes between 3X3 and 8X8.
//
public class Board
{
	// minimum and maximum field sizes
	private const int MinRows = 3;
	private const int MinCols = 3;
	private const int MaxRows = 8;
	private const int MaxCols = 8;

	// fields
	private readonly int[,] _board;
	private readonly int _emptyField;
	private readonly int _seriesCount;

	// constructors, default board size is 3X3
	public Board(int rows = 3, int cols = 3)
	{
		if( !ArgsInRange(rows, cols) )
		{
			var msg = $"Board size has to be in Range [{MinRows},{MinCols}] and: [{MaxRows},{MaxCols}]";
			throw new ArgumentOutOfRangeException(msg);
		}
		_board = new int[rows, cols];
		// determine the _emptyField value
		// this value enable  summing row and cols in determining wining moves
		_emptyField = Math.Max(rows, cols);
		Clear();
		//  determine length of a series
		_seriesCount = 3;
	}

	// properties, get only
	public int Size => _board.Length;
	public int Rows => _board.GetUpperBound(0) + 1;
	public int Columns => _board.GetUpperBound(1) + 1;

	// fill all fields with _emptyField Value
	// value is determined at board creation
	public void Clear()
	{
		for (var i = 0; i < _board.GetLength(0); i++)
		{
			for (var x = 0; x < _board.GetLength(1); x++)
			{
				_board[i,x] = _emptyField;
			}
		}
	}

	// methods
	private static bool ArgsInRange(int rows, int cols)
	{
		return rows >= MinRows & rows <= MaxRows & cols >= MinCols & cols <= MaxCols;
	}

	//  return true if all fields contain _emptyField value
	public bool IsEmpty()
	{
		var boardEnumerator = _board.GetEnumerator();
		while ( boardEnumerator.MoveNext() )
		{
			if( _emptyField != (int)boardEnumerator.Current )
			{
				return false;
			}
		}
		return true;
	}

	// determine if the field can be played
	//
	public bool IsFieldEmpty(int row, int col)
	{
		return _board[row, col] == _emptyField;
	}

	// Put a move on the board (row, col, value is  0 or 1 )
	public void PutMove(Move move)
	{
		PutMove(move.Row, move.Col, move.Value);
	}

	public void PutMove(int row, int col, int move)
	{
		if ( !NotOverTheEdge(row, col) ) // note negation
		{
			var msg = $"Field [{row},{col}] is outside the board!";
			throw new ArgumentOutOfRangeException(msg);
		}

		if ( !IsFieldEmpty(row, col) )
		{
			var current= _board[row, col];
			var msg = $"Field [{row},{col}] is already taken: {current}";
			throw new ArgumentException(msg);
		}
		_board[row, col] = move;
	}

	//  After every move we check if a series of length _seriesCount is
	//  established. The Game can use this to determine a winner
	//
	public bool MoveCreatesSeries(  Move mv)
	{
		return CreatesRowSeries( mv ) |
		       CreatesColumnSeries( mv ) |
		       CreatesLeftDownDiagonalSeries( mv ) |
		       CreatesRightDownDiagonalSeries(  mv );
	}

	public bool CreatesLeftDownDiagonalSeries( Move move )
	{
		var seriesLength = 0;
		var mv  = GetLeftDownStartingPoint(move);
		while( NotOverTheEdge(mv.Row, mv.Col)  & seriesLength != _seriesCount )
		{
			if (_board[mv.Row, mv.Col ] != move.Value)
			{
				// start over
				seriesLength = 0;
			}
			else
			{
				// add to series length
				seriesLength += 1;
			}
			mv.Row += 1;
			mv.Col += 1;
		}
		return seriesLength == _seriesCount;
	}

	public bool NotOverTheEdge( int row, int column )
	{
		return (row < Rows)
		       & (column < Columns)
		       & row >= _board.GetLowerBound(0)
		       & column >= _board.GetLowerBound(1);
	}

	public bool CreatesRightDownDiagonalSeries( Move move )
	{
		var seriesLength = 0;
		var mv  = GetRightDownStartingPoint(move);
		while( NotOverTheEdge(mv.Row, mv.Col)  & seriesLength != _seriesCount )
		{
			if (_board[mv.Row, mv.Col ] != move.Value)
			{
				// start over
				seriesLength = 0;
			}
			else
			{
				// add to series length
				seriesLength += 1;
			}
			mv.Row += 1;
			mv.Col -= 1;
		}
		return seriesLength == _seriesCount;
	}

	public (int Row, int Col)  GetLeftDownStartingPoint( Move mv )
	{
		var spCol =  mv.Col - mv.Row < _board.GetLowerBound(0) ? _board.GetLowerBound(0) : mv.Col - mv.Row;
		var spRow  =  mv.Row + (spCol-mv.Col) < 0 ? 0  : mv.Row + (spCol- mv.Col) ;
		return (spRow, spCol);
	}

	public  (int Row, int Col) GetRightDownStartingPoint(Move mv)
	{
		var spCol = mv.Col + mv.Row < Columns ? mv.Col + mv.Row : _board.GetUpperBound(1);
		var spRow = mv.Row - (spCol-mv.Col) < 0 ? 0  : mv.Row - (spCol-mv.Col) ;
		return (spRow, spCol);
	}

	//  count on move row, until series is found or row is processed
	public bool CreatesRowSeries(Move move)
	{
		var seriesLength = 0;
		for ( var cnt = 0; cnt < Columns & seriesLength != _seriesCount; ++cnt )
		{
			if (_board[move.Row, cnt] != move.Value)
			{
				// start over
				seriesLength = 0;
			}
			else
			{
				// add to series length
				seriesLength += 1;
			}
		}
		return seriesLength == _seriesCount;
	}

	//  count on  move column, until series is found or column is processed
	public bool CreatesColumnSeries(Move move)
	{
		var seriesLength = 0;
		for ( var cnt = 0; cnt < Rows & seriesLength != _seriesCount; ++cnt )
		{
			if (_board[ cnt, move.Col ] != move.Value)
			{
				// start over
				seriesLength = 0;
			}
			else
			{
				// add to series length
				seriesLength += 1;
			}
		}
		return seriesLength == _seriesCount;
	}
}
