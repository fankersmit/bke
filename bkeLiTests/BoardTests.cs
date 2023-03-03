namespace bkeLiTests;

using bkeLib;

public class BoardTests
{
	const int SeriesCount = 3;

	// test data for right-down diagonal
	public static IEnumerable<object[]> DataForRightDownDiagonals()
	{
		// board,size rows, cols, moveRow, moveCol, resultRow, resultCol
		yield return new object[] { 3, 3, 0,0, 0,0 };
		yield return new object[] { 3, 3, 1,1, 0,2 };
		yield return new object[] { 3, 3, 2,0, 0,2 };
		yield return new object[] { 4, 4, 2,2, 1,3 };
		yield return new object[] { 4, 4, 0,1, 0,1 };
		yield return new object[] { 4, 4, 1,2, 0,3 };
		yield return new object[] { 4, 4, 2,3, 2,3 };
		yield return new object[] { 4, 7, 3,3, 0,6 };
		yield return new object[] { 4, 7, 2,4, 0,6 };
		yield return new object[] { 4, 7, 1,4, 0,5 };
		yield return new object[] { 4, 7, 3,4, 1,6 };
		yield return new object[] { 6, 3, 3,0, 1,2 };
		yield return new object[] { 8, 5, 6,2, 4,4 };
	}

	[Theory]
	[MemberData(nameof(DataForRightDownDiagonals))]
	public void Can_Find_RightDown_Diagonal_StartingPoint( int br, int bc, int mr, int mc, int rr, int rc )
	{
		// arrange
		var board = new Board(br,bc);
		var expectedStartingPoint = (rr, rc);
		// act
		var move = new Move(mr, mc, 1);
		board.PutMove(move);
		// assert
		Assert.Equal(expectedStartingPoint, board.GetRightDownStartingPoint(move));
	}

	// test data for left-down diagonal
	public static IEnumerable<object[]> DataForLeftDownDiagonals()
	{
		// board,size rows, cols, moveRow, moveCol, resultRow, resultCol
		yield return new object[] { 3, 3, 0,0, 0,0 };
		yield return new object[] { 3, 3, 1,1, 0,0 };
		yield return new object[] { 3, 3, 2,0, 2,0 };
		yield return new object[] { 4, 4, 2,2, 0,0 };
		yield return new object[] { 4, 4, 0,1, 0,1 };
		yield return new object[] { 4, 4, 1,2, 0,1 };
		yield return new object[] { 4, 4, 2,3, 0,1 };
		yield return new object[] { 4, 7, 3,3, 0,0 };
		yield return new object[] { 4, 7, 2,4, 0,2 };
		yield return new object[] { 4, 7, 1,4, 0,3 };
		yield return new object[] { 4, 7, 3,4, 0,1 };
		yield return new object[] { 6, 3, 3,0, 3,0 };
		yield return new object[] { 8, 5, 6,2, 4,0 };
	}

	[Theory]
	[MemberData(nameof(DataForLeftDownDiagonals))]
	public void Can_Find_LeftDown_Diagonal_StartingPoint(int br, int bc, int mr, int mc, int rr, int rc )
	{
		// arrange
		var board = new Board(br,bc);
		var move = new Move(mr, mc, 1);
		var expectedStartingPoint = (rr, rc);
		// act
		board.PutMove(move);
		// assert
		Assert.Equal(expectedStartingPoint, board.GetLeftDownStartingPoint(move));
	}

	[Fact]
	public void Can_Determine_Move_Creates_Series_On_Diagonal()
	{
		// arrange
		(int row, int col) lastMove = (-1,-1);
		(int row, int col) move = (0,2);
		var board = new Board();
		// act, create  diagonal series
		for( var cnt = 0; cnt < 3; ++cnt )
		{
			board.PutMove(move.row + cnt, move.col - cnt, 1);
			lastMove = (move.row + cnt, move.col - cnt);
		}
		// act
		var actual = board.MoveCreatesSeries( lastMove.row, lastMove.col, 1);
		// assert
		Assert.True( actual );
	}

	// test data for left-down diagonal
	public static IEnumerable<object[]> DataForColumns()
	{
		// rows, initial row, expected
		yield return new object[] { 3, 0, true };
		yield return new object[] { 8, 0, true };
		yield return new object[] { 6, 0, true };
		yield return new object[] { 6, 2, true };
		yield return new object[] { 6, 4, false };
		yield return new object[] { 6, 5, false };
		yield return new object[] { 3, 1, false };
	}

