from collections import deque


def estado_valido(estado):
    """Comprueba que el lobo no se quede con la oveja ni la oveja con la col."""
    lobo, oveja, col, arriero = estado

    if lobo == oveja and arriero != lobo:
        return False
    if oveja == col and arriero != oveja:
        return False
    return True


def movimientos_posibles(estado):
    """Devuelve los estados que se pueden alcanzar en un solo viaje."""
    lobo, oveja, col, arriero = estado
    objetos = ("lobo", "oveja", "col")
    nuevos_estados = []

    # El arriero siempre cambia de orilla; puede llevar un objeto de su orilla.
    for indice, objeto in enumerate(objetos):
        valores = [lobo, oveja, col, arriero]
        if valores[indice] == arriero:
            valores[indice] = not valores[indice]
            valores[3] = not valores[3]
            nuevo_estado = tuple(valores)
            if estado_valido(nuevo_estado):
                nuevos_estados.append((nuevo_estado, objeto))

    valores = [lobo, oveja, col, not arriero]
    nuevo_estado = tuple(valores)
    if estado_valido(nuevo_estado):
        nuevos_estados.append((nuevo_estado, "solo"))

    return nuevos_estados


def resolver_arriero():
    """Busca y muestra la solución más corta del problema del arriero."""
    inicio = (False, False, False, False)
    objetivo = (True, True, True, True)
    pendientes = deque([inicio])
    anteriores = {inicio: (None, None)}

    while pendientes:
        estado = pendientes.popleft()
        if estado == objetivo:
            break

        for nuevo_estado, objeto in movimientos_posibles(estado):
            if nuevo_estado not in anteriores:
                anteriores[nuevo_estado] = (estado, objeto)
                pendientes.append(nuevo_estado)

    if objetivo not in anteriores:
        print("No existe una solución.")
        return

    solucion = []
    estado = objetivo
    while estado != inicio:
        estado_anterior, objeto = anteriores[estado]
        solucion.append((estado_anterior, objeto, estado))
        estado = estado_anterior

    print("Solución encontrada:")
    for paso, (_, objeto, estado) in enumerate(reversed(solucion), start=1):
        quien = "el arriero solo" if objeto == "solo" else f"el arriero lleva al {objeto}"
        print(f"{paso}. Cruza {quien}. Estado: {estado}")


if __name__ == "__main__":
    resolver_arriero()
            

