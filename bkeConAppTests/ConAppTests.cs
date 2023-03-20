using bkeConApp;
using bkeLib;

namespace bkeConAppTests;

public class ConAppTests
{
	[Fact]
	public void Can_Start_New_Game()
	{
		// arrange, act
		var game = Program.StartNewGame();
		// assert
		Assert.NotNull(game);
		Assert.IsType<Game>(game);
		Assert.True(game.BoardIsEmpty());
	}

	[Theory]
	[InlineData("A1", 0, 0)]
	[InlineData("A2", 0, 1)]
	[InlineData("A3", 0, 2)]
	[InlineData("B1", 1, 0)]
	[InlineData("B2", 1, 1)]
	[InlineData("B3", 1, 2)]
	[InlineData("C1", 2, 0)]
	[InlineData("C2", 2, 1)]
	[InlineData("C3", 2, 2)]
	public void Can_Convert_Valid_Input_NextMove(string input, int row, int col)
	{
		// arrange, act
		var result = Program.ConvertInputToCell(input);
		// assert
		Assert.Equal(row, result.row);
		Assert.Equal(col, result.col);
	}

	[Theory]
	[InlineData( 0, 'O' )]
	[InlineData( 1, 'X' )]
	[InlineData( 3, ' ' )]
	[InlineData( 23, ' ' )]
	public void Can_Convert_Move_To_DisplayChar( int input, char expectedResult)
	{
		// arrange, act
		var result = Program.DisplayCharFor(input);
		// assert
		Assert.Equal(expectedResult, result);
	}

	[Fact]
	public void Get_Next_Move_Returns_Move_From_Input()
	{
		// arrange
		const int currentPlayer = 0;
		var oldIn = Console.In;
		const string input = "A1";
		var expectedResult = new Move {Row = 0, Col =0, Value = 0 };
		Move result;
		// act
		using (var reader = new StringReader(input))
		{
			Console.SetIn(reader);
			Program.StartNewGame();
			result = Program.GetNextMove(currentPlayer);
		}
		// assert
		Assert.Equal(expectedResult.Row, result.Row );
		Assert.Equal(expectedResult.Col, result.Col);
		Assert.Equal(expectedResult.Value, result.Value );
		// clean up
		Console.SetIn(oldIn);
	}

	[Fact]
	public void Can_Display_New_Board()
	{
		const string expectedHrz = "\u2500";
		const string expectedVrt = "\u2502";
		const string expectedCrs = "\u253C";
		const int expectedLength = 74;

		// arrange, redirect console to memory
		string board;
		var oldOut = Console.Out;

		using( var ms = new MemoryStream() )
		{
			StreamWriter currentOut;
			using (currentOut = new StreamWriter(ms))
			{
				currentOut.AutoFlush = true;
				Console.SetOut(currentOut); // redirect console
				// act
				Program.DisplayBoard(new Game());

				ms.Flush();
				ms.Position = 0;
				using (var reader = new StreamReader(ms))
				{
					board = reader.ReadToEnd();
				}
			}
		}
		// assert
		Assert.Contains( expectedHrz, board );
		Assert.Contains( expectedVrt, board );
		Assert.Contains( expectedCrs, board );
		Assert.Equal( expectedLength, board.Length );

		// clean up, redirect console
		Console.SetOut(oldOut);
	}
}
