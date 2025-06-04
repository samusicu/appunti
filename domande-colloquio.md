## Dependency injection
La dependency injection è un principio della programmazione orientata agli oggetti che riguarda la gestione delle dipendenze tra oggetti. In parole semplici, una ‘dipendenza’ è un altro oggetto di cui una classe ha bisogno per funzionare.

Invece di creare direttamente queste dipendenze all’interno della classe — il che renderebbe il codice più rigido e difficile da testare — si usa la dependency injection per fornire queste dipendenze dall’esterno, tipicamente attraverso il costruttore, un metodo setter o un’iniezione diretta nei campi.
Questo approccio ha diversi vantaggi:

- Maggiore testabilità: possiamo facilmente sostituire le dipendenze reali con oggetti finti (mock o stub) durante i test.

- Bassa accoppiatura: le classi diventano meno dipendenti da implementazioni specifiche, facilitando il riutilizzo e la manutenzione del codice.

- Maggiore flessibilità: è più facile cambiare il comportamento dell’applicazione modificando le configurazioni, senza dover riscrivere le classi.

Un esempio concreto: supponiamo di avere una classe OrderService che ha bisogno di un oggetto PaymentProcessor. Con la dependency injection, PaymentProcessor viene passato come parametro al costruttore di OrderService, invece di essere creato con new PaymentProcessor() dentro OrderService. Questo rende più semplice cambiare il tipo di pagamento o testare il servizio senza dover modificare la classe stessa.

## API REST 
Un'API REST (Representational State Transfer) è uno stile architetturale per la progettazione di servizi web. Le API RESTful si basano sul protocollo HTTP e consentono a sistemi diversi di comunicare tra loro in modo semplice e scalabile, utilizzando operazioni standard.

Le principali caratteristiche di una REST API includono:

Stateless: ogni richiesta contiene tutte le informazioni necessarie; il server non mantiene lo stato tra le richieste.

Risorse identificate da URI: ogni "cosa" (es. utenti, prodotti) è rappresentata da un URI.

Uso di metodi HTTP standard per eseguire operazioni sulle risorse (GET, POST, PUT, DELETE).

## Autenticazione e Autorizzazione
I concetti di autenticazione e autorizzazione sono due fasi distinte ma spesso collegate nel controllo degli accessi:

- Autenticazione (“Chi sei?”): È il processo in cui un utente prova la propria identità.Tipicamente avviene fornendo credenziali: username/password, token, certificato, ecc.
- Autorizzazione (“Cosa puoi fare?”): Avviene dopo l’autenticazione. Serve per determinare quali risorse o azioni sono permesse all’utente. È basata su ruoli (es. admin, utente) o permessi specifici.

Es. Auth. Jwt: Nel login, il frontend invia le credenziali. Il backend verifica e restituisce un JWT firmato, che il frontend salva. Nelle richieste successive, il token viene inviato nell’header Authorization, e il backend lo verifica per autenticare e autorizzare l’utente.

Il JWT da solo non è cifrato, ma è sicuro se trasmesso su HTTPS, che protegge contro l’intercettazione. Il token è firmato digitalmente, quindi non può essere alterato senza invalidarsi (la firma è creata da un algoritmo di hash + chiave segreta, di solito HS256, e quando la chiamata arriva al backend questo ricalcola la firma partendo dalla chiave segreta per vedere se combaciano).

## Come proteggere un endpoint
- Autenticazione: JWT / OAuth2 / Sessioni per sapere se la richiesta proviene da un utente autenticato.
- Protezione tramite HTTPS: Impongo che tutte le comunicazioni avvengano solo su HTTPS, mai HTTP (crittografa il traffico e protegge da man-in-the-middle)
- Rate limiting / throttling: Evito abusi e attacchi come brute force o denial of service. Es. massimo 10 richieste/minuto per IP.
-  Validazione input e gestine eccezioni: Mai fidarsi dell’input dell’utente!
- CORS (Cross-Origin Resource Sharing): Limita quali origini possono accedere alle API.

## Cos'è la statelessness in un'API REST?
Un'API REST è stateless se ogni richiesta del client al server contiene tutte le informazioni necessarie per essere compresa ed elaborata, indipendentemente da richieste precedenti.
Il server non memorizza alcun contesto o stato del client tra una richiesta e l'altra.

✅ Esempio pratico:
Supponiamo tu stia creando un'applicazione che richiede al server i dettagli di un utente autenticato.

- In un'API stateless, ogni richiesta dovrebbe includere tutte le informazioni, ad esempio:

    GET /profile
    Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR...
    Qui il token di accesso contiene l’identità dell’utente, e il server può elaborare la richiesta senza sapere nulla di richieste precedenti.

    Nessuna "sessione" salvata lato server.

- In un'app stateful, il server potrebbe salvare in memoria una sessione per ogni client (come avviene spesso nelle applicazioni web tradizionali). Ad esempio:

    Il client fa il login.

    Il server memorizza che il client ha fatto login (es. Session["user"] = Mario).

    Le richieste successive non contengono credenziali, perché il server ricorda lo stato del client.

    Questo viola il principio REST di statelessness.

🔧 Vantaggi della statelessness REST:
- Scalabilità più semplice (i server non devono condividere lo stato tra loro)

- Più resilienza (ogni richiesta è indipendente)

- Più semplicità nel caching e load balancing

- Compatibilità con architetture serverless e microservizi

🧠 Ricorda:
"Stateless" non significa che non esiste alcuno stato, ma che lo stato viene gestito interamente dal client o in modo esplicito (es. tramite token).

## Buone pratiche per codice pulito e manutenibile
- SRP (Single Rensponsibility Principle): Ogni modulo, classe o funzione dovrebbe avere una sola responsabilità ben definita, evita classi o funzioni “giganti” che fanno troppe cose.
- Codice modulare: Suddividi il codice in livelli chiari: controller, servizi/business logic, repository/data access.
- Nomi chiari e descrittivi: Usa nomi di variabili, funzioni e classi che descrivono chiaramente il loro scopo.
- Gestione errori centralizzata	Debug più semplice
- Test automatici	Codice affidabile e meno bug
- DRY, Evitare duplicazioni	Meno errori e più manutenibilità

 