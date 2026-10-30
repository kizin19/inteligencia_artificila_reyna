using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrnasBayes
{
    public readonly struct Fraccion
    {
        public long Num { get; }
        public long Den { get; }

        public Fraccion(long num, long den)
        {
            long mcd = Mcd(Math.Abs(num), Math.Abs(den));
            if (mcd == 0) mcd = 1;
            Num = num / mcd;
            Den = den / mcd;
        }

        private static long Mcd(long a, long b) => b == 0 ? a : Mcd(b, a % b);

        public static Fraccion operator *(Fraccion a, Fraccion b) => new Fraccion(a.Num * b.Num, a.Den * b.Den);
        public static Fraccion operator +(Fraccion a, Fraccion b) => new Fraccion(a.Num * b.Den + b.Num * a.Den, a.Den * b.Den);
        public static Fraccion operator /(Fraccion a, Fraccion b) => new Fraccion(a.Num * b.Den, a.Den * b.Num);

        public double Valor => (double)Num / Den;

        public override string ToString() => $"{Num}/{Den}";
    }

    public class Urna
    {
        public string Nombre { get; }
        public int Rojas { get; }
        public int Negras { get; }

        public Urna(string nombre, int rojas, int negras)
        {
            Nombre = nombre;
            Rojas = rojas;
            Negras = negras;
        }

        public int Total => Rojas + Negras;

        public Fraccion ProbRoja() => new Fraccion(Rojas, Total);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var urnas = new List<Urna>
            {
                new Urna("A", 3, 5),
                new Urna("B", 2, 1),
                new Urna("C", 2, 3)
            };

            var probUrna = new Fraccion(1, urnas.Count);

            Titulo("EJERCICIO 2: TRES URNAS Y TEOREMA DE BAYES");

            Console.WriteLine("Datos:");
            foreach (var u in urnas)
                Console.WriteLine($"  Urna {u.Nombre}: {u.Rojas} rojas y {u.Negras} negras");
            Console.WriteLine($"  Se elige una urna al azar: P(A) = P(B) = P(C) = {probUrna}");
            Console.WriteLine("  Evidencia: la bola extraída fue ROJA.\n");

            // Paso 1: P(Roja | cada urna)
            Console.WriteLine("Paso 1: probabilidad de sacar roja en cada urna");
            foreach (var u in urnas)
                Console.WriteLine($"  P(R|{u.Nombre}) = {u.Rojas}/{u.Total} = {u.ProbRoja().Valor:F4}");

            // Paso 2: probabilidad total P(R)
            Fraccion probRoja = new Fraccion(0, 1);
            foreach (var u in urnas)
                probRoja = probRoja + (probUrna * u.ProbRoja());

            Console.WriteLine("\nPaso 2: probabilidad total de sacar roja (teorema de la probabilidad total)");
            Console.WriteLine("  P(R) = P(A)·P(R|A) + P(B)·P(R|B) + P(C)·P(R|C)");
            Console.WriteLine($"  P(R) = {probRoja} = {probRoja.Valor:F4}");

            // Paso 3: Bayes para cada urna
            Console.WriteLine("\nPaso 3: teorema de Bayes");
            Console.WriteLine("  P(Urna|R) = P(R|Urna)·P(Urna) / P(R)\n");

            Fraccion suma = new Fraccion(0, 1);
            foreach (var u in urnas)
            {
                Fraccion numerador = u.ProbRoja() * probUrna;
                Fraccion posterior = numerador / probRoja;
                suma = suma + posterior;

                Console.WriteLine($"  P({u.Nombre}|R) = ({u.ProbRoja()} · {probUrna}) / {probRoja} = {posterior} = {posterior.Valor:P2}");
            }

            Console.WriteLine($"\n  Comprobación: la suma de las tres probabilidades = {suma} (debe ser 1)");

            // Respuesta a la pregunta del ejercicio
            Fraccion respuesta = (urnas[0].ProbRoja() * probUrna) / probRoja;
            Titulo("RESPUESTA");
            Console.WriteLine($"  La probabilidad de que la bola roja venga de la urna A es");
            Console.WriteLine($"  P(A|R) = {respuesta} ≈ {respuesta.Valor:F4} ≈ {respuesta.Valor:P2}");
        }

        private static void Titulo(string texto)
        {
            Console.WriteLine(new string('=', 66));
            Console.WriteLine($"  {texto}");
            Console.WriteLine(new string('=', 66));
        }
    }
}