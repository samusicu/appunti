## 🚫SENZA Dependency Injection

    public class EmailService
    {
        public void SendEmail(string to, string message)
        {
            Console.WriteLine($"Email inviata a {to}: {message}");
        }
    }

    public class WelcomeController
    {
        private EmailService _emailService;

        public WelcomeController()
        {
            // L'istanza viene creata direttamente nella classe (accoppiamento forte)
            _emailService = new EmailService();
        }

        public void WelcomeUser(string email)
        {
            _emailService.SendEmail(email, "Benvenuto!");
        }
    }

    // Uso
    var controller = new WelcomeController();
    controller.WelcomeUser("utente@example.com");


🟥 Problema: WelcomeController è strettamente legato a EmailService. Se vuoi cambiare implementazione (es. invio via SMS), devi modificare il controller.

## ✅ CON Dependency Injection

    public interface IMessageService
    {
        void Send(string to, string message);
    }

    public class EmailService : IMessageService
    {
        public void Send(string to, string message)
        {
            Console.WriteLine($"Email inviata a {to}: {message}");
        }
    }

    public class WelcomeController
    {
        private readonly IMessageService _messageService;

        // Il servizio viene "iniettato" attraverso il costruttore
        public WelcomeController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void WelcomeUser(string email)
        {
            _messageService.Send(email, "Benvenuto!");
        }
    }

    // Simulazione di DI manuale
    IMessageService service = new EmailService();
    var controller = new WelcomeController(service);
    controller.WelcomeUser("utente@example.com");
    ✅ Vantaggi:

Il WelcomeController non sa nulla della classe EmailService.

Puoi facilmente sostituire EmailService con un'altra implementazione (SmsService, PushNotificationService, ecc.).


## ✅ CON Dependency Injection con Asp.NET
Nel file Program.cs (o Startup.cs nei progetti meno recenti), registri i servizi che vuoi iniettare:


    var builder = WebApplication.CreateBuilder(args);

    // Registrazione di un'interfaccia con la sua implementazione
    builder.Services.AddTransient<IMessageService, EmailService>();

    // Costruzione dell'app
    var app = builder.Build();

- AddTransient: nuova istanza ogni volta che viene richiesta

- AddScoped: una sola istanza per ogni richiesta HTTP

- AddSingleton: una sola istanza per tutta la durata dell'app

ASP.NET Core inietterà automaticamente i servizi nei controller (o altri oggetti gestiti) usando il costruttore:


    public class WelcomeController : Controller
    {
        private readonly IMessageService _messageService;

        public WelcomeController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public IActionResult Index(string email)
        {
            _messageService.Send(email, "Benvenuto!");
            return Ok();
        }
    }

Non serve iniettare manualmente IMessageService service = new EmailService(); e var controller = new WelcomeController(service); come faceva prima.