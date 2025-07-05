## EF Core DB First Rest API

1) Installa Entity Framework

        Install-Package Microsoft.EntityFrameworkCore;
        Install-Package Microsoft.EntityFrameworkCore.SqlServer;
        Install-Package Microsoft.EntityFrameworkCore.Tools

2) Crea i model a partire dal db 

        Scaffold-DbContext "Server=NB-0076\SQLEXPRESS;Database=DatanaseName;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

3) Nel file appsettings.json aggiungere la connection string per accesso al database

        "ConnectionStrings": {
        "DefaultConnection": "Server=.\\SQLEXPRESS;Database=DbName;Trusted_Connection=True;TrustServerCertificate=True;"
        },

4) Aggiungere nel Program il servizio per Entity Framework

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<NewContext>(options =>
            options.UseSqlServer(connectionString));

5) Creare controller Rest:  
    - [cartella Controller] -> tasto dx -> "aggiungi" -> "nuovo elemento di scaffolding"  
    - Seleziona "Controller API con azioni, che usa Entity Framework"  
    - Seleziona il context e il model da usare
  
6) Aggiungere Swagger  

    Install-Package Swashbuckle.AspNetCore

        // in Program

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build(); //questo c'è già

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    
        // opzionale, per aprirsi all'avvio, in launchSettings

        "https": {
            "commandName": "Project",
            "dotnetRunMessages": true,
            "launchBrowser": true, // cambia questo a true
            "launchUrl": "swagger", // aggiungi questo
            ...
        }

7) Crea DTO per evitare errori di ciclo

        // in Movie
        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
        // in Genre
        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
        //così si crea un ciclo infinito


        public class MovieDto
        {
            public int MovieId { get; set; }
            public string Title { get; set; }
            public int Year { get; set; }
            public List<GenreDto> Genres { get; set; }
        }

        public class GenreDto
        {
            public int GenreId { get; set; }
            public string Name { get; set; }
        }

        var movies = await _context.Movies
            .Include(m => m.Genres)
            .Select(m => new MovieDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Year = m.Year,
                Genres = m.Genres.Select(g => new GenreDto
                {
                    GenreId = g.GenreId,
                    Name = g.GenreName
                }).ToList()
            })
            .ToListAsync();

8) Cors

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("PermettiFrontend", builder =>
            {
                builder
                    .WithOrigins("http://127.0.0.1:5500")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        /*...*/

        app.UseCors("PermettiFrontend");

9) Frontend

Html:

    <div class="container py-5">
        <div class="table-responsive">
        <table class="table table-striped table-hover" id="resultsTable">
            <thead class="table-dark">
            <tr>
                <th>Title</th>
                <th>Year</th>
                <th>Genres</th>
            </tr>
            </thead>
            <tbody>
            <!-- I risultati verranno inseriti qui -->
            </tbody>
        </table>
        </div>
    </div>

Javacript:

    fetch("https://localhost:7157/api/Movies",{
        method : 'GET',
        headers: {
        'Content-Type': 'application/json'
    }
    }).then(response =>{
        if(!response.ok)
            throw new Error('Errore nella richiesta GET');
        return response.json();
    }).then(data => {
        console.log(data);
        const tableBody = document.getElementById('resultsTable').querySelector('tbody');
        tableBody.innerHTML = ''; 

        data.forEach(movie => {
        const row = document.createElement('tr');

        row.innerHTML = `
        <td>${movie.title}</td>
        <td>${movie.year}</td>
        <td>${movie.genres.map(g => g.name).join(", ")}</td>
        `;

        tableBody.appendChild(row);
    });
    })
    .catch(error => {
    console.error('Errore:', error);
    });