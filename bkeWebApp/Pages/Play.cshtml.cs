using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bkeWebApp.Pages;

public class Play : PageModel
{
	public void OnGet()
	{
		
	}
	
	public async Task<IActionResult> OnPostAsync()
	{
		// check if  valid move
		
		// display x or o
		
		// do we have a winner


		return RedirectToPage("./Play");
	}
}
