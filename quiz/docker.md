- un container software è: Un processo in esecuzione, isolato dagli altri e dal sistema operativo ospitante

- Un'immagine Docker è: 
Un insieme di layers la cui unione forma il filesystem utilizzato dal container

- ubuntu:15.6: Un'immagine con nome repository ubuntu e tag 15.6

- docker build serve a: Costruire un'immagine Docker

- Nel processo di build di un'immagine Docker, ogni istruzione del Dockerfile diventa: un layer

- docker run: avvia un nuovo container

- docker exec serve a: Eseguire un comando in un container attivo

- docker inspect serve per: Ottenere la rappresentazione JSON di una risorsa Docker

- docker ps, senza ulteriori parametri, serve per: Elencare solo i container attivi

- comando per tutti i container attivi e non: docker ps --all

- comando spegnimento di tutti i container attivi: docker stop $(docker ps -q)

- docker images
Elenca tutte le immagini Docker presenti localmente.

- docker rmi <image_id>
Rimuove una o più immagini.

- docker rm <container_id>
Rimuove uno o più container fermi.

- docker logs <container_id>
Mostra i log del container.

- docker pull <image>
Scarica un’immagine dal registry (es. Docker Hub).

- docker push <image>
Pubblica un’immagine in un registry (devi essere loggato con docker login).

- Quale dei seguenti comandi consente di accedere tramite shell ad un container già attivo?

        a.docker run -it ubuntu:latest /bin/bash (no)
        b.docker exec -it bfbd1c8d74e7 /bin/bash (SI)
        c.docker exec -it ubuntu:latest /bin/bash (no)

- L'istruzione FROM del Dockerfile serve per: Creare un'immagine che erediti tutti i layer da un'altra immagine

- Quale istruzione del Dockerfile consente di copiare un file da internet: ADD

- Nel linguaggio YAML è possibile:

        a.Utilizzare il carattere TAB per indentare il testo (no)
        b.Descrivere solo collezioni di tipi scalari (no)
        c.Descrivere collezioni di oggetti (SI)


- sintassi di una collezione di elementi di tipo stringa nel linguaggio YAML:

        cities:
        - Torino
        - Roma
        - Milano

- Il Docker Daemon è: Il processo che gestisce i container

- Con quale protocollo espone le sue funzionalità il Docker Daemon?: HTTP/REST

- Qual'è la differenza tra un volume e una bind mount: Un volume è un oggetto docker, la bind mount è una directory sul Docker Host montata dentro al container

- comando per montare un volume di nome "myvol":

        docker run \
        --rm \
        --name test \
        --mount source=myvol,target=/myvol \
        busybox:latest 

- tipi di volumi: volume(interno alla CLI di docker), bind mount, tmpfs

- ENTRYPOINT: consente di configurare il processo che verrà eseguito dentro al container. Questo processo NON potrà essere sostituito dal comando docker run tramite l'ultimo parametro.

- CMD: consente di configurare il processo che verrà eseguito dentro al container. Questo processo potrà essere sostituito dal comando docker run tramite l'ultimo parametro.

- Quali file possono essere creati/modificati all'interno di un container Docker: 
I file nuovi possono essere creati nel layer "ReadWrite" del container, anche quelli presenti nei layer "ReadOnly" delle immagini possono essere modificati grazie alla strategia "CopyOnWrite"

- comando nuova rete: docker network create mynet

- tipi di reti: bridge, host, none

- rete di tipo bridge:

        a.docker create bridge my-net (no)
        b.docker network create my-net (SI)
        c.docker create my-net (no)

- Le reti host, bridge e none hanno uno scope di tipo local

- Quale indirizzo di rete viene assegnato alla rete "bridge", creata di default da Docker durante l'installazione: 
172.17.0.0/16

- Un container con una rete di tipo host

        a.NON necessita il remapping sul Docker Host delle sue porte per poter essere raggiunto da client esterni a Docker (SI)
        b.E' meno performante rispetto ad un omologo sulla rete bridge (no)
        c.Necessita il remapping sul Docker Host delle sue porte per poter essere raggiunto da client esterni a Docker (no)

- Quante reti possono essere associate ad un container tramite il comando docker run: 1

- elementi obbligatori nel file YAML che descrive un Pod: apiVersion, kind e metadata e spec

- istruzione EXPOSE sul Dockerfile: esporre una porta TCP o UDP al di fuori del container

- Quale dei seguenti comandi rimappa la porta 80 del container sulla la porta 8080 del Docker Host?:

        a.docker run -p 80:8080 nginx:latest (no)
        b.docker run -p 8080:80 nginx:latest (SI)
        c.docker run -p 80 nginx:8080 (no)

- Docker Compose è: uno strumento per la definizione e l'esecuzione di applicazioni Docker multi-container

- Quale dei seguenti storage driver è installato di default con Docker?

        a.local (no)
        b.storagelayer (no)
        c.overlay2 (SI)


- In Kubernetes, il nome di quale risorsa viene risolto in indirizzo IP dal DNS interno?: Service

- Quale comando aumenta le repliche di un Pod se eseguito nei confronti di un Deployment:

        a.kubectl scale --replicas=10 deployment/my-deployment (SI)
        b.kubectl rollout --replicas=10 deployment/my-deployment (no)
        c.kubectl deploy --replicas=10 deployment/my-deployment (no)

- Quale dei seguenti comandi restituisce l'indicazione dei nodi sui quali sono in esecuzione i Pod di un ReplicaSet:

        a.kubectl get pod -o wide (SI)
        b.Entrambe le altre risposte (no)
        c.kubectl describe replicaset.apps/myreplicaset (no)

- Un Service di tipo NodePort apre una porta sul nodo Kubernetes:

        a.Su tutti i nodi, indipendentemente dal fatto che il Pod, a cui quel Service è stato associato, sia in esecuzione (SI)
        b.Solo se sul nodo è schedulato il Pod a cui quel Service è stato associato (no)
        c.Solo se il cluster Kubernetes è gestito da un Cloud provider pubblico (no)