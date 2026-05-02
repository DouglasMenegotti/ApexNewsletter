using Api.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<MotorsportScraperService>();
builder.Services.AddScoped<AiSummarizerService>();

var app = builder.Build();

List<Newsletter> newsletters = new List<Newsletter>();
List<User> users = new List<User>();

app.MapGet("/raspar-noticias", async (IConfiguration config, MotorsportScraperService scraper, AiSummarizerService aiService) =>
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

            newsletters.Add(novaNoticiaParaOBanco);
        }

        relatorioGeral.Add(categoria, new
        {
            QuantidadeDeLinksEncontrados = linksEncontrados.Count,
            NoticiaGeradaPelaIA = noticiaFinal
        });
    }

    return Results.Ok(relatorioGeral);
});

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

app.MapPut("/api/newsletters", ([FromBody] Newsletter newsletter) =>
{
    var news = newsletters.FirstOrDefault(n => n.Id == newsletter.Id);
    if (news != null)
    {
        news.Title = newsletter.Title;
        return Results.Ok("Notícia atualizada");
    }

    return Results.NotFound("Notícia não encontrada");
});

app.MapDelete("/api/newsletters/{id}", ([FromRoute] Guid id) =>
{
    var news = newsletters.FirstOrDefault(n => n.Id == id);
    if (news != null)
    {
        newsletters.Remove(news);
        return Results.Ok("Removido com sucesso!");
    }

    return Results.NotFound("Newsletter não encontrada!");
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

app.MapPut("/api/users", ([FromBody] User user) =>
{
    var u = users.FirstOrDefault(u => u.Id == user.Id);
    if (u != null)
    {
        u.Name = user.Name;
        return Results.Ok("Usuário atualizado");
    }

    return Results.NotFound("Usuário não encontrado");
});

app.MapDelete("/api/users/{id}", ([FromRoute] Guid id) =>
{
    var u = users.FirstOrDefault(user => user.Id == id);
    if (u != null)
    {
        users.Remove(u);
        return Results.Ok("Removido com sucesso!");
    }

    return Results.NotFound("Usuário não encontrado!");
});

app.MapGet("/", () => "API está rodando perfeitamente!");

app.Run();