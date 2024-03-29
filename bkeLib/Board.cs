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
	private readonly string[] _validMoves;

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
		// this value enables summing row and cols in determining winning moves
		_emptyField = Math.Max(rows, cols);
		Clear();
		// determine valid moves
		_validMoves = CalcValidMoves();
		//  determine length of a series
		_seriesCount = 3;
	}

	// properties, get only
	public int Size => _board.Length;
	public int Rows => _board.GetUpperBound(0) + 1;
	public int Columns => _board.GetUpperBound(1) + 1;

	// properties, indexers
	// Define the get indexer to allow client code to use [] notation.
	public int this[int row, int col]
	{
		get => _board [row, col];
	}

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

	public bool IsValidCell(string cell)
	{
		return _validMoves.Contains(cell.ToUpper());
	}

	public bool IsValidMove(Move move)
	{
		return IsValidMove(move.Row, move.Col, move.Value);
	}

	public bool IsValidMove(int row, int col, int move)
	{
		if ( !NotOverTheEdge(row, col) )
		{
			var msg = $"Field [{row},{col}] is outside the board!";
			throw new InvalidOperationException(msg);
		}

		if ( !IsFieldEmpty(row, col) )
		{
			var current= _board[row, col];
			var msg = $"Field [{row},{col}] is already taken: {current}";
			throw new InvalidOperationException(msg);
		}
		return true;
	}

	// Put a move on the board (row, col, value is  0 or 1 )
	public void PutMove(Move move)
	{
		PutMove(move.Row, move.Col, move.Value);
	}

	public void PutMove(int row, int col, int move)
	{
		if( IsValidMove( row, col, move) )
		{
			_board[row, col] = move;
		}
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

	private string[] CalcValidMoves()
	{
		var validMoves = new string[Rows * Columns];
		var multipl = Rows > Columns ? Columns : Rows; 
		for( var row = 0 ; row < Rows; ++row)
		{
			for (var col = 0; col < Columns; ++col)
			{
				validMoves[row*multipl + col] = $"{ (char)(row + 65)}{col+1}";
			}
		}

		return validMoves;
	}

	// display current board and moves played
	public void Render()
	{
		var hrz = new string('\u2500', 3);
		const string vrt = "\u2502";
		const string crs = "\u253C";

		Console.WriteLine();
		for (var row = 0; row < Rows; ++row)
		{
			for (var col = 0; col < Columns; ++col)
			{
				var displayChar = DisplayCharFor( _board[row, col]);
				Console.Write($" {displayChar} {(col < Columns - 1 ? vrt : Environment.NewLine)}");
			}

			// line in between
			if (row < (Rows - 1))
			{
				for (var col = 0; col < Columns - 1; ++col)
				{
					Console.Write($"{hrz}{crs}");
				}
				Console.WriteLine(hrz);
			}
		}
		Console.WriteLine();
	}

	// note the use of switch expression
	public char DisplayCharFor(int i) => i switch
	{
		0 => 'O',
		1 => 'X',
		_ => ' '
	};
}
