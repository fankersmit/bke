using bkeLib;

namespace GameTests;

public class MoveTests
{
	[Fact]
	public void Move_Is_Initialized()
	{
		// arrange
		var row = 2;
		var col = 3;
		var val = 1;
		// act
		var move = new Move(row, col, val);
		// assert
		Assert.Equal(row, move.Row);
		Assert.Equal(col, move.Col);
		Assert.Equal(val, move.Value);
	}
}
