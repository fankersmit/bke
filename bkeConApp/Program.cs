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
		private static bool _keepRunning = true;
		private static bool _gameCompleted = true;
		private const int Cross = 1;
		private const int Zero = 0;

		// Main is where game playing happens
		//
		private static void Main(string[] args)
		{
			// end the program with control+C, allowing running loop to finish
			Console.CancelKeyPress += CancelEventHandler;
			Console.OutputEncoding = Encoding.Unicode;

			WelcomeMessage();
			var game = StartNewGame();
			var currentPlayer = Cross;

			// main playing loop
			while (_keepRunning)
			{
				if (_gameCompleted)
				{
					game = StartNewGame();
					_gameCompleted = false;
				}

				DisplayBoard(game);
				var move = GetNextMove();

				try
				{
					game.PlayMove(currentPlayer, move.row, move.col);
				}
				catch (InvalidOperationException e)
				{
					Console.WriteLine(e.Message);
					continue;
				}

				if (game.IsWinningMove(move.row, move.col))
				{
					DisplayBoard(game);
					Console.WriteLine($"We have a winner: {currentPlayer}");
					_gameCompleted = true;
				}
				else
				{
					_gameCompleted = game.IsGameCompleted();
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

		// return a tuple
		internal static ( int row, int col) GetNextMove()
		{
			var validMoves = new [] { "A1", "A2", "A3", "B1", "B2", "B3", "C1", "C2", "C3" };
			string? nextMove; //CS8600
			do
			{
				nextMove = Console.ReadLine();
				nextMove  = nextMove ?? string.Empty;
			} while (!validMoves.AsEnumerable().Contains(nextMove.ToUpper()));

			return ConvertInputToMove(nextMove);
		}

		internal static (int row, int col) ConvertInputToMove(string nextMove)
		{
			var col = int.Parse(nextMove.Substring(1, 1)) - 1;
			var row = nextMove.ToUpper()[0] - 65;
			return (row, col);
		}

		internal static void DisplayBoard(Game game)
		{
			const string hrz = "\u2500";
			const string vrt = "\u2502";
			const string crs = "\u253C";
			Console.WriteLine();
			for (var row = 0; row < 3; ++row)
			{
				Console.Write(" ");
				for (var col = 0; col < 3; ++col)
				{
					var displayChar = DisplayCharFor(game.Board[row, col]);
					Console.Write($" {displayChar} {(col < 2 ? vrt : Environment.NewLine)}");
				}

				if (row < 2)
				{
					Console.WriteLine($" {hrz}{hrz}{hrz}{crs}{hrz}{hrz}{hrz}{crs}{hrz}{hrz}{hrz}");
				}
			}
			Console.WriteLine();
		}

		// note the use of switch expression
		internal static char DisplayCharFor(int i) => i switch
		{
			0 => 'O',
			1 => 'X',
			_ => ' '
		};

		internal static Game StartNewGame()
		{
			return new Game();
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
