using system;
using system.collections.generic;

//clasificacion de texto con multiples palabras

public class bayes 
{
    private readolny hashset<string>_textoSeparado = new();

    //ganar dinero
    public void palabras (string input)
    {
        string[] palabras = input.split(' ');

        foreach (string palabra in palabras)
        {
            _textoSeparado.add(palabra);
        }

        console.writeline("palabras separadas: " + string.join(", ", _textoSeparado));
    }

    public void clasificacion()
    {
        //spam o no spam ganar dinero
        double scoreSpam = 0;

        if (_textoSeparado.contains("ganar") && _textoSeparado.contains("dinero"))
        {
            scoreSpam += 97.6; // Alta probabilidad de spam
        }else if(_textoSeparado.contains("dinero"))
        {
            scoreSpam += 84; // Probabilidad moderada de spam
        }else if(_textoSeparado.contains("ganar"))
        {
            scoreSpam += 77; // Baja probabilidad de spam   
        }
    }
}

class program
{
    static void main()
    {
        var bayes = new bayes();

        console.writeline("ingrese palabras separadas por espacio: ");
        string input = console.readline();

        bayes.palabras(input);
    }
}