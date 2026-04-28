using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

List<Newsletter> newsletters = new List<Newsletter>();
List<User> users = new List<User>();

var app = builder.Build();



// GET - listar todas
app.MapGet("/api/newsletters", () =>
{
    if (newsletters.Any())
    {
        return Results.Ok(newsletters.ToList());
    }
    return Results.NoContent();
});

app.MapGet("/api/newsletters/{id}", ([FromRoute] Guid id) =>
{
    foreach (Newsletter news in newsletters)
    {
        if (news.Id == id)
        {
            return Results.Ok(news);
        }
    }
    return Results.NotFound("Notícia não encontrada!");
});

// POST - criar
app.MapPost("/api/newsletters", ([FromBody] Newsletter newsletter) =>
{
    foreach (Newsletter news in newsletters)
    {
        if (news.Title == newsletter.Title)
        {
            return Results.Conflict("Essa notícia já existe!");
        }
    }

    newsletters.Add(newsletter);
    return Results.Created("", "Notícia adicionada com sucesso");
});

app.MapDelete("/api/newsletters/{id}", ([FromRoute] Guid id) =>
{
    foreach (Newsletter news in newsletters)
    {
        if (news.Id == id)
        {
            return Results.Ok("Removido com sucesso!");
        }
    }

    return Results.NotFound("Newsletter não encontrada!");
});

app.MapPut("/api/newsletters", ([FromBody] Newsletter newsletter) =>
{
    foreach (Newsletter news in newsletters)
    {
        if (news.Id == newsletter.Id)
        {
            newsletters.Add(newsletter);
            return Results.Ok("Notícia atualizada");
        }
    }

    return Results.NotFound("Notícia não encontrada");
});



app.MapGet("/api/users", () =>
{
    if (users.Any())
    {
        return Results.Ok(users.ToList());
    }
    return Results.NoContent();
});

app.MapGet("/api/users/{id}", ([FromRoute] Guid id) =>
{
    foreach (User user in users)
    {
        if (user.Id == id)
        {
            return Results.Ok(user);
        }
    }
    return Results.NotFound("Usuário não encontrado!");
});

app.MapPost("/api/users", ([FromBody] User user) =>
{
    foreach (User u in users)
    {
        if (user.Name == u.Name)
        {
            return Results.Conflict("Já existe esse usuário!");
        }
    }

    users.Add(user);
    return Results.Created("", "Usuário adicionado com sucesso");
});

app.MapDelete("/api/users/{id}", ([FromRoute] Guid id) =>
{
    foreach (User u in users)
    {
        if (u.Id == id)
        {
            return Results.Ok("Removido com sucesso!");
        }
    }

    return Results.NotFound("Usuário não encontrado!");
});

app.MapPut("/api/users", ([FromBody] User user) =>
{
    foreach (User u in users)
    {
        if (u.Id == user.Id)
        {
            users.Add(u);
            return Results.Ok("Usuário atualizado");
        }
    }

    return Results.NotFound("Usuário não encontrado");
});



app.MapGet("/", () => "API está rodando");

app.Run();
