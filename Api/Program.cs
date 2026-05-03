using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=apexnewsletter.db"));

builder.Services.AddScoped<MotorsportScraperService>();
builder.Services.AddScoped<AiSummarizerService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/raspar-noticias", async (IConfiguration config, MotorsportScraperService scraper, AiSummarizerService aiService, AppDbContext db) =>
{
    var racingSites = config.GetSection("RacingSites").Get<Dictionary<string, string>>();

    if (racingSites == null || racingSites.Count == 0)
        return Results.BadRequest("Nenhum site configurado no appsettings.json");

    var relatorioGeral = new Dictionary<string, object>();

    foreach (var site in racingSites)
    {
        string categoria = site.Key;
        string urlBase = site.Value;
        var todosOsLinks = await scraper.ObterLinksAsync(urlBase);
        string termoBusca = categoria.ToLower() == "brasil" ? "stockcar-br" : categoria.ToLower();
        var linksEncontrados = todosOsLinks
            .Where(link => link.ToLower().Contains($"/{termoBusca}/"))
            .ToList();

        string noticiaFinal = "Nenhuma notícia encontrada.";

        if (linksEncontrados.Count > 0)
        {
            string urlNoticia = linksEncontrados[0];
            string textoBruto = await scraper.ObterTextoDaNoticiaAsync(urlNoticia);
            noticiaFinal = await aiService.ResumirNoticiaAsync(textoBruto, categoria, urlNoticia);

            var novaNoticiaParaOBanco = new Newsletter
            {
                Id = Guid.NewGuid(),
                Title = $"Resumo IA - {categoria} ({DateTime.Now:dd/MM/yyyy})",
                Content = noticiaFinal,
            };

            db.Newsletters.Add(novaNoticiaParaOBanco);
        }

        relatorioGeral.Add(categoria, new
        {
            QuantidadeDeLinksEncontrados = linksEncontrados.Count,
            NoticiaGeradaPelaIA = noticiaFinal
        });
    }

    await db.SaveChangesAsync();
    return Results.Ok(relatorioGeral);
});

app.MapGet("/api/newsletters", async (AppDbContext db) =>
{
    var newsletters = await db.Newsletters.ToListAsync();
    if (newsletters.Any())
        return Results.Ok(newsletters);
    return Results.NoContent();
});

app.MapGet("/api/newsletters/{id}", async ([FromRoute] Guid id, AppDbContext db) =>
{
    var news = await db.Newsletters.FindAsync(id);
    if (news != null)
        return Results.Ok(news);
    return Results.NotFound("Notícia não encontrada!");
});

app.MapPost("/api/newsletters", async ([FromBody] Newsletter newsletter, AppDbContext db) =>
{
    var existe = await db.Newsletters.AnyAsync(n => n.Title == newsletter.Title);
    if (existe)
        return Results.Conflict("Essa notícia já existe!");

    db.Newsletters.Add(newsletter);
    await db.SaveChangesAsync();
    return Results.Created("", "Notícia adicionada com sucesso");
});

app.MapPut("/api/newsletters", async ([FromBody] Newsletter newsletter, AppDbContext db) =>
{
    var news = await db.Newsletters.FindAsync(newsletter.Id);
    if (news != null)
    {
        news.Title = newsletter.Title;
        await db.SaveChangesAsync();
        return Results.Ok("Notícia atualizada");
    }
    return Results.NotFound("Notícia não encontrada");
});

app.MapDelete("/api/newsletters/{id}", async ([FromRoute] Guid id, AppDbContext db) =>
{
    var news = await db.Newsletters.FindAsync(id);
    if (news != null)
    {
        db.Newsletters.Remove(news);
        await db.SaveChangesAsync();
        return Results.Ok("Removido com sucesso!");
    }
    return Results.NotFound("Newsletter não encontrada!");
});

app.MapGet("/api/users", async (AppDbContext db) =>
{
    var users = await db.Users.ToListAsync();
    if (users.Any())
        return Results.Ok(users);
    return Results.NoContent();
});

app.MapGet("/api/users/{id}", async ([FromRoute] Guid id, AppDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user != null)
        return Results.Ok(user);
    return Results.NotFound("Usuário não encontrado!");
});

app.MapPost("/api/users", async ([FromBody] User user, AppDbContext db) =>
{
    var existe = await db.Users.AnyAsync(u => u.Name == user.Name);
    if (existe)
        return Results.Conflict("Já existe esse usuário!");

    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created("", "Usuário adicionado com sucesso");
});

app.MapPut("/api/users", async ([FromBody] User user, AppDbContext db) =>
{
    var u = await db.Users.FindAsync(user.Id);
    if (u != null)
    {
        u.Name = user.Name;
        await db.SaveChangesAsync();
        return Results.Ok("Usuário atualizado");
    }
    return Results.NotFound("Usuário não encontrado");
});

app.MapDelete("/api/users/{id}", async ([FromRoute] Guid id, AppDbContext db) =>
{
    var u = await db.Users.FindAsync(id);
    if (u != null)
    {
        db.Users.Remove(u);
        await db.SaveChangesAsync();
        return Results.Ok("Removido com sucesso!");
    }
    return Results.NotFound("Usuário não encontrado!");
});

app.MapGet("/", () => "API está rodando perfeitamente!");

app.Run();