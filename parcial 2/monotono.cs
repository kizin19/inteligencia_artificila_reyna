//Algoritmo Monótono en C# (Inferencia Acumulativa)
//En este modelo, el método Inferir() deduce conocimientos basados en las reglas fijas. 
//Agregar nuevos hechos a la estructura HashSet no puede invalidar ni eliminar las 
//conclusiones obtenidas.


using System;
using System.Collections.Generic;

public class SistemaMonotono
{
    private readonly HashSet<string> _baseConocimiento = new();

    public void AgregarHecho(string hecho)
    {
        _baseConocimiento.Add(hecho);
    }

    public HashSet<string> Inferir()
    {
        var conclusiones = new HashSet<string>();

        // Regla 1: Si es perro -> Es mamífero
        if (_baseConocimiento.Contains("es_perro"))
        {
            conclusiones.Add("es_mamifero");
        }

        // Regla 2: Si es mamífero -> Es ser vivo
        if (conclusiones.Contains("es_mamifero") || _baseConocimiento.Contains("es_mamifero"))
        {
            conclusiones.Add("es_ser_vivo");
        }

        return conclusiones;
    }
}

//    Uso del programa
class Program
{
    static void Main()
    {
        var sistema = new SistemaMonotono();
        sistema.AgregarHecho("es_perro");

        Console.WriteLine("Conclusiones iniciales: " + string.Join(", ", sistema.Inferir()));
        // Salida: es_mamifero, es_ser_vivo

        // Agregamos un hecho potencialmente contradictorio
        sistema.AgregarHecho("es_de_juguete");

        // Las conclusiones deducidas previamente no cambian ni se cancelan
        Console.WriteLine("Conclusiones tras agregar nuevo hecho: " + string.Join(", ", sistema.Inferir()));
        // Salida: es_mamifero, es_ser_vivo
    }
}