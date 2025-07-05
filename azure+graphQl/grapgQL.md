## Queries
Le query sono operazioni GraphQL che leggono dati da un server. I concetti chiave includono:
- Fields (Campi): GraphQL è essenzialmente richiedere campi specifici su oggetti. Puoi navigare oggetti correlati e i loro campi, permettendo ai client di recuperare molti dati correlati in una singola richiesta.
- Arguments (Argomenti): Ogni campo e oggetto nidificato può avere i propri argomenti, rendendo GraphQL un sostituto completo per fare più richieste API.

        query GetUser {
            # user prende come argomento id 
            user(id: "123") {
                # id, name, email sono campi di user
                id
                name
                email
                createdAt
            }
        }
- Variables (Variabili): Permettono di passare valori dinamici alle query senza manipolare direttamente la stringa di query. Le variabili sono precedute dal carattere $ e possono essere usate per fornire valori dinamici agli argomenti dei campi.

        query GetUserPosts($userId: ID!) {
            user(id: $userId) {
                name
                email
            }
        }

        # variabili
        {
        "userId": "123"
        }
- Fragments: I fragment sono set di campi riutilizzabili che possono essere usati secondo necessità in più query. Utili per evitare ripetizioni in query complesse.

        fragment UserInfo on User {
            id
            name
            email
        }

        query GetUsersWithFragment {
            currentUser {
                ...UserInfo
                role
            }
            suggestedUsers(limit: 3) {
                ...UserInfo
                mutualFriends
            }
        }
- Directives: Le direttive eseguibili possono essere applicate alle query per cambiare il risultato di una query GraphQL quando viene eseguita sul server. Include @include e @skip per controllo condizionale.

        query GetUserProfile($isAdmin: Boolean!) {
            user(id: "123") {
                id
                name
                email
                # Salta questi campi se NON è admin
                personalData @skip(if: $isAdmin) {
                    phoneNumber
                    address
                    dateOfBirth
                }
                # Include questi campi solo se è admin
                adminData @include(if: $isAdmin) {
                    lastLogin
                    ipAddress
                    permissions
                }
            }
        }

Note Pratiche:
- Richiedi solo i campi di cui hai bisogno
- Usa fragment per evitare duplicazioni
- Sfrutta gli alias per rinominare campi o fare più richieste dello stesso tipo
- Usa variabili per query dinamiche

## Mutations 
Le mutazioni sono usate per modificare dati lato server. Caratteristiche principali:
- Creazione dati: I campi di mutazione possono accettare argomenti e spesso utilizzano Input Object types per passare oggetti strutturati.

        mutation CreatePost {
            createPost(input: {
                title: "Il mio nuovo post"
                content: "Contenuto del post..."
                categoryId: "tech"
                tags: ["javascript", "graphql"]
            }) {
                    id
                    title
                    content
                    createdAt
            }
        }

- Aggiornamento dati: GraphQL favorisce mutazioni specifiche per ogni scopo (come updateHumanName) invece di endpoint generici come in REST.

        mutation UpdateUserProfile($userId: ID!, $input: UpdateUserInput!) {
            updateUser(id: $userId, input: $input) {
                id
                name
                email
                bio
                updatedAt
            }
        }

        {
            "userId": "123",
            "input": {
                "name": "Mario Rossi",
                "bio": "Sviluppatore Full Stack",
                "avatar": "https://example.com/avatar.jpg"
            }
        }
- Eliminazione dati: Possiamo usare mutazioni per eliminare dati esistenti definendo un altro campo sul tipo Mutation.

        mutation DeletePost($postId: ID!) {
            deletePost(id: $postId) {
                success
                message
                deletedPostId
            }
        }
- Esecuzione seriale: Mentre i campi query vengono eseguiti in parallelo, i campi mutation vengono eseguiti in serie. Questo garantisce che non si verifichino race condition quando si inviano più mutazioni.

        mutation MultipleOperations {
            likePost(postId: "789") {
                id
                likesCount
            }
            addComment(postId: "789", text: "Ottimo articolo!") {
                id
                text
                createdAt
            }
            followUser(userId: "456") {
                id
                isFollowing
                followersCount
            }
        }

Note Pratiche:
- Progetta mutazioni specifiche per ogni caso d'uso
- Restituisci dati utili per aggiornare la UI
- Ricorda che le mutazioni vengono eseguite in serie
- Usa Input Types per argomenti complessi
## Subscriptions 
Le subscription permettono di ricevere aggiornamenti in tempo reale tramite richieste di lunga durata. Aspetti importanti:
- Definizione: I campi subscription sono definiti esattamente come i campi Query e Mutation, ma usando il tipo di operazione root subscription.
- Implementazione: Le subscription GraphQL sono tipicamente supportate da un sistema pub/sub separato e spesso implementate con WebSockets o server-sent events.
- Limitazioni: Ogni operazione deve avere esattamente un campo root, a differenza di query e mutazioni che possono avere più campi root.
- Scalabilità: Le operazioni subscription sono più complicate da implementare rispetto a query o mutazioni perché devi mantenere il documento GraphQL, le variabili e altro contesto per tutta la durata della subscription.
- Casi d'uso ideali: Le operazioni subscription sono adatte per dati che cambiano spesso e incrementalmente, e per client che devono ricevere questi aggiornamenti incrementali il più vicino possibile al tempo reale.

Esempi:

    # esempio base: nuovi messaggi in una chat
    graphqlsubscription NewMessages {
        messageAdded(channelId: "general") {
            id
            text
            createdAt
            user {
            id
            name
            avatar
            }
        }
    }

    # evento ricevuto:
    json{
        "data": {
            "messageAdded": {
                "id": "msg_001",
                "text": "Ciao a tutti!",
                "createdAt": "2024-06-13T16:45:00Z",
                "user": {
                    "id": "123",
                    "name": "Mario Rossi",
                    "avatar": "https://example.com/mario.jpg"
                }
            }
        }
    }


    # esempio con variabili,notifiche utente
    graphqlsubscription UserNotifications($userId: ID!) {
        notificationAdded(userId: $userId) {
            id
            type
            title
            message
            read
            createdAt
            relatedEntity {
                ... on Post {
                    id
                    title
                }
                ... on User {
                    id
                    name
                }
            }
        }
    }
    # variabili:
    json{
    "userId": "123"
    }
    
    # aggiornamenti stato ordine
    graphqlsubscription OrderStatusUpdates($orderId: ID!) {
        orderStatusChanged(orderId: $orderId) {
            id
            status
            statusHistory {
            status
            timestamp
            note
            }
            estimatedDelivery
        }
    }

    # presenza utenti online
    graphqlsubscription OnlineUsers {
        userPresenceChanged {
            user {
            id
            name
            avatar
            }
            status
            lastSeen
            isTyping
        }
    }

    # esempio avanzato: aggiornamenti live di un documento collaborativo
    graphqlsubscription DocumentChanges($documentId: ID!) {
        documentUpdated(documentId: $documentId) {
            id
            version
            changes {
                type
                position
                content
                author {
                    id
                    name
                }
                timestamp
                }
            activeCollaborators {
                id
                name
                cursor {
                    line
                    column
                }
            }
        }
    }

Note Pratiche:
- Ogni subscription può avere un solo campo root
- Considera la scalabilità e l'architettura necessaria
- Usa solo per dati che cambiano frequentemente
- Implementa logica di riconnessione nel client