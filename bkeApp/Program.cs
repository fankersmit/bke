// See https://aka.ms/new-console-template for more information
using bkeLib;

Console.WriteLine("Boter, Kaas en  Eieren!");
// naam  speler1  X
// naam  speler 2 0
// toon leeg bord

var game = new Game();

//  helper methods

static void DisplayBoard(Game game)
{

	for (var row = 0; row < 4; ++row)
	{
		Console.WriteLine( "|   |   |   |   |");
		Console.WriteLine( "| - | - | - | - |");
		Console.WriteLine( "|   |   |   |   |");
		Console.WriteLine( "| - | - | - | - |");
	}
} 
