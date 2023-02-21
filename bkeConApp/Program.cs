﻿
using System.Text;
using bkeLib;

namespace bkeConApp;

internal class Program
{
	private static bool _keepRunning = true;
	private static bool _gameCompleted = true;
	private const int Cross = 1;
	private const int Zero = 0;

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

			DisplayBoard( game );
			var move = GetNextMove();
			game.PlayMove( currentPlayer ,move.row, move.col);
			if (game.IsWinningMove(move.row, move.col))
			{
				Console. WriteLine($"We have a winner: {currentPlayer}");
				_gameCompleted = true;
			}

			currentPlayer = currentPlayer == Cross ? Zero : Cross;
		}

		Console.WriteLine("Done playing, press any key...");
		Console.ReadKey();
		Environment.Exit(0);
	}

	// return a tuple
	private static ( int row, int col) GetNextMove()
	{
		var row = GetRow();
		var col = GetColumn();
		return (row, col);
	}

	private static int GetColumn()
	{
		// notice: numpad is not supported
		var validCols = new[] { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3 };
		var keyInfo = new ConsoleKeyInfo();
		do
		{
			keyInfo = Console.ReadKey();
		} while (!validCols.AsEnumerable().Contains(keyInfo.Key));
		return int.Parse(keyInfo.KeyChar.ToString()) - 1;
	}

	private static int GetRow()
	{
		var validRows = new[] { ConsoleKey.A, ConsoleKey.B, ConsoleKey.C };
		var keyInfo = new ConsoleKeyInfo();
		do
		{
			keyInfo = Console.ReadKey();
		} while (!validRows.AsEnumerable().Contains(keyInfo.Key));
		return keyInfo.KeyChar.ToString().ToUpper()[0] - 65;
	}

	private static void DisplayBoard(Game game)
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
				Console.Write($" {displayChar} { (col<2?vrt:Environment.NewLine)}" );
			}
			if (row < 2)
			{
				Console.WriteLine($" {hrz}{hrz}{hrz}{crs}{hrz}{hrz}{hrz}{crs}{hrz}{hrz}{hrz}");
			}
		}
		Console.WriteLine();
	}

	// note the use of switch expression
	private static char DisplayCharFor(int i) => i switch
	{
		0 => 'O',
		1 => 'X',
		_ => ' '
	};

	private static Game StartNewGame()
	{
		return new Game();
	}

	// helper methods 
	private static void CancelEventHandler(object? sender, ConsoleCancelEventArgs e)
	{
		_keepRunning = false;
	}

	private static void WelcomeMessage()
	{
		Console.WriteLine("Welcome, start playing, press <Enter>.  Stop playing,  press <CTRL+C>");
		Console.WriteLine("Specify a move by giving row (A-C) and column (1-3). ex.: A2 or C3");
		Console.ReadKey(true);
	}
}
