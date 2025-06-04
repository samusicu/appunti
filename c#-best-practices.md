## Throw Exceptions
non rilanciare l'errore dentro il catch

    void ThrowIfNotOk (string input)
    {
        try {
            if (input != "ok")
                throw new Exception("not ok");
        } catch (Exception e){
            Console.Writeline(e.Message);
            throw e; // !!! non si fa !!!
        }
    }


## Async/Await e Task
ricordati queste keyword per metodi asincroni

    public async Task FetchAsync()
    {
        await Task.Delay();
        throw ner InvalidOperationException("Fallito");
    }

    // richiama il metodo con await
    await FetchAsync();


## Using 
ricorda questa keyword per i file

    public void WriteToFile(string filePath)
    {
        // !!! male !!!
        var writer = new StreamWriter(fileP8ath);
        writer.Write("prova");
        // non chiude il file, rimane in memoria

        using ( var writer = new StreamWriter(filePath))
        writer.Write("prova");
        // chiude il file appena esce dalla funzione
    }
    

## String vs StringBuilder
la stringa è un oggetto immutabile, ogni volta che la modifichi ne crei una nuova, vale anche per la concatenazione

    // concatenazione di un array di stringhe
    string result = "";
    foreach (var item in items)
    {
        result += item + ",";
    }
    return result; // pessimo in termini id allocazione in memoria

StringBuilder esiste proprio per risolvere questo problema 

    var builder = new System.Text.StringBuilder();
    foreach(var item in items)
    {
        builder.Append(item);
        builder.Append(",");
    }
    return builder.ToString(); // molto più efficiente

## Riusare HttpClient
se dichiari HttpClient dentro un metodo verrà ricreato ogni volta. Molto inefficiente e rischia di mandare in errore la connessione (socket exhaustion o esaurimento delle porte disponibili)

    public async Task<string> GetDataAsync(string url)
    {
        using var client = new HttpClient(); // !!!
        var response = await client.GetAsync(url);
        return response;
    }

HttpClient deve essere una proprietà del service

    public class MyService
    {
        private static readonly HttpClient _client = new HttpClient();

        public async Task<string> GetDataAsync(string url)
        {
            var response = await _client.GetAsync(url);
            return response;
        }
    }

### IHttpClientFactory in Asp.NET Core

Startup.cs / Program.cs

    builder.Services.AddHttpClient("GitHub", client =>
    {
        client.BaseAddress = new Uri("https://api.github.com/");
        client.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp");
    });

Classe consumer

    public class MyService
    {
        private readonly HttpClient _client;

        public MyService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("GitHub");
        }

        public async Task<string> GetDataAsync()
        {
            var response = await _client.GetAsync("/repos/dotnet/runtime");
            return await response.Content.ReadAsStringAsync();
        }
    }


// possibili estensioni  
 C# Best Practices  
Usa nomi significativi per variabili, metodi e classi

Applica il principio SOLID

Evita la logica nel costruttore

Prediligi l’interfaccia rispetto alla classe concreta

Usa le eccezioni con criterio

Prediligi la composition over inheritance

Scrivi codice immutabile ove possibile

Usa async/await per operazioni I/O

Applica pattern di progettazione quando opportuno

Usa null-coalescing operator e null-conditional operator

🟦 ASP.NET (MVC / Web API) Best Practices  
Separa chiaramente i livelli (Controller, Service, Repository)

Usa Dependency Injection

Valida sempre i dati in input (ModelState, Data Annotations)

Gestisci centralmente gli errori (middleware o filter)

Evita la logica nei controller, mantienili "thin"

Usa DTO e AutoMapper per l'esposizione dei dati

Proteggi le API con autenticazione/autorizzazione

Usa Rate Limiting e CORS

Scrivi Unit Test e Integration Test

Minimizza la superficie esposta (nascondi endpoint non usati)

🟦 Entity Framework Best Practices  
Usa DbContext come scoped (per request)

Evita query N+1 con Include()

Non esporre direttamente le entità EF (usa DTO)

Usa AsNoTracking() quando non serve il tracking

Applica migrations in modo controllato

Indica chiavi primarie e foreign key esplicitamente

Usa Fluent API per configurazioni complesse

Evita il caricamento lazy quando non necessario

Mantieni le query performanti (usa proiezioni con Select)

Gestisci la concorrenza con RowVersion o simili

