using bkeLib;

namespace bkeLibTests;

public class MoveTests
{
	[Fact]
	public void Move_Is_Initialized()
	{
		// arrange
		const int row = 2;
		const int col = 3;
		const int val = 1;
		// act
		var move = new Move(row, col, val);
		// assert
		Assert.Equal(row, move.Row);
		Assert.Equal(col, move.Col);
		Assert.Equal(val, move.Value);
	}
}
