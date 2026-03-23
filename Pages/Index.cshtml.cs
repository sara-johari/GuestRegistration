using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GuestRegistration.Models;

public class IndexModel : PageModel {
    [BindProperty]
    public Guest GuestInfo { get; set; } = new();

    private readonly string _connectionString;
    private readonly IConfiguration _configuration;

    public IndexModel(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");
    }

    public void OnGet() { }

    public IActionResult OnPost() {
        // Old direct-insert behavior is now replaced by API endpoint /api/guests.
        // This action can stay as a fallback or be used by non-JavaScript clients.
        if (!ModelState.IsValid) return Page();
        return RedirectToPage("Success");
    }
}