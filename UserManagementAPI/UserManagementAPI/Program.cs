using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global middleware for exception handling
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = "Ocorreu um erro inesperado.", detail = ex.Message });
    }
});

// In-memory user store
var users = new List<User>();

// Email validation regex
const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

// Helper for validation
bool IsValidUser(User user, out string? error)
{
    if (string.IsNullOrWhiteSpace(user.Name))
    {
        error = "Name cannot be empty.";
        return false;
    }
    if (string.IsNullOrWhiteSpace(user.Email))
    {
        error = "Email cannot be empty.";
        return false;
    }
    if (!Regex.IsMatch(user.Email, emailPattern))
    {
        error = "Email is not valid.";
        return false;
    }
    error = null;
    return true;
}

// Create user
app.MapPost("/users", (User user) =>
{
    if (!IsValidUser(user, out var error))
        return Results.BadRequest(new { error });

    user.Id = Guid.NewGuid();
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});

// Get paginated users
app.MapGet("/users", (int? page, int? pageSize) =>
{
    int p = page ?? 1;
    int ps = pageSize ?? 10;
    var pagedUsers = users.Skip((p - 1) * ps).Take(ps);
    return Results.Ok(pagedUsers);
});

// Get user by id
app.MapGet("/users/{id}", (string id) =>
{
    if (!Guid.TryParse(id, out var guid))
        return Results.BadRequest(new { error = "The provided ID is not a valid GUID." });

    var user = users.FirstOrDefault(u => u.Id == guid);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

// Update user
app.MapPut("/users/{id}", (Guid id, User updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();

    if (!IsValidUser(updatedUser, out var error))
        return Results.BadRequest(new { error });

    user.Name = updatedUser.Name;
    user.Email = updatedUser.Email;
    return Results.Ok(user);
});

// Delete user
app.MapDelete("/users/{id}", (Guid id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();

    users.Remove(user);
    return Results.NoContent();
});

app.Run();

record User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}