using Microsoft.Data.SqlClient;
using GuestRegistration.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MapStaticAssets();

app.UseSession();

app.MapPost("/api/guests", async (Guest guest, IConfiguration configuration) =>
{
    if (string.IsNullOrWhiteSpace(guest.FullName) || string.IsNullOrWhiteSpace(guest.Email) || guest.PhoneNumber == 0)
    {
        return Results.BadRequest(new { error = "FullName, Email, and PhoneNumber are required." });
    }

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        return Results.Problem("Connection string not configured.");
    }

    await using var conn = new SqlConnection(connectionString);
    await conn.OpenAsync();

    const string sql = "INSERT INTO guests (full_name, email, phone) VALUES (@name, @email, @phone); SELECT CAST(SCOPE_IDENTITY() AS INT);";
    await using var cmd = new SqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@name", guest.FullName);
    cmd.Parameters.AddWithValue("@email", guest.Email);
    cmd.Parameters.AddWithValue("@phone", guest.PhoneNumber);

    var insertedId = await cmd.ExecuteScalarAsync();
    if (insertedId != null)
    {
        return Results.Created($"/api/guests/{insertedId}", new
        {
            id = insertedId,
            guest.FullName,
            guest.Email,
            guest.PhoneNumber
        });
    }

    return Results.Problem("Unable to insert guest record.");
});

app.MapGet("/api/guests", async (IConfiguration configuration) =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        return Results.Problem("Connection string not configured.");
    }

    await using var conn = new SqlConnection(connectionString);
    await conn.OpenAsync();

    const string sql = "SELECT id, full_name, email, phone FROM guests;";
    await using var cmd = new SqlCommand(sql, conn);
    await using var reader = await cmd.ExecuteReaderAsync();

    var guests = new List<object>();
    while (await reader.ReadAsync())
    {
        guests.Add(new
        {
            id = reader.GetInt32(0),
            fullName = reader.GetString(1),
            email = reader.GetString(2),
            phone = reader.GetInt32(3)
        });
    }

    return Results.Ok(guests);
});

app.MapRazorPages().WithStaticAssets();

app.Run();
