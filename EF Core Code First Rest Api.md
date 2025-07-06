## EF Core Code First Rest API

1) Installa Entity Framework

        Install-Package Microsoft.EntityFrameworkCore;
        Install-Package Microsoft.EntityFrameworkCore.SqlServer;
        Install-Package Microsoft.EntityFrameworkCore.Tools

2) aggiungi stringa di connessione su appsetting.json

        "ConnectionStrings": {
            "DefaultConnection": "Server=.\\SQLEXPRESS;Database=dbname;Trusted_Connection=True;TrustServerCertificate=True;"
        }

3) aggiungi la cartella Model e crea le classi corrispondenti alle tabelle
annotazioni [Key] per la pk, [Required] per i campi non nullable ecc...

        public class User
        {
            [Key]
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
        }

3) Aggiungi la classe ApplicationDbContext che estende DbContext
deve avere un costruttore che richiama la classe genitore mandando le opzioni
dentro deve avere le proprietà DbSet per ogni tabella 

        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

            public DbSet<User> Users { get; set; }
        }

4) aggiungi il DbContext al program

        builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
            ));


5) aggiungi migrazioni

        add-migration NomeMigration 
        update-database 

6) opz: se sei su un progetto separato (senza program) serve una classe ContextFactory

        public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

                // Replace with your actual connection string
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=TestCodeFirst;Trusted_Connection=True;TrustServerCertificate=True;");

                return new AppDbContext(optionsBuilder.Options);
            }
        }

7) opz: classe con relazioni n:n

        public class Order
        {
            [Key]
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public int ProductId { get; set; }
            [ForeignKey("ProductId")] public Product Product { get; set; }
            [Required]
            public int UserId { get; set; }
            [ForeignKey("UserId")] public User User { get; set; }
            [Required]
            public int Quantity { get; set; }
        }

8) opz: automapper
// attento che siano della stessa versione
Install-Package AutoMapper
Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection

        //crea mappingProfile
        public class MappingProfile : Profile
        {
            public MappingProfile() 
            {
                CreateMap<Product, ProductDto>()
                    .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.BrandName))
                    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.CategoryName));
            }
        }

        //usa mapping (tipo nel service)
        var products = await context.Products.ToListAsync();
        return mapper.Map<IEnumerable<ProductDto>>(products);