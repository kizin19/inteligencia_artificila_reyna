using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiagnosticoRed
{
    // =====================================================================
    // PARTE 1: RAZONAMIENTO MONÓTONO
    // Las conclusiones solo se acumulan; nunca se eliminan.
    // =====================================================================
    public class SistemaMonotono
    {
        private readonly HashSet<string> _hechos = new HashSet<string>();
        private readonly HashSet<string> _conclusiones = new HashSet<string>();

        public void AgregarHecho(string hecho)
        {
            if (_hechos.Add(hecho))
                Console.WriteLine($"  [+] Hecho agregado: \"{hecho}\"");
        }

        // Encadenamiento hacia adelante hasta alcanzar un punto fijo.
        // Solo se ejecutan operaciones Add sobre las conclusiones: jamás Remove.
        public void Inferir()
        {
            bool huboCambios;
            do
            {
                huboCambios = false;

                // Regla 1: latencia_alta -> posible_falla
                if (_hechos.Contains("latencia_alta") && _conclusiones.Add("posible_falla"))
                    huboCambios = true;

                // Regla 2: posible_falla O servidor_caido -> notificar_admin
                bool condicion = _conclusiones.Contains("posible_falla") || _hechos.Contains("servidor_caido");
                if (condicion && _conclusiones.Add("notificar_admin"))
                    huboCambios = true;

            } while (huboCambios);
        }

        public void MostrarEstado()
        {
            Console.WriteLine($"  Hechos       : {{ {string.Join(", ", _hechos)} }}");
            Console.WriteLine($"  Conclusiones : {{ {string.Join(", ", _conclusiones)} }}");
        }

        public bool Concluye(string conclusion) => _conclusiones.Contains(conclusion);
    }

    // =====================================================================
    // PARTE 2: RAZONAMIENTO NO MONÓTONO
    // Una conclusión por defecto puede ser retractada ante una excepción.
    // =====================================================================
    public class SistemaNoMonotono
    {
        private readonly HashSet<string> _hechos = new HashSet<string>();
        private string _conclusionActual = "(sin conclusión)";

        public void AgregarHecho(string hecho)
        {
            if (_hechos.Add(hecho))
                Console.WriteLine($"  [+] Hecho agregado: \"{hecho}\"");
        }

        public string Evaluar()
        {
            string nuevaConclusion;

            if (!_hechos.Contains("servidor_encendido"))
            {
                nuevaConclusion = "(sin conclusión)";
            }
            else
            {
                // Excepciones (defeaters) de la regla por defecto
                bool hayExcepcion = _hechos.Contains("mantenimiento_programado")
                                 || _hechos.Contains("disco_lleno");

                nuevaConclusion = hayExcepcion
                    ? "servidor_NO_operativo (excepción detectada)"
                    : "servidor_OPERATIVO";
            }

            // Si cambia la conclusión previa, se muestra la retracción
            if (nuevaConclusion != _conclusionActual && _conclusionActual == "servidor_OPERATIVO")
            {
                Console.WriteLine($"  [!] RETRACTACIÓN: se retira \"{_conclusionActual}\"");
            }

            _conclusionActual = nuevaConclusion;
            return _conclusionActual;
        }

        public void MostrarEstado()
        {
            Console.WriteLine($"  Hechos       : {{ {string.Join(", ", _hechos)} }}");
            Console.WriteLine($"  Conclusión   : {_conclusionActual}");
        }
    }

    // =====================================================================
    // PARTE 3: PROGRAMA PRINCIPAL
    // =====================================================================
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Titulo("DIAGNÓSTICO DE SERVIDORES: MONÓTONO vs NO MONÓTONO");

            EjecutarModuloMonotono();
            Console.WriteLine();
            EjecutarModuloNoMonotono();
            Console.WriteLine();
            MostrarComparacion();
        }

        private static void EjecutarModuloMonotono()
        {
            Titulo("PARTE 1: SISTEMA MONÓTONO");
            var sistema = new SistemaMonotono();

            Paso("Paso 1: se agrega \"latencia_alta\" y se infiere");
            sistema.AgregarHecho("latencia_alta");
            sistema.Inferir();
            sistema.MostrarEstado();

            Console.WriteLine();
            Paso("Paso 2: se agrega \"conexion_restablecida\" y se infiere de nuevo");
            sistema.AgregarHecho("conexion_restablecida");
            sistema.Inferir();
            sistema.MostrarEstado();

            Console.WriteLine();
            bool intactas = sistema.Concluye("posible_falla") && sistema.Concluye("notificar_admin");
            Console.WriteLine(intactas
                ? "  >> RESULTADO: \"posible_falla\" y \"notificar_admin\" SE MANTIENEN INTACTAS.\n" +
                  "     El sistema monótono no puede eliminar verdades ya deducidas."
                : "  >> ERROR: las conclusiones cambiaron (no debería ocurrir).");
        }

        private static void EjecutarModuloNoMonotono()
        {
            Titulo("PARTE 2: SISTEMA NO MONÓTONO");
            var sistema = new SistemaNoMonotono();

            Paso("Paso 1: se agrega solo \"servidor_encendido\" y se evalúa");
            sistema.AgregarHecho("servidor_encendido");
            string resultado1 = sistema.Evaluar();
            Console.WriteLine($"  Conclusión: {resultado1}");
            sistema.MostrarEstado();

            Console.WriteLine();
            Paso("Paso 2: se agrega \"mantenimiento_programado\" y se reevalúa");
            sistema.AgregarHecho("mantenimiento_programado");
            string resultado2 = sistema.Evaluar();
            Console.WriteLine($"  Conclusión: {resultado2}");
            sistema.MostrarEstado();

            Console.WriteLine();
            Console.WriteLine($"  >> RESULTADO: la conclusión pasó de \"{resultado1}\"\n" +
                              $"     a \"{resultado2}\".\n" +
                              "     El sistema no monótono SÍ puede retractar conclusiones ante nueva evidencia.");
        }

        private static void MostrarComparacion()
        {
            Titulo("COMPARACIÓN FINAL");
            Console.WriteLine("  Monótono    : al agregar evidencia, las conclusiones previas NUNCA se pierden.");
            Console.WriteLine("  No monótono : al agregar evidencia, las conclusiones previas PUEDEN retractarse.");
        }

        private static void Titulo(string texto)
        {
            string linea = new string('=', 62);
            Console.WriteLine(linea);
            Console.WriteLine($"  {texto}");
            Console.WriteLine(linea);
        }

        private static void Paso(string texto)
        {
            Console.WriteLine($"--- {texto} ---");
        }
    }
}