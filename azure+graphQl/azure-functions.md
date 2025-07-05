## Livelli di autorizzazione
- Anonymous: accesso pubblico senza chiave, nessuna sicurezza, chiunque con l’URL può accedere.
- Function: richiede una function key (?code=<key>):
    - Chiave di funzione: specifica per una sola funzione.
    - Host key: condivisa tra tutte le funzioni dell'app.
- Admin: accesso solo con host-level admin key.
- User: Deprecato, non più usato in Azure Functions.
- System: Utilizzato da Azure per processi interni (scaling, timer trigger, ecc.).

## In Process vs Isolated
Nel modello in-process, il codice viene eseguito nello stesso processo del runtime di Azure Functions. Nel modello isolated, il codice viene eseguito in un processo separato.

### In-Process

    ┌──────────────────────────────────────────────┐
    │         Azure Functions Host (Runtime)       │
    │  ┌──────────────────────────────────────────┐│
    │  │         Funzioni statiche registrate     ││
    │  │  ┌────────────────────────────────────┐  ││
    │  │  │   [FunctionName("Func1")]          │  ││
    │  │  │   public static Task Run(...)      │  ││
    │  │  └────────────────────────────────────┘  ││
    │  │                                          ││
    │  │  DI limitata via FunctionsStartup:       ││
    │  │  ┌────────────────────────────────────┐  ││
    │  │  │ Configure(IFunctionsHostBuilder)   │  ││
    │  │  │ - AddSingleton<Svc1>()             │  ││
    │  │  └────────────────────────────────────┘  ││
    │  └──────────────────────────────────────────┘│
    └──────────────────────────────────────────────┘

- ✅Performance migliori (meno overhead)
- ✅Meno complessità iniziale
- ❌Legato alla versione .NET del runtime Azure Functions
- ❌Meno controllo sull'ambiente di esecuzione
- ❌Condivisione di risorse può causare conflitti
- ❌Meno flessibilità nel dependency injection

### Isolated:

    ┌──────────────────────────────────────────────┐
    │          Azure Functions Host (gRPC)         │
    │        (Gestisce il ciclo vita esterno)      │
    └──────────────────────────────────────────────┘
                    │
        comunica via RPC (gRPC)
                    ▼
    ┌──────────────────────────────────────────────┐
    │         Worker Process (.NET Isolated)       │
    │  ┌──────────────────────────────────────────┐│
    │  │          Generic Host (.NET)             ││
    │  │   ┌────────────────────────────────────┐ ││
    │  │   │         DI Container               │ ││
    │  │   │   ┌────────┐   ┌────────┐          │ ││
    │  │   │   │ Func1  │   │  Svc1  │          │ ││
    │  │   │   └────────┘   └────────┘          │ ││
    │  │   │    (injected in Func1)             │ ││
    │  │   └────────────────────────────────────┘ ││
    │  └──────────────────────────────────────────┘│
    └──────────────────────────────────────────────┘

- ❌Leggero overhead nelle performance
- ❌Complessità iniziale maggiore
- ✅Controllo totale sulla versione .NET (.NET 6, 7, 8+)
- ✅Isolamento completo - errori non impattano il runtime
- ✅Middleware personalizzato per logging, autenticazione, ecc.
- ✅Dependency injection avanzato
- ✅Supporto futuro - Microsoft sta investendo qui

### Nel codice:
- **Static**: In-Process accetta solo classi statiche, Isolated accetta tutto ed è consigliato usarle  non-static
- **Asp.net**: In-process usa direttamente ASP.NET Core. Isolated invece non carica ASP.NET Core, quindi non puoi accedere a middleware, routing o HttpRequest. Questo obbliga l’uso di oggetti specifici come HttpRequestData

        using Microsoft.AspNetCore.Http; //in-process 
        using Microsoft.Azure.Functions.Worker.Http //isolated

        ...

        // nei parametri del metodo 
        [HttpTrigger(AuthorizationLevel.Function,"post")] /*...*/
        HttpRequest req //in-process
        HttpRequestData req //isolated

- **Logging**: In contesto in-process è gestito implicitamente. In Isolated hai il pieno controllo tramite FunctionContext

        //in-process: ILogger iniettato nel metodo
        log.LogInformation("message");

        //isolated: FunctionContext iniettato
        var logger = context.GetLogger("HttpTriggerIsolated");
        logger.LogInformation("message");

- **Risposta**: L’assenza del binding automatico in Isolated richiede di creare manualmente le risposte HTTP. Questo è più verboso, ma consente maggiore flessibilità 

        //in-process
        return new OkObjectResult($"response");

        //isolated: creazione response esplicita
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync($"response");
        return response;

- **Durable**: Il nuovo SDK Microsoft.DurableTask per Isolated è pensato per essere più modulare, supportando architetture future-proof. Si interfaccia direttamente con Durable Task Platform, con meno vincoli del vecchio SDK.

        //orchestrator
        [OrchestrationTrigger] IDurableOrchestrationContext context //in-process
        [OrchestrationTrigger] TaskOrchestrationContext context //isolated

        //trigger
        [DurableClient] IDurableOrchestrationClient client //in-process
        [DurableClient] DurableTaskClient client //isolated

## Ciclo di vita delle dipendenze
L'iniezione delle dipendenze può avvenire in tre modi: Singleton, Transient, Scoped.  
Esempio: Il servizio OrderService viene iniettato nella classe OrderFunctions.

- Transient: istanze separate ogni volta che viene usata la dipendenza, anche se nella stessa richiesta

        ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
        │ OrderFunctions  │    │ OrderFunctions  │    │ OrderFunctions  │
        │    Instance A   │    │    Instance B   │    │    Instance C   │
        │       │         │    │       │         │    │       │         │
        │  OrderService   │    │  OrderService   │    │  OrderService   │
        │    Instance A   │    │    Instance B   │    │    Instance C   │
        └─────────────────┘    └─────────────────┘    └─────────────────┘
                ↓                       ↓                       ↓ 
                └───────────────────────┼───────────────────────┘
                                 ISTANZE DIVERSE


- Scoped: ogni richiesta Http crea una nuova istanza 

                        Richiesta 1                       Richiesta 2
                ┌───────────────────────┐                     │
                ↓                       ↓                     ↓
        ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
        │ OrderFunctions  │    │ OrderFunctions  │    │ OrderFunctions  │
        │    Instance A   │    │    Instance B   │    │    Instance C   │
        │       │         │    │       │         │    │       │         │
        │  OrderService   │    │  OrderService   │    │  OrderService   │
        │    Instance X   │    │    Instance X   │    │    Instance Y   │
        └─────────────────┘    └─────────────────┘    └─────────────────┘
                ↑                      ↑                      ↑ 
                └──────────────────────┘                      │
                    STESSA ISTANZA                         DIVERSA


- Singleton: tutte le invocazioni usano la stessa istanza

        ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
        │ OrderFunctions  │    │ OrderFunctions  │    │ OrderFunctions  │
        │    Instance A   │    │    Instance B   │    │    Instance C   │
        │       │         │    │       │         │    │       │         │
        │  OrderService   │    │  OrderService   │    │  OrderService   │
        │    Instance X   │    │   Instance X    │    │   Instance X    │
        └─────────────────┘    └─────────────────┘    └─────────────────┘
                    ↑                 ↑                     ↑
                    └─────────────────┼─────────────────────┘
                                STESSA ISTANZA




