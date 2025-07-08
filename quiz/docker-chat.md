- FROM – Definisce l’immagine base.

- RUN – Esegue comandi nella fase di build.

- COPY / ADD – Copia file dalla macchina host nell'immagine.

- WORKDIR – Imposta la directory di lavoro.

- CMD – Comando di default eseguito all’avvio del container.

- ENTRYPOINT – Simile a CMD, ma più rigido (preferito per applicazioni CLI).

- EXPOSE – Documenta le porte usate dal container (non le pubblica).

- ENV – Definisce variabili d’ambiente.

## Concetti chiave
Volume  
Permette di salvare dati persistenti anche dopo la distruzione di un container.
docker volume create myvol
docker run -v myvol:/app/data ...

Bind Mount    
Collega una directory dell'host al container:
docker run -v $(pwd):/app ...

Port Mapping  
Mappa le porte host-container:
docker run -p 8080:80 nginx

Detaching  
Avvio in background:
docker run -d nginx

## Docker Compose (multi-container)
File docker-compose.yml per definire più servizi.

Comandi principali:

docker-compose up – Avvia i servizi.

docker-compose down – Ferma e rimuove tutto.

docker-compose build – Costruisce le immagini definite.

## Gestione delle credenziali
docker login – Effettua l'accesso al Docker Hub o ad altri registries.

docker logout – Effettua il logout.