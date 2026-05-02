using HtmlAgilityPack;

namespace Api.Services;

public class MotorsportScraperService
{
    public async Task<List<string>> ObterLinksAsync(string urlBase)
    {
        var web = new HtmlWeb();
        web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

        var document = await web.LoadFromWebAsync(urlBase);
        var linksLimpinhos = new List<string>();
        var todosOsLinks = document.DocumentNode.SelectNodes("//a[@href]");

        if (todosOsLinks == null) return linksLimpinhos;

        foreach (var linkNode in todosOsLinks)
        {
            string href = linkNode.GetAttributeValue("href", string.Empty);

            if (href.Contains("/news/") && href.Length > 25)
            {
                string linkCompleto = href.StartsWith("http") ? href : $"https://motorsport.uol.com.br{href}";

                if (!linksLimpinhos.Contains(linkCompleto))
                {
                    linksLimpinhos.Add(linkCompleto);
                }
            }
        }

        return linksLimpinhos;
    }

    public async Task<string> ObterTextoDaNoticiaAsync(string urlNoticia)
    {
        var web = new HtmlWeb();
        web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

        var document = await web.LoadFromWebAsync(urlNoticia);
        var paragrafos = document.DocumentNode.SelectNodes("//p");

        if (paragrafos == null) return "Texto não encontrado na página.";

        var textosLimpos = paragrafos
            .Select(p => p.InnerText.Trim())
            .Where(texto => texto.Length > 50);

        return string.Join("\n\n", textosLimpos);
    }
}