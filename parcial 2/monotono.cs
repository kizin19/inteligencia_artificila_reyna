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

        if (_baseConocimiento.Contains("es_perro"))
        {
            conclusiones.Add("es_mamifero");
        }

        if (conclusiones.Contains("es_mamifero") || _baseConocimiento.Contains("es_mamifero"))
        {
            conclusiones.Add("es_ser_vivo");
        }

        return conclusiones;
    }
}

class Program
{
    static void Main()
    {
        var sistema = new SistemaMonotono();
        sistema.AgregarHecho("es_perro");

        Console.WriteLine("Conclusiones iniciales: " + string.Join(", ", sistema.Inferir()));

        sistema.AgregarHecho("es_de_juguete");

        Console.WriteLine("Conclusiones tras agregar nuevo hecho: " + string.Join(", ", sistema.Inferir()));
    }
}