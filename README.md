Introducción

“*En el mundo mágico de Vindegard, donde reinaba la paz y la armonía, apareció el diablo Pepadum, un ser despreciable y vil, descrito en el grimorio de la Paz. Según contaba la leyenda, esparcía la maldad en el mundo mediante la invocación de sus siervos, eliminando todo tipo de subsistencia, intentando aumentar de esta forma su ejército, ya que si la gente moría por su mano, absorbería su alma y la manejaría a su antojo.*

*Pero la civilización de Vindegard conocía desde hace mucho tiempo esta leyenda y ha estado preparándose para su vuelta. Ha estado mejorando sus defensas y creando nuevas armas con las que hacer frente a los diferentes siervos de Pepadum, intentando que este cese sus invocaciones y conviva con la civilización de manera pacífica. [...]”.*

![](images/4.png)

Nuestra historia cuenta cómo la civilización de Vindegard se defendía de los ataques de Pepadum, el mismísimo Dios de la Muerte, mediante el uso de torres que atacaban con más o menos fuerza a los súbditos del Dios. Todos los intentos de defensa estaban controlados por el General Lolio, con una experiencia formidable en el campo de batalla. 

![](images/5.png)

Pepadum a medida que morían sus súbditos iba generando más y más, cada vez eran más fuertes, por lo que tenían que mejorar sus torres de tal forma que pudieran eliminar a todos sus enemigos y logren hacer que el Dios de la Muerte levante la bandera blanca.

Propuesta Proyecto Final

Nuestro proyecto final consiste en crear una inteligencia artificial basada en esta historia relatada, aplicándola así en un videojuego del género de Tower Defense. El proyecto va a ser un Tower Defense al estilo Bloons TD, con una generación aleatoria del mundo en cada partida que se comience, con una serie de torres aliadas que el usuario podrá colocar en unos lugares correspondientes, cada una con sus respectivas características, stats y daño, que se encargarán de defenderse de enemigos, que tendrán también sus características propias. El objetivo de estos será llegar al final del camino y el nuestro será eliminar a todos los enemigos, antes de que lleguen al final y logren ganar. Al ser tres integrantes en este proyecto, vamos a diferenciar claramente el trabajo de cada uno:

![](images/1.png)

![](images/2.png)

- David Rodríguez Gómez: Encargado de hacer una generación de mundo aleatoria. Para ello se va a disponer el escenario en casillas, y cada una será guardada en un array de dos dimensiones. Cada tipo de casilla tendrá un valor correspondiente (por ejemplo, valores de 0 para entrada y salida, 1 para camino transitable, 2 para los lugares que se coloquen las torres, y 3 camino no transitable). El mapa va a tener un tamaño fijo de casillas, pero en cada run del juego puede haber más o menos casillas transitables, con la posibilidad de poder colocar más torres o menos. Al principio habrá únicamente un camino de entrada a salida (como idea hacer más de un camino posible, para que la IA de los enemigos pueda demostrar más su capacidad). A su vez, cada casilla tendría un valor correspondiente de paso, para que la IA de los enemigos pueda elegir el camino más rápido.
- Manuel Adeliño Consuegra: se encargará de la lógica de las torres, ya que cada torre podrá tener distintos comportamientos, el primero de ellos será atacar al enemigo que esté más cercano a la salida, el segundo será atacar al enemigo más fuerte que se encuentre en su zona y el último comportamiento sería atacar al enemigo que esté más lejos de la salida. En principio habrá un tipo de torre que será polivalente:
  - Arquero: dispara un proyectil físico, es decir, no es hit scan, en una dirección que puede atravesar un número determinado de enemigos.
- Alejandro Ortega Ruiz: Encargado de la generación de enemigos, sus distintos tipos, sus distintas stats (velocidad, vida), su movimiento a través del mapa generado, e ia de enemigos (camino más corto “1 camino”, y camino con menos torres “2 o más caminos”).

Creemos que la ambición es adecuada, es decir, pensamos que el contenido propuesto es bastante alcanzable, a la vez que añadimos cosas nuevas. Además, si llevamos un buen ritmo tenemos pensado añadir las siguientes ampliaciones:

- David: Generación de más de un camino posible para que los enemigos puedan transitar.
- Manuel: Añadir distintos tipos de torre:
  - Torre bombardero: ataque lento, con daño en área de efecto del proyectil pequeña y que hace mucho daño. El proyectil será hitscan, es decir seleccionará un enemigo de los posibles y se le hará el daño directamente. Si hay enemigos al lado de ese seleccionado  y en el área de impacto se les aplicará daño pero reducido.
  - Torre de pulsos:  Ataque en área pero que afecta a todo su radio de acción. Al atacar (crear un pulso) hace daño sobre todo el radio que tiene alrededor. Daño reducido ya que puede atacar a muchos enemigos a la vez
- Alejandro: En caso de que el mapa genere más de 1 camino para alcanzar la meta, que los enemigos tengan en cuenta ambos caminos y a su vez la disposición de las torretas de manera que elijan el camino más eficiente para llegar a la meta.

¿Cuál es el punto de partida?

Nuestro proyecto utilizará assets de terceras personas (modelos 3D, sonidos, ambientes, etc.), para intentar mostrar de manera fiel la historia contada previamente.No obstante, todo lo relacionado al funcionamiento e implementación de la inteligencia artificial estará realizada por nosotros mismos, evitando usar recursos de terceras personas.

¿Cómo lo vamos a hacer?

Aunque aún no hemos concretado cómo van a ser las especificaciones, pensamos que serán de la siguiente manera:

- David Rodríguez Gómez: Para la realización de la generación autónoma y aleatoria del terreno, aplicaremos el algortimo de Ruido de Perlin, dado en el tema 1 de la asignatura Inteligencia Artificial para Videojuegos. Antes de la generación de enemigos y/o torretas, se realizarán todo tipo de pruebas relacionadas con la generación del mundo. No obstante, para no entorpecer el trabajo de mis compañeros, realizaré una escena sin generación aleatoria para que puedan realizar sus pruebas y podamos realizar nuestro trabajo a la par.
- Manuel Adeliño Consuegra: Las torres, al tener diferentes modos de ataque, tendrán distintos comportamientos. Estará comprobando en cada iteración que enemigos están dentro de su radio, y si su modo es ataque al primero o al último accederá a una variable de los enemigos que contendrá la distancia hasta la meta siguiendo el camino, sin embargo, si su modo es atacar al más fuerte, comparará a los tipos de enemigos y en caso de empate atacará al que más vida tenga. En cuanto al proyectil, al ser físico, se instanciará un prefab flecha que atravesará a un determinado número de enemigos y que desaparecerá al quedarse sin resistencia(atravesar al máximo posible) o recorrer una distancia predeterminada.
- Alejandro Ortega Ruiz: Para la generación de enemigos, tendré unos enemigos con distintas stats guardadas, de manera que según las rondas vayan avanzando estos enemigos irán siendo más complicados y en mayor cantidad, mediante el algoritmo de A\* se realizará una búsqueda del camino óptimo para que los enemigos sepan porque camino es más probable llegar al final.

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

