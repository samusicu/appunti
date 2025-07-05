# SOLID Principles
## Single Responsibility Principle (SRP)
Una classe dovrebbe avere una sola ragione di cambiare. Significa che ogni classe dovrebbe fare una sola cosa, e farla bene.

❌ Violazione:

    class Report {
        public string Title { get; set; }
        public string Content { get; set; }

        public void Print() {
            Console.WriteLine(Title);
            Console.WriteLine(Content);
        }

        public void SaveToFile(string path) {
            File.WriteAllText(path, Title + "\n" + Content);
        }
    }

✅ Rispetto di SRP:

    class Report {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    class ReportPrinter {
        public void Print(Report report) {
            Console.WriteLine(report.Title);
            Console.WriteLine(report.Content);
        }
    }

    class ReportSaver {
        public void SaveToFile(Report report, string path) {
            File.WriteAllText(path, report.Title + "\n" + report.Content);
        }
    }


## Open/Closed Principle (OCP)
Le entità software dovrebbero essere aperte all’estensione, ma chiuse alla modifica. Puoi aggiungere nuove funzionalità estendendo il codice, non cambiandolo.

❌ Violazione:

    class DiscountCalculator {
        public double CalculateDiscount(string customerType) {
            if (customerType == "Regular")
                return 0.1;
            else if (customerType == "VIP")
                return 0.2;
            return 0;
        }
    }

✅ Rispetto di OCP:

    interface IDiscountStrategy {
        double GetDiscount();
    }

    class RegularCustomerDiscount : IDiscountStrategy {
        public double GetDiscount() => 0.1;
    }

    class VIPCustomerDiscount : IDiscountStrategy {
        public double GetDiscount() => 0.2;
    }

    class DiscountCalculator {
        public double CalculateDiscount(IDiscountStrategy strategy) {
            return strategy.GetDiscount();
        }
    }


## Liskov Substitution Principle (LSP)
Le classi derivate dovrebbero poter essere sostituite alle loro classi base senza rompere il comportamento del programma.

❌ Violazione:

    class Bird {
        public virtual void Fly() {
            Console.WriteLine("Flying");
        }
    }

    class Ostrich : Bird {
        public override void Fly() {
            throw new NotImplementedException(); // Le struzzi non volano!
        }
    }

✅ Rispetto di LSP:

    abstract class Bird {}

    interface IFlyingBird {
        void Fly();
    }

    class Sparrow : Bird, IFlyingBird {
        public void Fly() {
            Console.WriteLine("Flying");
        }
    }

    class Ostrich : Bird {
        // Non implementa IFlyingBird
    }


## Interface Segregation Principle (ISP)
È meglio avere molte interfacce specifiche piuttosto che una generale e pesante. Le classi non dovrebbero essere obbligate a implementare metodi che non usano.

❌ Violazione:

    interface IMultiFunctionDevice {
        void Print();
        void Scan();
        void Fax();
    }

    class OldPrinter : IMultiFunctionDevice {
        public void Print() { /* OK */ }
        public void Scan() { throw new NotImplementedException(); }
        public void Fax() { throw new NotImplementedException(); }
    }

✅ Rispetto di ISP:

    interface IPrinter {
        void Print();
    }

    interface IScanner {
        void Scan();
    }

    class SimplePrinter : IPrinter {
        public void Print() { /* stampa */ }
    }

## Dependency Inversion Principle (DIP)
Le classi dovrebbero dipendere da astrazioni, non da classi concrete. I dettagli devono dipendere dalle astrazioni, non viceversa.

❌ Violazione:

    class FileLogger {
        public void Log(string message) {
            File.WriteAllText("log.txt", message);
        }
    }

    class UserService {
        private FileLogger logger = new FileLogger();

        public void Register() {
            logger.Log("User registered");
        }
    }

✅ Rispetto di DIP:

    interface ILogger {
        void Log(string message);
    }

    class FileLogger : ILogger {
        public void Log(string message) {
            File.WriteAllText("log.txt", message);
        }
    }

    class UserService {
        private readonly ILogger logger;

        public UserService(ILogger logger) {
            this.logger = logger;
        }

        public void Register() {
            logger.Log("User registered");
        }
    }
