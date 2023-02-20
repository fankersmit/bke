namespace bkeLiTests;

using bkeLib;

public class UnitTest1
{
	private const int Zero = 0;
	private const int Cross = 1;
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
		Assert.Throws<InvalidOperationException>( ()=> game.PlayMove(Zero, 0, 0) );
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
		Assert.True(game.DoWeHave_a_Winner(move1,row, 2));
	}
}
