
Propuesta Proyecto Final

Nuestro proyecto final consiste en crear una inteligencia artificial basada en los videojuegos del género de Tower Defense. El proyecto va a ser un Tower Defense al estilo Bloons TD, con una generación aleatoria del mundo en cada partida que se comience, con una serie de torres aliadas que el usuario podrá colocar en unos lugares correspondientes, cada una con sus respectivas características, stats y daño, que se encargarán de defenderse de enemigos, que tendrán también sus características propias. El objetivo de estos será llegar al final del camino y el nuestro será eliminar a todos los enemigos, antes de que lleguen al final y logren ganar. Al ser tres integrantes en este proyecto, vamos a diferenciar claramente el trabajo de cada uno:

![](Aspose.Words.cfa8f7d0-1d85-4db2-9dea-500dcd16ec8a.001.png)

![](Aspose.Words.cfa8f7d0-1d85-4db2-9dea-500dcd16ec8a.002.png)

- David Rodríguez Gómez: Encargado de hacer una generación de mundo aleatoria. Para ello se va a disponer el escenario en casillas, y cada una será guardada en un array de dos dimensiones. Cada tipo de casilla tendrá un valor correspondiente (por ejemplo, valores de 0 para entrada y salida, 1 para camino transitable, 2 para los lugares que se coloquen las torres, y 3 camino no transitable). El mapa va a tener un tamaño fijo de casillas, pero en cada run del juego puede haber más o menos casillas transitables, con la posibilidad de poder colocar más torres o menos. Al principio habrá únicamente un camino de entrada a salida (como idea hacer más de un camino posible, para que la IA de los enemigos pueda demostrar más su capacidad). A su vez, cada casilla tendría un valor correspondiente de paso, para que la IA de los enemigos pueda elegir el camino más rápido.
- Manuel Adeliño Consuegra: se encargará de la lógica de las torres, ya que cada torre podrá tener distintos comportamientos, el primero de ellos será atacar al enemigo que esté más cercano a la salida, el segundo será atacar al enemigo más fuerte que se encuentre en su zona y el último comportamiento sería atacar al enemigo que esté más lejos de la salida. En principio habrá un tipo de torre que será polivalente:
  - Arquero: dispara un proyectil físico, es decir, no es hit scan, en una dirección que puede atravesar un número determinado de enemigos.
- Alejandro Ortega Ruiz: Encargado de la generación de enemigos, sus distintos tipos, sus distintas stats (velocidad, vida), su movimiento a través del mapa generado, e ia de enemigos (camino más corto “1 camino”, y camino con menos torres “2 o más caminos”).

Creemos que la ambición es adecuada, es decir, pensamos que el contenido propuesto es bastante alcanzable, a la vez que añadimos cosas nuevas. Además, si llevamos un buen ritmo tenemos pensado añadir las siguientes ampliaciones:

- David: Generación de más de un camino posible para que los enemigos puedan transitar.
- Manuel: Añadir distintos tipos de torre:
  - Torre bombardero: ataque lento, con daño en área de efecto del proyectil pequeña y que hace mucho daño. El proyectil será hitscan, es decir seleccionará un enemigo de los posibles y se le hará el daño directamente. Si hay enemigos al lado de ese seleccionado  y en el área de impacto se les aplicará daño pero reducido.
  - Torre de pulsos:  Ataque en área pero que afecta a todo su radio de acción. Al atacar (crear un pulso) hacer daño sobre todo el radio uqe tiene alrededor. Daño reducido ya uqe puede atacar a muchos enemigos a la vez
- Alejandro: En caso de que el mapa genere más de 1 camino para alcanzar la meta, que los enemigos tengan en cuenta ambos caminos y a su vez la disposición de las torretas de manera que elijan el camino más eficiente para llegar a la meta.



Bibliografía y/o referentes

- *Diapositivas del Curso 2021-2022 de la asignatura de Inteligencia Artificial del tercer curso del grado de Desarrollo de Videojuegos impartida por Federico Peinado Gil.*
- *AI for Games, Third Edition Ian Millington.*
- *Unity 2018 Artificial Intelligence Cookbook, Second Edition (**Repositorio**)* <https://github.com/PacktPublishing/Unity-2018-Artificial-Intelligence-Cookbook-Second-Edition>
- *Unity Artificial Intelligence Programming, Fourth Edition (**Repositorio**) <https://github.com/PacktPublishing/Unity-Artificial-Intelligence-Programming-Fourth-Edition>*
- *Opsive, Behavior Designer <https://opsive.com/assets/behavior-designer/>*
- *Unity, Bolt Visual Scripting <https://docs.unity3d.com/bolt/1.4/manual/index.html>*
- *Unity, Navegación y Búsqueda de caminos <https://docs.unity3d.com/es/2019.3/Manual/Navigation.html>*
- *Plants vs. Zombies [Compra Plants vs. Zombies – PC – EA*](https://www.ea.com/es-es/games/plants-vs-zombies/plants-vs-zombies)*
- *Bloons Tower Defense [Bloons Tower Defense - Wikipedia, la enciclopedia libre*](https://es.wikipedia.org/wiki/Bloons_Tower_Defense)*

