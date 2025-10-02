var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// In-memory user store
var users = new List<User>();

// Create user
app.MapPost("/users", (User user) =>
{
    user.Id = Guid.NewGuid();
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});

// Get all users
app.MapGet("/users", () => users);

// Get user by id
app.MapGet("/users/{id}", (Guid id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

// Update user
app.MapPut("/users/{id}", (Guid id, User updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();

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