// Algoritmo No Monótono en C# (Lógica por Defecto / Retracción)
// Este modelo evalúa si existen hechos que actúen como "excepciones" (defeaters). 
// Si se agrega un nuevo hecho contradictorio, la deducción por defecto se reevalúa 
// y se retracta.

using System;
using System.Collections.Generic;

public class SistemaNoMonotono
{
    private readonly HashSet<string> _hechos = new();

    public void AgregarHecho(string hecho)
    {
        _hechos.Add(hecho);
    }

    public string InferirSiVuela(string entidad)
    {
        // Condición inicial: Si es ave, por defecto asumimos que vuela
        if (_hechos.Contains($"{entidad}_es_ave"))
        {
            // Verificación de excepciones (Mecanismo No Monótono)
            if (_hechos.Contains($"{entidad}_es_pinguino") || _hechos.Contains($"{entidad}_tiene_ala_rota"))
            {
                return $"{entidad} NO vuela (razón: excepción detectada).";
            }

            return $"{entidad} vuela (por defecto).";
        }

        return $"No hay suficiente información sobre {entidad}.";
    }
}

//     Uso del programa
class Program
{
    static void Main()
    {
        var agente = new SistemaNoMonotono();

        // Paso 1: Solo sabemos que Piolín es un ave
        agente.AgregarHecho("piolin_es_ave");
        Console.WriteLine(agente.InferirSiVuela("piolin"));
        // Salida: piolin vuela (por defecto).

        // Paso 2: Descubrimos información adicional (Piolín es un pingüino)
        agente.AgregarHecho("piolin_es_pinguino");

        // La conclusión previa SE RETRACTA
        Console.WriteLine(agente.InferirSiVuela("piolin"));
        // Salida: piolin NO vuela (razón: excepción detectada).
    }
}