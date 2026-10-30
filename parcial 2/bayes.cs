using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiltroSpamBayes
{
    // Una palabra sospechosa con sus probabilidades condicionales
    public class Palabra
    {
        public string Texto { get; }
        public double ProbDadoSpam { get; }      // P(palabra | Spam)
        public double ProbDadoNoSpam { get; }    // P(palabra | No Spam)

        public Palabra(string texto, double probDadoSpam, double probDadoNoSpam)
        {
            Texto = texto;
            ProbDadoSpam = probDadoSpam;
            ProbDadoNoSpam = probDadoNoSpam;
        }
    }

    public class ClasificadorBayes
    {
        public double ProbPreviaSpam { get; } = 0.40;   // P(Spam) = 40%

        public List<Palabra> Palabras { get; } = new List<Palabra>
        {
            new Palabra("ganar",  0.70, 0.05),   
            new Palabra("dinero", 0.60, 0.10)    
        };
        public List<Palabra> DetectarPalabras(string mensaje)
        {
            string texto = mensaje.ToLowerInvariant();
            return Palabras.Where(p => texto.Contains(p.Texto)).ToList();
        }

        public double CalcularProbSpam(List<Palabra> detectadas, bool mostrarPasos)
        {
            double probSpam = ProbPreviaSpam;

            if (mostrarPasos)
                Console.WriteLine($"  P(Spam) inicial = {probSpam:P2}");

            foreach (var palabra in detectadas)
            {
                double numerador = palabra.ProbDadoSpam * probSpam;
                double denominador = numerador + palabra.ProbDadoNoSpam * (1 - probSpam);
                double posterior = numerador / denominador;

                if (mostrarPasos)
                {
                    Console.WriteLine($"  Se detectó \"{palabra.Texto}\":");
                    Console.WriteLine($"    P(Spam|{palabra.Texto}) = ({palabra.ProbDadoSpam:P0} x {probSpam:P2}) / " +
                                      $"(({palabra.ProbDadoSpam:P0} x {probSpam:P2}) + ({palabra.ProbDadoNoSpam:P0} x {1 - probSpam:P2}))");
                    Console.WriteLine($"    P(Spam|{palabra.Texto}) = {posterior:P2}");
                }

                probSpam = posterior;
            }

            return probSpam;
        }

        public string Clasificar(double probSpam)
        {
            return probSpam >= 0.50 ? "SPAM" : "NO SPAM";
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var clasificador = new ClasificadorBayes();

            Titulo("FILTRO DE SPAM CON TEOREMA DE BAYES");
            MostrarParametros(clasificador);

            Titulo("CASOS DE DEMOSTRACIÓN");
            var casos = new[]
            {
                "Hola, te veo mañana en la reunión",
                "Puedes ganar un premio hoy",
                "Te mandamos el dinero de la renta",
                "Puedes ganar dinero rápido desde casa"
            };

            foreach (var mensaje in casos)
                AnalizarMensaje(clasificador, mensaje);

            Titulo("MODO INTERACTIVO (Enter vacío para salir)");
            while (true)
            {
                Console.Write("\nEscribe un mensaje: ");
                string entrada = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(entrada)) break;
                AnalizarMensaje(clasificador, entrada);
            }

            Console.WriteLine("\nFin del programa.");
        }

        private static void AnalizarMensaje(ClasificadorBayes clasificador, string mensaje)
        {
            Console.WriteLine($"\nMensaje: \"{mensaje}\"");

            var detectadas = clasificador.DetectarPalabras(mensaje);
            if (detectadas.Count == 0)
                Console.WriteLine("  No se detectaron palabras sospechosas.");

            double prob = clasificador.CalcularProbSpam(detectadas, true);

            Console.WriteLine($"  >> Probabilidad final de spam: {prob:P2}");
            Console.WriteLine($"  >> Clasificación: {clasificador.Clasificar(prob)}");
        }

        private static void MostrarParametros(ClasificadorBayes c)
        {
            Console.WriteLine($"  P(Spam) = {c.ProbPreviaSpam:P0}   P(No Spam) = {1 - c.ProbPreviaSpam:P0}");
            foreach (var p in c.Palabras)
            {
                Console.WriteLine($"  \"{p.Texto}\": P(palabra|Spam) = {p.ProbDadoSpam:P0}, " +
                                  $"P(palabra|No Spam) = {p.ProbDadoNoSpam:P0}");
            }
            Console.WriteLine();
        }

        private static void Titulo(string texto)
        {
            Console.WriteLine(new string('=', 64));
            Console.WriteLine($"  {texto}");
            Console.WriteLine(new string('=', 64));
        }
    }
}