	[Theory]
	[MemberData(nameof(DataForColumns))]
	public void Can_Determine_Move_Creates_Series_On_Column(int rows, int initialRow, bool expected )
	{
		// arrange
		var lastMove = new Move(-1,-1,-1);
		var board = new Board(rows); // cols has default of 3
		for (var row = initialRow; row < board.Rows & row < (initialRow + SeriesCount); ++row)
		{
			lastMove = new Move(row, 0, 1);
			board.PutMove(lastMove);
		}
		// act
		var actual = board.CreatesColumnSeries( lastMove );
		// assert
		Assert.Equal(expected, actual );
	}

	public static IEnumerable<object[]> DataForRows()
	{
		yield return new object[] { 3, 0, true };
		yield return new object[] { 8, 0, true };
		yield return new object[] { 6, 0, true };
		yield return new object[] { 6, 2, true };
		yield return new object[] { 6, 4, false };
		yield return new object[] { 6, 5, false };
		yield return new object[] { 3, 1, false };
	}

	[Theory]
	[MemberData(nameof(DataForRows))]
	public void Can_Determine_Move_Creates_Series_On_Row(int cols, int initialCol, bool expected )
	{
		// arrange
		var  lastMove = new Move(-1,-1,-1);
		var board = new Board(3,cols);
		for (var col = initialCol; col < board.Columns & col < (initialCol + SeriesCount); ++col)
		{
			lastMove = new Move(0,col,0);
			board.PutMove(lastMove);
		}
		// act
		var actual = board.CreatesRowSeries(lastMove);
		// assert
		Assert.Equal(expected, actual );
	}

	[Fact]
	public void Cannot_Put_Move_On_Occupied_Field()
	{
		// arrange
		var board = new Board();
	    board.PutMove(0,0,0);
		// act, assert
		Assert.Throws<ArgumentException>(() => board.PutMove(0, 0, 0));
	}

	[Fact]
	public void Can_Determine_If_Field_Is_Empty()
	{
		// arrange
		var expected =  true;
		// act
		var board = new Board();
		// assert
		Assert.Equal( expected, board.IsFieldEmpty(0,0));
	}

	[Fact]
	public void Can_Determine_If_Field_Is_Not_Empty()
	{
		// arrange
		var expected =  false;
		// act
		var board = new Board();
		board.PutMove(0,0,1);
		// assert
		Assert.Equal( expected, board.IsFieldEmpty(0,0));
	}

	[Fact]
	public void Can_Determine_If_Board_Is_Empty()
	{
		// arrange
		var expected = true;
		// act
		var board = new Board();
		// assert
		Assert.Equal(expected, board.IsEmpty() );
	}

	[Fact]

	public void Can_Determine_If_Board_Is_Not_Empty()
	{
		// arrange
		var expected = false;
		// act
		var board = new Board();
		board.PutMove(1, 1, 0);
		// assert
		Assert.Equal(expected, board.IsEmpty() );
	}

	[Theory]
	[InlineData(3,2)]
	[InlineData(2,3)]
	[InlineData(2,2)]
	[InlineData(12,8)]
	[InlineData(8, 12)]
	[InlineData(9, 9)]
	public void Creating_Board_With_Invalid_Size_Throws_Exception(int rows, int cols)
	{
		// arrange, act, assert
		Assert.Throws<ArgumentOutOfRangeException>( () => new Board(rows, cols));
	}

	[Theory]
	[InlineData(3,3)]
	[InlineData(8,8)]
	[InlineData(3,5)]
	[InlineData(5,3)]
	[InlineData(4,7)]
	[InlineData(6,5)]
	[InlineData(5,7)]
	[InlineData(7,8)]
	public void Can_Create_Board_Of_Valid_Size(int rows, int cols)
	{
		// arrange, act
		var board = new Board(rows, cols);
		// assert
		Assert.Equal( rows * cols, board.Size);
	}

	[Fact]
	public void Default_Board_Has_Nine_Fields()
	{
		// arrange
		var expectedSize = 9;
		var expectedRows = 3;
		var expectedCols = 3;

		// act
		var board = new Board();
		Assert.Equal(expectedSize, board.Size);
		Assert.Equal(expectedRows, board.Rows);
		Assert.Equal(expectedCols, board.Columns);
	}
}
