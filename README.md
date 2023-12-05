# Proyecto de Servicio Social [TUTORIAL Pianorama]

## Introducción
El juego Pianorama busca fortalecer la habilidad de la memoria con secuencias específicas de estímulos auditivos que el jugador debe replicar.

## Dinámica de Juego
Pianorama presenta al jugador una secuencia de notas reproducida en un pequeño piano, el jugador deberá recordar la secuencia y replicarla en el mismo piano. Aunque las notas son el principal estímulo involucrado en la secuencia, el piano como recurso visual ayuda a relacionar las notas a teclas específicas.

EL juego se compone de varios niveles, los primeros cuentan con un piano que representa las primeras octavas, con un total de 12 teclas, mientras que en los niveles superiores se tienen dos octavas, con un total de 24.

## Consideraciones
EL juego maneja un sistema de nivel dinámico, en el cual, de acuerdo al desempeño del jugador se sube o baja de nivel, cambiando el reto actual dependiente del nivel actual. Este sistema incita al jugador a repetir un nivel hasta que este demuestre haberlo dominado, se considera que:

* 3 aciertos (victorias), el jugador sube de nivel.
* 3 fallos (derrotas), el jugador baja de nivel.
* Si el jugador falla tras una racha de aciertos, su contador de aciertos baja a 0.
* Si el jugador acierta tras una racha de fallos, su contador de fallos baja a 0.

Los primeros niveles cuentan además, con apoyos visuales extra para facilitar la dificultad de entrada al juego, entre estos apoyos está, por ejemplo:

* Resaltar durante el turno del jugador las teclas a presionar (no en secuencia correcta)
* Resaltar las teclas utilizadas en la secuencia después de reproducir esta.
* Diferentes colores para las teclas en la primera octava.
* Secuencia de notas mostrada (nombres).

## Estructura del juego
El juego consiste de 11 niveles, en cada uno se modifican parámetros que aumentan la dificultad en los niveles más grandes.

1. Tamaño de secuencia: 2, Re:Reproduccion: 1, una octava
2. Tamaño de secuencia: 3, Re:Reproduccion: 1, una octava
3. Tamaño de secuencia: 3, Re:Reproduccion: 1, una octava
4. Tamaño de secuencia: 4, Re:Reproduccion: 1, una octava
5. Tamaño de secuencia: 4, Re:Reproduccion: 1, una octava
6. Tamaño de secuencia: 6, Re:Reproduccion: 2, dos octavas 
7. Tamaño de secuencia: 7, Re:Reproduccion: 2, dos octavas
8. Tamaño de secuencia: 8, Re:Reproduccion: 2, dos octavas
9. Tamaño de secuencia: 9, Re:Reproduccion: 2, dos octavas
10. Tamaño de secuencia: 10, Re:Reproduccion: 2, dos octavas
11. Tamaño de secuencia: 11, Re:Reproduccion: 2, dos octavas

## Tutorial
El tutorial explica la mecánica principal del juego. Ya que el juego utiliza una simulación muy cercana a un piano real, también se cuenta como punto importante explicar la interfaz y la interacción que puede tener el jugador.

## Registro de Actividades

### 04-12-23
Se analizó la estructura general y scripts del juego, también se preservó la versión legado de los assets utilizados y se probó la ejecución completa del juego.

### 05-12-23
Se crearon algunos assets estéticos para el tutorial. Se diseñó la primera versión del flujo del tutorial y se creó una nueva escena, se distribuyeron los elementos estéticos.
