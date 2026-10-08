import random

numero_ran=random.randint(0,101)

print(numero_ran)
llave=1
numero=int(input("Adivina el numero secreto entre el 0 al 100:"))
while llave==1:
    if numero==numero_ran:
        print("Felicidades, adivinaste el numero secreto")
        llave=0
    elif numero>numero_ran:
        print("El numero secreto es menor")
        numero=int(input("Adivina el numero secreto entre el 0 al 100:"))
    elif numero<numero_ran:
        print("El numero secreto es mayor")
        numero=int(input("Adivina el numero secreto entre el 0 al 100:"))
    else:
        print("Error, ingrese un numero valido")
        numero=int(input("Adivina el numero secreto entre el 0 al 100:"))
    
  
    