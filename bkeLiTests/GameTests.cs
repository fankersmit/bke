namespace bkeLiTests;

using bkeLib;

public class bkeLibUnitTests
{
	private const int Zero = 0;
	private const int Cross = 1;

	[Fact]
	public void Game_Not_Completed_When_Fields_Are_Free()
	{
		// arrange
		var game = new Game();

		// act, play
		game.PlayMove(Zero, 0, 0);
		game.PlayMove(Cross, 0, 1);
		game.PlayMove(Zero, 0, 2);
		game.PlayMove(Cross, 1, 0);
		// assert
		Assert.False(game.IsGameCompleted());
	}
	[Fact]
	public void Game_Completed_When_All_Fields_Are_Taken()
	{
		// arrange
		var game = new Game();

		// act, play
		game.PlayMove(Zero, 0, 0);
		game.PlayMove(Cross, 0, 1);
		game.PlayMove(Zero, 0, 2);
		game.PlayMove(Cross, 1, 0);
		game.PlayMove(Zero, 1, 1);
		game.PlayMove(Cross, 1, 2);
		game.PlayMove(Zero, 2, 0);
		game.PlayMove(Cross, 2, 2);
		game.PlayMove(Zero, 2, 1);
		// assert
		Assert.True(game.IsGameCompleted());
	}

	[Fact]
	public void Can_We_Start_New_Game()
	{
		// arrange
		var game = new Game();
		// act
		var boardIsEmpty = game.BoardIsEmpty();
		// assert
		Assert.True(boardIsEmpty);
	}

	[Fact]
	public void Move_Is_Recorded()
	{
		// arrange
		var game = new Game();
		// act
		game.PlayMove(Zero, 0, 0);
		// assert
		Assert.False(game.BoardIsEmpty());
	}

	[Fact]
	public void Cannot_Play_At_NonEmpty_Field_()
	{
		// arrange
		var game = new Game();
		game.PlayMove(Zero, 0, 0);
		// act, assert
		Assert.Throws<InvalidOperationException>( ()=> game.PlayMove(Cross, 0, 0) );
	}

	[Fact]
	public void Player_Cannot_Play_Twice()
	{
		// arrange
		var game = new Game();
		game.PlayMove(Zero, 0, 0);
		// act, assert
		Assert.Throws<InvalidOperationException>( ()=> game.PlayMove(Zero, 0, 0) );
	}

	[Fact]
	public void No_Winner_When_No_Three_Zeros_Or_Crosses()
	{
		// arrange
		var game = new Game();
		// act
		game.PlayMove(Zero, 0, 0);
		game.PlayMove(Cross, 0, 1);
		game.PlayMove(Zero, 1, 0);
		game.PlayMove(Cross, 2, 0);
		game.PlayMove(Zero, 1, 2);
		game.PlayMove(Cross, 0, 2);
		Assert.False(game.IsWinningMove(0, 2));
	}

	[Theory]
	[InlineData(Zero, Cross, 0)]
	[InlineData(Zero, Cross, 1)]
	[InlineData(Zero, Cross, 2)]
	[InlineData(Cross, Zero, 0)]
	[InlineData(Cross, Zero, 1)]
	[InlineData(Cross, Zero, 2)]
	public void Three_In_Row_Declares_Winner( int move1, int move2, int row)
	{
		// arrange
		var game = new Game();
		var rowMove2 = row == 0 ? 1 : 0;

		// act
		game.PlayMove(move1, row, 0);
		game.PlayMove(move2, rowMove2, 0);
		game.PlayMove(move1, row, 1);
		game.PlayMove(move2, rowMove2, 1);
		game.PlayMove(move1, row, 2);
		// assert
		Assert.True(game.IsWinningMove(row, 2));
	}

	[Theory]
	[InlineData(Zero, Cross, 0)]
	[InlineData(Zero, Cross, 1)]
	[InlineData(Zero, Cross, 2)]
	[InlineData(Cross, Zero, 0)]
	[InlineData(Cross, Zero, 1)]
	[InlineData(Cross, Zero, 2)]
	public void Three_In_Column_Declares_Winner( int move1, int move2, int col)
	{
		// arrange
		var game = new Game();
		var colMove2 = col == 0 ? 1 : 0;

		// act
		game.PlayMove(move1, 0, col);
		game.PlayMove(move2, 0, colMove2);
		game.PlayMove(move1, 1, col);
		game.PlayMove(move2, 1, colMove2);
		game.PlayMove(move1, 2, col);
		// assert
		Assert.True(game.IsWinningMove( 2, col));
	}

	[Theory]
	[InlineData(Zero, 0, 0)]
	[InlineData(Zero, 0, 2)]
	[InlineData(Zero, 2, 0)]
	[InlineData(Zero, 2, 2)]
	[InlineData(Cross, 0, 0)]
	[InlineData(Cross, 0, 2)]
	[InlineData(Cross, 2, 0)]
	[InlineData(Cross, 2, 2)]
	public void Three_In_Diagonal_Declares_Winner( int move1, int row, int col  )
	{
		// arrange
		var game = new Game();
		var move2 = move1 == Zero ? Cross : Zero;
		// use minus one  to determine next moves along diagonal
		var signCol = col == 0 ? 1 : -1;
		var signRow = row == 0 ? 1 : -1;

		// act
		game.PlayMove( move1, row, col);
		game.PlayMove( move2, row, col + 1*signCol);
		game.PlayMove( move1, row + 1*signRow, col + 1*signCol);
 		game.PlayMove( move2, row+1*signRow , col+ 2*signCol );
		game.PlayMove( move1, row + 2*signRow, col + 2*signCol );
		// assert
		Assert.True(game.IsWinningMove( row + 2*signRow, col + 2*signCol));
	}
}
