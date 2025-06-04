## Cosa Accade Dietro le Quinte quando Usate Entity Framework?
Quando si eseguono delle operazioni tramite EF, accadono diverse cose dietro le quinte:

- Tracking delle entità:  
EF tiene traccia dello stato di ogni oggetto (ad esempio, se è stato aggiunto, modificato o eliminato) per decidere quali operazioni SQL inviare al database.

- Lazy Loading:  
Quando si recuperano oggetti dal database, EF può recuperare dati correlati solo quando necessario, invece di caricarli subito. Questo è utile per evitare il sovraccarico di caricamenti non necessari, ma può anche causare delle prestazioni inferiori se usato in modo improprio.

- Eager Loading:  
Può essere usato per caricare insieme alle entità principali anche le entità correlate in un'unica query (ad esempio, utilizzando .Include()), riducendo il numero di round-trip al database.

- Query Generata:  
EF trasforma le operazioni LINQ in query SQL ottimizzate, ma non sempre la generazione della query è perfetta. A volte potrebbe generare query inefficienti che richiedono ottimizzazioni manuali.

## Ottimizzazione dei Flussi di Business tramite Entity Framework
EF è utile per ottimizzare i flussi di business, ma bisogna prestare attenzione ad alcune pratiche per ottenere il massimo delle performance. Questi sono alcuni aspetti da considerare:

- Minimizzare le query:

    - Lazy Loading vs Eager Loading: Usare Eager Loading (ad esempio, con .Include()) riduce il numero di query e fa sì che tutte le informazioni siano caricate in un'unica operazione.
    - No N+1 Queries: Assicurati che EF non stia eseguendo una query separata per ogni entità correlata (problema comune di "N+1 query"). Puoi ottimizzare questo aspetto con il Bulk Loading.

- Transazioni e Concorrenza:

    - Utilizzare le transazioni per garantire che tutte le operazioni di un flusso di business vengano completate correttamente, in modo da mantenere la consistenza del database.
    - Gestire la concorrenza ottimisticamente (con versioni o timestamp nelle tabelle) per evitare conflitti in caso di accessi simultanei.

- Gestione dei Carichi Elevati:

    - Per carichi di lavoro ad alta intensità, considera l'uso di query batch o l'invio di query SQL personalizzate per ottimizzare operazioni complesse che non sono ben gestite da EF.
    - Utilizzare compilation di query per evitare di rigenerare lo stesso piano di esecuzione delle query ripetute.

- Caching:

    - EF non ha un proprio sistema di caching, ma puoi integrare una libreria di caching per memorizzare le risposte delle query che non cambiano frequentemente.
- Sincronizzazione asincrona:

    - Utilizzare metodi asincroni come SaveChangesAsync() e ToListAsync() per migliorare la reattività dell'applicazione in ambienti ad alta concorrenza.

## Strumenti di Diagnostica e Ottimizzazione
- SQL Profiler: Puoi usare strumenti come SQL Profiler per monitorare le query generate da EF e identificare eventuali inefficienze.

- EF Performance Profiler: Ci sono anche strumenti più specifici per Entity Framework che ti aiutano a misurare le performance e identificare i colli di bottiglia nelle operazioni di accesso al database.

- Lazy Loading vs Eager Loading: Fai attenzione alla strategia di caricamento dei dati. Eager Loading può essere utile per caricare tutte le informazioni richieste in un'unica query, ma può diventare costoso se usato in modo eccessivo.

## Entity Tracking
Quando esegui una query con EF Core, le entità vengono tracciate per impostazione predefinita dal contesto (DbContext). Questo significa che:
- Se modifichi le proprietà di un'entità, EF Core rileva automaticamente le modifiche.
- Quando chiami SaveChanges(), EF Core aggiorna automaticamente il database con le modifiche rilevate.
- Se carichi di nuovo la stessa entità, il contesto potrebbe restituire la versione già in memoria, evitando una nuova query al database.
### No Tracking
Per migliorare le prestazioni, EF Core consente di disabilitare il tracking quando non è necessario (ad esempio, per operazioni di sola lettura).  
Se non vuoi che EF Core tenga traccia delle modifiche (per migliorare le prestazioni nelle query di sola lettura), puoi usare AsNoTracking():

    using (var context = new MyDbContext())
    {
        var customer = context.Customers.AsNoTracking().FirstOrDefault(c => c.Id == 1);
        customer.Name = "Nuovo Nome";
        context.SaveChanges(); // Non funzionerà automaticamente perché l'entità non è tracciata!
    }
Dopo AsNoTracking(), le modifiche non vengono rilevate automaticamente e devi esplicitamente dire a EF di aggiornare l'entità:

    context.Customers.Update(customer);
    context.SaveChanges();

## Loading
### Lazy Loading
Quando carichi un'entità principale, le sue proprietà di navigazione (relazioni con altre entità) non vengono caricate subito; se accedi a una proprietà di navigazione, EF esegue una query separata per recuperare i dati mancanti.  
Esempio:

    // Due entità con rapporto 1:N (studente:corsi)
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        // virtual -> Lazy Loading attivo
        public virtual List<Course> Courses { get; set; }
    }

    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }


    // Esempio di query
    using (var context = new MyDbContext())
    {
        var student = context.Students.FirstOrDefault(s => s.Id == 1);
        Console.WriteLine(student.Name); // Query per recuperare lo studente

        Console.WriteLine(student.Courses.Count); // Seconda query per caricare i corsi (Lazy Loading)
    }

Questa scelta è vantaggiosa se le proprietà di navigazione sono poco usate, altrimenti vengono generate molteplici query (N+1 Query Problem).

**Nota**: In EF Core, il Lazy Loading non è attivo per default. Devi installare il pacchetto Microsoft.EntityFrameworkCore.Proxies e abilitare le proxy:

    optionsBuilder.UseLazyLoadingProxies();

altrimenti provando ad accedere ad una proprietà di navigazione questa risulterà null.

## Eager Loading
L'Eager Loading carica subito tutti i dati correlati in un'unica query, evitando le query multiple del Lazy Loading; si usa il metodo .Include() per specificare che vogliamo caricare subito le proprietà di navigazione.
Esempio:

    using (var context = new MyDbContext())
    {
        var student = context.Students
                            .Include(s => s.Courses) // Carica subito i corsi
                            .FirstOrDefault(s => s.Id == 1);

        Console.WriteLine(student.Name);
        Console.WriteLine(student.Courses.Count); // Nessuna query aggiuntiva
    }

Questa scelta è vantaggiosa solo se i dati servono effettivamente, perchè pur generando un unica query questa è più complessa e alloca più memoria.

## Explicit Loading (EF Core)
Il DbContext in EF Core fornisce il metodo .Entry() per ottenere l'accesso a un'entità specifica e permette di caricare manualmente:

- Collezioni (relazioni uno-a-molti o molti-a-molti) con .Collection().Load()  
- Singole entità (relazioni uno-a-uno o molti-a-uno) con .Reference().Load()

Si ottiene lo stesso effetto del lazy loading ma il caricamento è manuale, quindi è utile quando vuoi più controllo sul caricamento dei dati ed evitare query non necessarie.

## Diagnostica
Per visualizzare le query generate da EF:
- SQL Server Profiler
- sul program:

        builder.Services.AddDbContext<MyDbContext>((serviceProvider, options) =>
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            options.UseSqlServer("connection_string")
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging();
        });