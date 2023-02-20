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

	[Fact]
	public void Three_In_Row_Declares_Winner()
	{
		// arrange
		var game = new Game();
		// act
		game.PlayMove(Zero, 0, 0);
		game.PlayMove(Cross, 0, 1);
		game.PlayMove(Zero, 1, 0);
		game.PlayMove(Cross, 0, 2);
		game.PlayMove(Zero, 2, 0);
		// assert
		Assert.True(game.DoWeHave_a_Winner());
	}
}
