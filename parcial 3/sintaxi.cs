
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        var preguntas = new List<Pregunta>
        {
            new Pregunta("pastel", "¿El pastel aumentó de volumen?"),
            new Pregunta("polvo", "¿El polvo para hornear está vencido?"),
            new Pregunta("galletas", "¿Las galletas se expanden demasiado?"),
            new Pregunta("mantequilla", "¿La mantequilla estaba derretida?"),
            new Pregunta("pan", "¿La masa de pan aumenta de tamaño?"),
            new Pregunta("temperatura", "¿La temperatura ambiente es baja?"),
            new Pregunta("masa", "¿La masa de galletas está pegajosa?"),
            new Pregunta("mezcla", "¿Los ingredientes ya están mezclados?"),
            new Pregunta("crema", "¿La crema para batir permanece líquida?"),
            new Pregunta("ambiente", "¿La crema está a temperatura ambiente?")
        };

        var diagnosticos = new List<Regla>
        {
            new Regla("Regla 1",
                new List<Condicion>
                {
                    new Condicion("pastel", false),
                    new Condicion("polvo", true)
                },
                "Posible pérdida de efectividad del polvo para hornear."),

            new Regla("Regla 2",
                new List<Condicion>
                {
                    new Condicion("galletas", true),
                    new Condicion("mantequilla", true)
                },
                "Posible exceso de expansión por mantequilla derretida."),

            new Regla("Regla 3",
                new List<Condicion>
                {
                    new Condicion("pan", false),
                    new Condicion("temperatura", true)
                },
                "Posible fermentación lenta por baja temperatura.")
        };

        var recomendaciones = new List<Regla>
        {
            new Regla("Regla 4",
                new List<Condicion>
                {
                    new Condicion("masa", true),
                    new Condicion("mezcla", true)
                },
                "Refrigerar la masa durante 20 a 30 minutos antes de moldear."),

            new Regla("Regla 5",
                new List<Condicion>
                {
                    new Condicion("crema", true),
                    new Condicion("ambiente", true)
                },
                "Refrigerar la crema y los utensilios antes de volver a batir.")
        };

        var hechos = new Dictionary<string, bool>();

        Console.WriteLine("=== SISTEMA EXPERTO DE REPOSTERIA ===\n");

        foreach (var pregunta in preguntas)
        {
            hechos[pregunta.Clave] = LeerRespuesta(pregunta.Texto);
        }

        Console.WriteLine("\n=== RESULTADOS ===");

        EvaluarReglas("DIAGNOSTICOS", diagnosticos, hechos);

        EvaluarReglas("RECOMENDACIONES", recomendaciones, hechos);

        Console.WriteLine("\nProceso terminado.");
    }

    static void EvaluarReglas(
        string titulo,
        List<Regla> reglas,
        Dictionary<string, bool> hechos)
    {
        Console.WriteLine("\n--- " + titulo + " ---");

        bool encontrado = false;

        foreach (var regla in reglas)
        {
            bool cumple = regla.Condiciones.All(
                condicion =>
                    hechos[condicion.Clave] == condicion.ValorEsperado
            );

            if (cumple)
            {
                Console.WriteLine(regla.Nombre + ": " + regla.Resultado);
                encontrado = true;
            }
        }

        if (!encontrado)
        {
            Console.WriteLine("No se encontraron resultados.");
        }
    }

    // Funcion para recibir respuestas de si o no
    static bool LeerRespuesta(string pregunta)
    {
        while (true)
        {
            Console.Write(pregunta + " (s/n): ");

            string respuesta = (Console.ReadLine() ?? "")
                .Trim().ToLower();

            if (respuesta == "s" || respuesta == "si")
                return true;

            if (respuesta == "n" || respuesta == "no")
                return false;

            Console.WriteLine("Escribe solamente s o n.");
        }
    }
}

class Pregunta
{
    public string Clave { get; set; }
    public string Texto { get; set; }

    public Pregunta(string clave, string texto)
    {
        Clave = clave;
        Texto = texto;
    }
}

// CLASE PARA LAS CONDICIONES
class Condicion
{
    public string Clave { get; set; }
    public bool ValorEsperado { get; set; }

    public Condicion(string clave, bool valor)
    {
        Clave = clave;
        ValorEsperado = valor;
    }
}

class Regla
{
    public string Nombre { get; set; }
    public List<Condicion> Condiciones { get; set; }
    public string Resultado { get; set; }

    public Regla(string nombre, List<Condicion> condiciones,
                 string resultado)
    {
        Nombre = nombre;
        Condiciones = condiciones;
        Resultado = resultado;
    }
}
