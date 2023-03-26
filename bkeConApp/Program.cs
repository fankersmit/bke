#if DEBUG
#define APPTEST
#endif

using System.Runtime.CompilerServices;
using System.Text;
using bkeLib;


// makes methods of Program visible to tests class
//
#if APPTEST
[assembly: InternalsVisibleTo("bkeConAppTests")]
#endif
namespace bkeConApp
{
	internal class Program
	{
		private const int Cross = 1;
		private const int Zero = 0;
		private static bool _keepRunning = true;
		private static bool _gameCompleted = true;
		private static Game _game;

		// Main is where game playing happens
		//
		private static void Main(string[] args)
		{
			// end the program with control+C, allowing running loop to finish
			Console.CancelKeyPress += CancelEventHandler;
			// make sure the board is drawn correctly.
			Console.OutputEncoding = Encoding.Unicode;

			WelcomeMessage();
			var currentPlayer = Cross;

			// main playing loop
			while (_keepRunning)
			{
				if (_gameCompleted)
				{
					StartNewGame();
					_gameCompleted = false;
				}

				DisplayBoard(_game);
				var move = GetNextMove( currentPlayer);

				try
				{
					_game.PlayMove(move);
				}
				catch (InvalidOperationException e)
				{
					Console.WriteLine(e.Message);
					continue;
				}

				if (_game.IsWinningMove(move))
				{
					DisplayBoard(_game);
					Console.WriteLine($"We have a winner: {currentPlayer}");
					_gameCompleted = true;
				}
				else
				{
					_gameCompleted = _game.IsGameCompleted();
					if (_gameCompleted)
					{
						Console.WriteLine("It's a  draw.");
					}
				}

				// change current player
				// each  game starts with another player
				currentPlayer = currentPlayer == Cross ? Zero : Cross;
			}

			Console.WriteLine("Done playing, press any key...");
			Console.ReadKey();
			Environment.Exit(0);
		}

		internal static Move GetNextMove(int currentPlayer)
		{
			string? nextMove; //CS8600
			do
			{
				nextMove = Console.ReadLine();
				nextMove = nextMove ?? string.Empty;
			}
			while( !_game.Board.IsValidCell(nextMove));
			var rc = ConvertInputToCell(nextMove);
			return new Move { Row = rc.row, Col = rc.col, Value = currentPlayer };
		}

		internal static (int row, int col) ConvertInputToCell(string nextMove)
		{
			var col = int.Parse(nextMove.Substring(1, 1)) - 1;
			var row = nextMove.ToUpper()[0] - 65;
			return (row, col);
		}

		internal static void DisplayBoard(Game game)
		{
			game.Board.Render();
		}

		internal static Game StartNewGame()
		{
			_game = new Game();
			return _game;
		}

		// helper methods
		internal static void CancelEventHandler(object? sender, ConsoleCancelEventArgs e)
		{
			_keepRunning = false;
		}

		internal static void WelcomeMessage()
		{
			Console.WriteLine("Welcome, start playing, press <Enter>.  Stop playing,  press <CTRL+C>");
			Console.WriteLine("Specify a move by giving row (A-C) and column (1-3). ex.: A2 or C3");
			Console.ReadKey(true);
		}
	}
}
