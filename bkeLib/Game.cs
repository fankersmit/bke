namespace bkeLib;

public class Game
{
	private readonly Board _board;
	private int _lastMove = -1; // init to non used value
	private int _movesCount; // initialized to 0 by default

	// Constructors
	// Start a new game on default board 3X3
	public Game()
	{
		_board = new Board();
		StartNewGame();
	}

	public Game( Board  board)
	{
		_board = board;
		StartNewGame();
	}

	// properties
	//
	public Board Board => _board;

	// Do we have an empty  playing field?
	public bool BoardIsEmpty() => _board.IsEmpty();

	// initialize board to play a new game
	// playing fields are  -3, totals are -9
	//
	public void ClearBoard()
	{
		_board.Clear();
	}

	private void StartNewGame()
	{
		ClearBoard();
		_movesCount = 0;
	}

	// play the next move
	// if it is illegal exception is thrown.
	//
	public void PlayMove(int move, int row, int col)
	{
		if (_lastMove == move)
		{
			const string msg = "Same player cannot  play twice.";
			throw new InvalidOperationException(msg);
		}

		if (_board.IsValidMove(row, col, move))
		{
			Board.PutMove(row, col, move);
			_lastMove = move;
			++_movesCount;
	}
}

	public void PlayMove( Move move )
	{
		if (_lastMove == move.Value)
		{
			const string msg = "Same player cannot  play twice.";
			throw new InvalidOperationException(msg);
		}

		if (_board.IsValidMove( move))
		{
			Board.PutMove(move);
			_lastMove = move.Value;
			++_movesCount;
		}
	}

	// determine if last move was a winning move by checking if there
	// is a row , column or diagonal on the board
	public bool IsWinningMove(int row, int col)
	{
		return _board.MoveCreatesSeries(new Move(row, col, _lastMove));
	}

	public bool IsWinningMove(Move move)
	{
		return _board.MoveCreatesSeries(move);
	}

	// determine if there are still possible moves  to play
	// or is the bord totally filled
	public bool IsGameCompleted()
	{
		return _movesCount == _board.Size;
	}
}
