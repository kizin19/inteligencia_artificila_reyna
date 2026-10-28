import random


MINIMO = 0
MAXIMO = 100


def leer_numero(mensaje, minimo=MINIMO, maximo=MAXIMO):
    """Lee un entero y verifica que se encuentre dentro del rango permitido."""
    while True:
        try:
            numero = int(input(mensaje))
        except ValueError:
            print("Error: ingrese un numero entero.")
            continue

        if minimo <= numero <= maximo:
            return numero

        print(f"Error: el numero debe estar entre {minimo} y {maximo}.")


def adivinar_numero():
    numero_secreto = random.randint(MINIMO, MAXIMO)
    intentos = 0
    numero = None

    print(f"\nAdivina el numero secreto entre {MINIMO} y {MAXIMO}.")
    while numero != numero_secreto:
        numero = leer_numero("Escribe tu intento: ")
        intentos += 1

        if numero < numero_secreto:
            print("El numero secreto es mayor.")
        elif numero > numero_secreto:
            print("El numero secreto es menor.")

    print(f"Felicidades, adivinaste el numero secreto en {intentos} intento(s).")


def busqueda_heuristica():
    numero_objetivo = leer_numero(
        f"\nEscribe un numero entre {MINIMO} y {MAXIMO} para que el programa lo encuentre: "
    )
    limite_inferior = MINIMO
    limite_superior = MAXIMO
    intentos = 0

    print("\nBuscando mediante una heuristica de busqueda binaria...")
    while limite_inferior <= limite_superior:
        intento = (limite_inferior + limite_superior) // 2
        intentos += 1
        print(f"Intento {intentos}: el programa prueba con {intento}.")

        if intento == numero_objetivo:
            print(f"El programa encontro el numero {numero_objetivo}.")
            print(f"Numero de intentos: {intentos}.")
            return
        if intento < numero_objetivo:
            limite_inferior = intento + 1
        else:
            limite_superior = intento - 1


def main():
    print("=== Juego del numero secreto ===")
    print("1. Adivinar el numero secreto")
    print("2. Encontrar un numero usando heuristica")

    opcion = leer_numero("Elige una opcion (1 o 2): ", 1, 2)
    if opcion == 1:
        adivinar_numero()
    else:
        busqueda_heuristica()


if __name__ == "__main__":
    main()
