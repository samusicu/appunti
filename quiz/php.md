# Test 1

Domanda 1  
Come si può cancellare un cookie in PHP?

a.
Chiamando la funzione delete_cookie()

b.
Rimuovendo il file cookie dal server

c.
Utilizzando la funzione unset()

d.
Impostando l'orario di scadenza del cookie a una data passata

La risposta corretta è: Impostando l'orario di scadenza del cookie a una data passata  

Domanda 2  
Dove viene memorizzato un cookie?

a.
Nella sessione PHP

b.
Sul dispositivo dell'utente

c.
Nel database del sito web

d.
Sul server

La risposta corretta è: Sul dispositivo dell'utente  

Domanda 3  
Che cos'è un cookie nel contesto dello sviluppo web?

a.
Un piccolo file di testo memorizzato sul dispositivo dell'utente dal browser web

b.
Un linguaggio di programmazione utilizzato per lo scripting lato client

c.
Uno script lato server per la gestione delle sessioni

d.
Un tipo di database utilizzato per memorizzare i dati dell'utente

La risposta corretta è: Un piccolo file di testo memorizzato sul dispositivo dell'utente dal browser web  

Domanda 4  
Quale funzione PHP viene utilizzata per impostare un cookie?

a.
create_cookie()

b.
define_cookie()

c.
setcookie()

d.
set_cookie()

La risposta corretta è: setcookie()  

Domanda 5  
Quale attributo del cookie assicura che esso venga inviato solo su connessioni sicure?

a.
Impostando il flag Secure a true

b.
Memorizzandolo nel database del server

c.
Utilizzando il flag http_only

d.
Utilizzando il protocollo HTTPS

La risposta corretta è: Impostando il flag Secure a true  

Domanda 6  
Per quanto tempo può persistere un cookie sul dispositivo di un utente se non è impostato un tempo di scadenza?

a.
Fino a quando l'utente lo elimina manualmente

b.
Fino a quando il browser viene chiuso

c.
Fino a quando la sessione termina

d.
Per sempre

La risposta corretta è: Fino a quando il browser viene chiuso  

Domanda 7  
Qual è il percorso predefinito di un cookie in PHP se non specificato?

a.
La home directory dell'utente

b.
La directory di cache del browser

c.
La directory radice (/)

d.
La directory corrente in cui è in esecuzione lo script

La risposta corretta è: La directory corrente in cui è in esecuzione lo script  

Domanda 8  
Quale attributo di un cookie impedisce l'accesso al cookie da parte di JavaScript?

a.
L'attributo Path

b.
L'attributo Domain

c.
L'attributo Secure

d.
L'attributo HttpOnly

La risposta corretta è: L'attributo HttpOnly  

Domanda 9  
 Come è possibile impostare un cookie con una durata di 30 giorni in PHP?

a.
Utilizzando un parametro booleano

b.
Utilizzando un flag di sessione

c.
Usando time() + (86400 * 30) per impostare l'ora di scadenza

d.
Usando time() + (3600 * 24) per impostare l'ora di scadenza

La risposta corretta è: Usando time() + (86400 * 30) per impostare l'ora di scadenza  

Domanda 10  
Quale di questi metodi è valido per accedere a un cookie in PHP?

a.
$_COOKIE['cookie_name']

b.
$_POST['cookie_name']

c.
$_SESSION['cookie_name']

d.
$_GET['cookie_name']

La risposta corretta è: $_COOKIE['cookie_name']


# Test 2

Domanda 1  
Dove vengono generalmente memorizzati i dati di sessione?

a.
Nella memoria cache del browser

b.
In un database del sito web

c.
Sul dispositivo dell'utente

d.
In un file sul server

La risposta corretta è: In un file sul server

Domanda 2  
Quale direttiva di configurazione PHP determina la durata della sessione?

a.
session.timeout

b.
session.save_path

c.
session.gc_maxlifetime

d.
session.cookie_lifetime

La risposta corretta è: session.gc_maxlifetime

Domanda 3  
Come si accede ai dati memorizzati in una sessione in PHP?

a.
$_POST['session_var']

b.
$_GET['session_var']

c.
$_SESSION['session_var']

d.
$_COOKIE['session_var']

La risposta corretta è: $_SESSION['session_var']

Domanda 4  
Come si distrugge una sessione in PHP?

a.
Eliminando i file di sessione dal server

b.
Chiudendo il browser dell'utente

c.
Rimuovendo i cookie associati alla sessione

d.
Chiamando session_destroy()

La risposta corretta è: Chiamando session_destroy()

Domanda 5  
Qual è lo scopo principale delle sessioni in PHP?

a.
Generare cookie sicuri per l'utente

b.
Memorizzare temporaneamente i dati dell'utente tra diverse pagine web

c.
Archiviare permanentemente i dati dell'utente sul server

d.
Creare copie di backup del database

La risposta corretta è: Memorizzare temporaneamente i dati dell'utente tra diverse pagine web

Domanda 6  
Cosa accade ai dati di sessione quando viene chiusa la sessione?

a.
Vengono trasferiti nel database

b.
Vengono persi se non sono stati salvati

c.
Vengono conservati fino alla scadenza della sessione

d.
Vengono automaticamente salvati in un file di log

La risposta corretta è: Vengono conservati fino alla scadenza della sessione

Domanda 7  
Quale funzione PHP viene utilizzata per iniziare una sessione?

a.
session_start()

b.
session_init()

c.
session_begin()

d.
start_session()

La risposta corretta è: session_start()

Domanda 8  
Come si può rigenerare l'ID di sessione in PHP per migliorare la sicurezza?

a.
Utilizzando session_id() con un nuovo valore

b.
Rimuovendo il vecchio ID e creando un nuovo cookie

c.
Utilizzando session_start() con un parametro di sicurezza

d.
Chiamando session_regenerate_id()

La risposta corretta è: Chiamando session_regenerate_id()

Domanda 9  
Quale delle seguenti affermazioni è vera riguardo alle sessioni?

a.
Le sessioni memorizzano i dati lato client

b.
Le sessioni scadono automaticamente quando l'utente chiude il browser

c.
Le sessioni possono essere utilizzate per memorizzare grandi quantità di dati persistenti

d.
Le sessioni sono visibili e modificabili dal client

La risposta corretta è: Le sessioni scadono automaticamente quando l'utente chiude il browser

Domanda 10   
Cosa succede se chiami session_start() più volte in uno script PHP?

a.
Viene generato un errore fatale

b.
La funzione ignora le chiamate successive e continua con la sessione esistente

c.
La sessione viene riavviata ogni volta

d.
La sessione viene chiusa e riaperta

La risposta corretta è: La funzione ignora le chiamate successive e continua con la sessione esistente