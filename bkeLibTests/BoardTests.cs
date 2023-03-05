using System.Runtime.Intrinsics.X86;

namespace bkeLibTests;

using bkeLib;

public class BoardTests
{
	private const int SeriesCount = 3;

	//
	public static IEnumerable<object[]> DataCreatesSeries()
	{
		// row, col, expected result
		yield return new object[] { 2, 0, 0, 2, true };
		yield return new object[] { 1, 0, 1, 2, true };
		yield return new object[] { 0, 2, 2, 0, true };
		yield return new object[] { 0, 1, 2, 1, true };
		yield return new object[] { 2, 0, 2, 2, false };
		yield return new object[] { 1, 0, 2, 2, false };
		yield return new object[] { 0, 2, 2, 2, false };
		yield return new object[] { 0, 1, 2, 2, false };
	}

	[Theory]
	[MemberData( nameof(DataCreatesSeries))]
	public void Determine_If_Move_Creates_Series(int r1, int c1, int  r2, int c2, bool expected)
	{
		// arrange, test for row, col sBand both diagonals
		var board = new Board();
		var secondMove = new Move(r1, c1, 0);
		var thirdMove = new Move( r2, c2, 0);
		// act
		board.PutMove( new Move(1,1, 0)); // play in the centre
		board.PutMove( secondMove);
		board.PutMove( thirdMove);
		// assert
		Assert.Equal( expected,  board.MoveCreatesSeries(thirdMove) );
	}

	public static IEnumerable<object[]> DataForNotOverTheEdge()
	{
		// row, col, expected result
		yield return new object[] { 1, 2, true };
		yield return new object[] { 2, 0, true };
		yield return new object[] { 0, 2, true };
		yield return new object[] { 0, 4, false };
		yield return new object[] { 4, 0, false };
		yield return new object[] { -1, -1, false };
	}

	[Theory]
	[MemberData(nameof(DataForNotOverTheEdge))]
	public void Can_Determine_Move_Is_On_Board( int row, int col, bool expected)
	{
		// arrange
		var board = new Board();
		// act, assert
		Assert.Equal(expected,board.NotOverTheEdge(row, col) );
	}

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

	// test data for columns
	public static IEnumerable<object[]> DataForRightDownDiagonalsSeries()
	{
		// rows, cols, initial row and col, expected result
		yield return new object[] { 3, 3, 0, 2, true };
		yield return new object[] { 4, 4, 1, 3, true };
		yield return new object[] { 5, 5, 2, 2, true };
		yield return new object[] { 3, 3, 1, 1, false };
		yield return new object[] { 3, 5, 0, 2, true };
		yield return new object[] { 3, 5, 0, 1, false };
		yield return new object[] { 5, 3, 3, 0, false };
		yield return new object[] { 5, 3, 2, 0, false };
		yield return new object[] { 5, 4, 2, 3, true };
	}

	[Theory]
	[MemberData(nameof(DataForRightDownDiagonalsSeries))]
	public void Can_Determine_Move_Creates_Series_On_RightDown_Diagonal(int bsr, int bsc,int r, int c, bool expected)
	{
		// arrange
		var  lastMove = new Move(-1,-1, -1);
		var board = new Board(bsr,bsc);
		var row =  r;
		var col = c;
		// act, create diagonal series
		for( var cnt = 0; cnt < SeriesCount; ++cnt )
		{
			//  break on board  border
			lastMove = new Move(row + cnt, col - cnt, 1);
			if ( board.NotOverTheEdge(lastMove.Row, lastMove.Col))
			{
				board.PutMove(lastMove);
			}
		}
		// act
		var actual = board.CreatesRightDownDiagonalSeries( lastMove);
		// assert
		Assert.Equal( expected, actual );
	}

	[Fact]
	public void Can_Find_Diagonal_Series_With_Hole()
	{
		// arrange
		var board = new Board(8, 8);
		var lastMove = new Move();
		// act
		for( var cnt = 0; cnt < 6; ++cnt )
		{
			var val = cnt == 2 ? 0 : 1; // create a hole on the diagonal;
			lastMove = new Move(cnt, cnt, val);
			board.PutMove(lastMove);
		}
		// assert
		Assert.True( board.CreatesLeftDownDiagonalSeries(lastMove ));
	}

	// test data for left-down diagonal
	public static IEnumerable<object[]> DataForLeftDownDiagonalsStartingPoint()
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
	[MemberData(nameof(DataForLeftDownDiagonalsStartingPoint))]
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

	// test data for columns
	public static IEnumerable<object[]> DataForLeftDownDiagonalsSeries()
	{
		// rows, cols, initial row and col, expected result
		yield return new object[] { 3, 3, 0, 0, true };
		yield return new object[] { 4, 4, 1, 1, true };
		yield return new object[] { 5, 5, 2, 2, true };
		yield return new object[] { 3, 3, 1, 1, false };
		yield return new object[] { 3, 5, 0, 2, true };
		yield return new object[] { 3, 5, 0, 3, false };
		yield return new object[] { 5, 3, 3, 0, false };
		yield return new object[] { 5, 3, 2, 0, true };
	}

	[Theory]
	[MemberData(nameof(DataForLeftDownDiagonalsSeries))]
	public void Can_Determine_Move_Creates_Series_On_LeftDown_Diagonal(int bsr, int bsc,int r, int c, bool expected)
	{
		// arrange
		var  lastMove = new Move(-1,-1, -1);
		var board = new Board(bsr,bsc);
		var row =  r;
		var col = c;
		// act, create diagonal series
		for( var cnt = 0; cnt < SeriesCount; ++cnt )
		{
			//  break on board  border
			lastMove = new Move(row + cnt, col + cnt, 1);
			if ( board.NotOverTheEdge(lastMove.Row, lastMove.Col))
			{
				board.PutMove(lastMove);
			}
		}
		// act
		var actual = board.CreatesLeftDownDiagonalSeries( lastMove);
		// assert
		Assert.Equal( expected, actual );
	}

	// test data for columns
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
	public void Cannot_Put_Move_Outside_TheBoard()
	{
		// arrange
		var board = new Board(); // default size is 3 rows and columns
		// act
		var move = new Move(3, 3, 1);
		// assert
		Assert.Throws<ArgumentOutOfRangeException>( ()=> board.PutMove(move));
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
