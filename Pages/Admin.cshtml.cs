using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using GuestRegistration.Models;

public class AdminModel : PageModel
{
    private readonly IConfiguration _configuration;

    public AdminModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [BindProperty]
    public string Passcode { get; set; } = string.Empty;

    [BindProperty]
    public string? EnteredPasscode { get; set; }

    public List<Guest> Guests { get; set; } = new();
    public bool IsAuthenticated { get; set; } = false;
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        CheckAuthentication();
    }

    public async Task<IActionResult> OnPost()
    {
        var adminPasscode = _configuration["AdminPasscode"] ?? "admin123";

        if (EnteredPasscode == adminPasscode)
        {
            HttpContext.Session.SetString("AdminAuth", "true");
            IsAuthenticated = true;
            await LoadGuests();
            return Page();
        }
        else
        {
            ErrorMessage = "Invalid passcode. Please try again.";
            return Page();
        }
    }

    private void CheckAuthentication()
    {
        var sessionAuth = HttpContext.Session.GetString("AdminAuth");
        IsAuthenticated = sessionAuth == "true";
        
        if (IsAuthenticated)
        {
            LoadGuestsAsync().Wait();
        }
    }

    private async Task LoadGuests()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                ErrorMessage = "Connection string not configured.";
                return;
            }

            await using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            const string sql = "SELECT id, full_name, email, phone FROM guests ORDER BY id DESC;";
            await using var cmd = new SqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Guests.Add(new Guest
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    PhoneNumber = reader.GetInt32(3)
                });
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
    }

    private async Task LoadGuestsAsync()
    {
        await LoadGuests();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Clear();
        return RedirectToPage("/Index");
    }
}
