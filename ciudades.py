import heapq

# ---------------------------------------------------------------
# 1. DATOS DEL PROBLEMA (mapa de Rumanía, Russell & Norvig)
# ---------------------------------------------------------------
carreteras = [
    ("Arad", "Zerind", 75), ("Arad", "Sibiu", 140), ("Arad", "Timisoara", 118),
    ("Zerind", "Oradea", 71), ("Oradea", "Sibiu", 151),
    ("Timisoara", "Lugoj", 111), ("Lugoj", "Mehadia", 70),
    ("Mehadia", "Drobeta", 75), ("Drobeta", "Craiova", 120),
    ("Craiova", "Rimnicu Vilcea", 146), ("Craiova", "Pitesti", 138),
    ("Sibiu", "Fagaras", 99), ("Sibiu", "Rimnicu Vilcea", 80),
    ("Rimnicu Vilcea", "Pitesti", 97), ("Pitesti", "Bucarest", 101),
    ("Fagaras", "Bucarest", 211), ("Bucarest", "Giurgiu", 90),
    ("Bucarest", "Urziceni", 85), ("Urziceni", "Hirsova", 98),
    ("Urziceni", "Vaslui", 142), ("Hirsova", "Eforie", 86),
    ("Vaslui", "Iasi", 92), ("Iasi", "Neamt", 87),
]

# Heurística: distancia en línea recta hasta Bucarest
h = {
    "Arad": 366, "Bucarest": 0, "Craiova": 160, "Drobeta": 242, "Eforie": 161,
    "Fagaras": 176, "Giurgiu": 77, "Hirsova": 151, "Iasi": 226, "Lugoj": 244,
    "Mehadia": 241, "Neamt": 234, "Oradea": 380, "Pitesti": 100,
    "Rimnicu Vilcea": 193, "Sibiu": 253, "Timisoara": 329, "Urziceni": 80,
    "Vaslui": 199, "Zerind": 374,
}

# Grafo como diccionario de adyacencia (las carreteras van en ambos sentidos)
grafo = {}
for a, b, costo in carreteras:
    grafo.setdefault(a, []).append((b, costo))
    grafo.setdefault(b, []).append((a, costo))


# ---------------------------------------------------------------
# 2. ALGORITMO A*
# ---------------------------------------------------------------
def a_estrella(inicio, meta):
    # Cada elemento de la frontera: (f, contador, ciudad)
    # El contador evita comparar ciudades si hay empate en f.
    contador = 0
    frontera = [(h[inicio], contador, inicio)]

    g = {inicio: 0}          # mejor costo conocido desde el inicio
    padre = {inicio: None}   # para reconstruir el camino
    explorados = set()
    orden_expansion = []     # solo para mostrar qué hizo el algoritmo

    while frontera:
        f, _, actual = heapq.heappop(frontera)

        if actual in explorados:   # entrada vieja/duplicada en el heap
            continue

        explorados.add(actual)
        orden_expansion.append((actual, g[actual], h[actual], f))

        if actual == meta:
            return reconstruir_camino(padre, meta), g[meta], orden_expansion

        for vecino, costo in grafo[actual]:
            nuevo_g = g[actual] + costo
            # Si es la primera vez que vemos al vecino, o encontramos
            # un camino más barato, lo actualizamos.
            if vecino not in g or nuevo_g < g[vecino]:
                g[vecino] = nuevo_g
                padre[vecino] = actual
                contador += 1
                heapq.heappush(frontera, (nuevo_g + h[vecino], contador, vecino))

    return None, float("inf"), orden_expansion   # no hay camino


def reconstruir_camino(padre, meta):
    camino = []
    nodo = meta
    while nodo is not None:
        camino.append(nodo)
        nodo = padre[nodo]
    return camino[::-1]


# ---------------------------------------------------------------
# 3. PROGRAMA PRINCIPAL
# ---------------------------------------------------------------
if __name__ == "__main__":
    camino, costo, pasos = a_estrella("Arad", "Bucarest")

    print("Orden de expansión:")
    print(f"{'Ciudad':<16}{'g':>6}{'h':>6}{'f':>6}")
    for ciudad, g_n, h_n, f_n in pasos:
        print(f"{ciudad:<16}{g_n:>6}{h_n:>6}{f_n:>6}")

    print("\nRuta:", " -> ".join(camino))
    print("Costo total:", costo, "km")