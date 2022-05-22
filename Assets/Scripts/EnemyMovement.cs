using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // MOVIMIENTO POR ASTAR (2 O MAS CAMINOS)
    public float velocidadMax;
    Graph grafo;
    bool dead = false;
    

    private Rigidbody rb;
    private Vector3 velocidad;

    //Variables cuando se controlaado por la IA
    List<Vertex> camino;
    int indice = 0;
    private Transform casillaPisada;
    private GameObject inicio;
    private GameObject salida;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        inicio = GameObject.FindGameObjectWithTag("Inicio");
        if (inicio == null)
        {
            //Debug.Log("No pilla Inicio");
        }
        else
        {
            //Debug.Log("Pilla Inicio");
        }

        salida = GameObject.FindGameObjectWithTag("Exit");
        if (salida == null)
        {
            //Debug.Log("No pilla Salida");
        }
        //else Debug.Log("Pilla Salida");

        grafo = GameObject.FindGameObjectWithTag("Generador").GetComponent<GeneracionMapa>();
        if (grafo == null)
        {
            //Debug.Log("No pilla grafo");
        }
        //Debug.Log("Pilla grafo");

        //Debug.Log("Espacio");
        camino = grafo.GetPathAstar(this.inicio, this.salida, grafo.ManhattanDist);
        //Debug.Log(" AH: " + camino.Count);
        //if (smoothPath)
        //{
        //    Debug.Log("Entra en smooth");
        //    Line();
        //}
        //else
        //{
        //    //Dibujar el hilo
        //    //MostrarCamino(camino, colorCamino);
        //}

        //Actualizamos el camino en el script de MovimientoJugador
        ActualizarCaminoSalida(camino);
    }

    void Update()
    {
        //cuando se pulse espacio, la comprobación de la pulsación se hace en MostrarHilo
        SeguirHilo();
    }

    public void setMuerto(bool muerto)
    {
        dead = muerto;
    }
    private void FixedUpdate()
    {
        //Cuando el player no esté bajo el control de la IA, el movimiento se controla mediante físicas
        if (!rb.isKinematic)
        {
            rb.AddForce(velocidad, ForceMode.Force);
        }
    }

    //A este método se le llama cuyando se pulsa el espacio, una vez se ah calculado el camino con el A*
    public void ActualizarCaminoSalida(List<Vertex> caminoLista)
    {
        transform.forward = Vector3.forward;
        camino = caminoLista;

        if (caminoLista.Count > 0)
        {
            //la lista la recorremos de fin a principio
            indice = camino.Count - 1;
            casillaPisada = camino[indice].GetComponent<Transform>();
        }
        else
        {
            //dejar las variables a su valor por defecto
            casillaPisada = null;
            indice = 0;
        }
    }

    //Este método se llama en el update cuando se está pulsando el espacio(en ;ostrarHilo)
    private void SeguirHilo()
    {
        //!kinematic == WASD, casillaPisada==null -> no tiene camino
        if (casillaPisada == null) return;
        //Debug.Log("Movimiento");
        Vector3 casPisPos = casillaPisada.position;
        Vector3 teseoPos = transform.position;
        //y=0 para que la distancia que se suma al ser 3D no cuente en el cálculo de las distancias, es decir, la magnitud de distanacia se mide en 2D
        teseoPos.y = 0;
        casPisPos.y = 0;

        float distance = (casPisPos - teseoPos).magnitude;


        //Hay que controlar si se ha llegado a la siguiente casilla y si no estamos en el final
        if (distance < 0.2f && indice > 0)
        {
            Vertex casillaActual = camino[indice];
            ///Borrar la porción de hilo por la que acabamos de pasar
            //Renderer r = casillaActual.GetComponent<Renderer>();
            //r.material.color = Color.white;

            //cambiamos a las siguiente casilla que  hay en el camino proporcionado por el A*
            if (indice > 0)
            {
                //El error de que no actualizaba la ruta al movernos era este indice, CUIDADO!
                indice--;
                casillaActual = camino[indice];
                casillaPisada = casillaActual.GetComponent<Transform>();
            }
            else rb.isKinematic = false;

        }

        //Dirección a la que moverse
        Vector3 dir = (casillaPisada.position - transform.position);
        //Direccion en 2D
        dir.y = 0;
        //dir.Normalize();//si no normalizamos va muy rápido
        dir *= velocidadMax;

        //transform.Translate(dir * Time.deltaTime);
        GetComponent<Rigidbody>().velocity = velocidadFinal(dir);
        transform.LookAt(new Vector3(casillaPisada.position.x, transform.position.y, casillaPisada.position.z));
        //Quaternion.LookRotation(dir, Vector3.forward);
        //Debug.Log("Velocidad normal: " + transform.position);
    }

    Vector3 velocidadFinal(Vector3 dirDestino)
    {
        float magnitud = Mathf.Sqrt(Mathf.Pow(dirDestino.x, 2) + Mathf.Pow(dirDestino.y, 2) + Mathf.Pow(dirDestino.z, 2));
        float a = velocidadMax / magnitud;
        Vector3 velFinal = new Vector3(a * dirDestino.x, a * dirDestino.y, a * dirDestino.z);
        //Debug.Log("Vel final: " + velFinal);
        return velFinal;
    }

    public void llegaAlFinal()
    {
        if (Spawner.enemies.Count > 0 && !dead)
        {
            Debug.Log("Ha llegado al objetivo");
            Spawner.enemies.RemoveAt(0);
            Destroy(this.gameObject);
            Debug.Log("Enemigos restantes: " + Spawner.enemies.Count);
            dead = true;

            GameManager.Instance.QuitaVidas((int)(GetComponent<Enemigo>().tipoEnem + 1));
            GameManager.Instance.pasaEnem((int)(GetComponent<Enemigo>().tipoEnem));
        }
    }




    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Ha llegado al objetivo");
    //    Spawner.enemies.RemoveAt(0);
    //    Destroy(this.gameObject);
    //    Debug.Log("Enemigos restantes: " + Spawner.enemies.Count);
    //}


    // MOVIMIENTO POR WAYPOINTS (1 CAMINO)
    // ---------------------------------------------
    // ---------------------------------------------
    // ---------------------------------------------

    //public float speed = 10f;

    //private Transform target;
    //private int wavpointIndex = 0;

    //private void Start()
    //{
    //    target = Waypoints.points[0];
    //}

    //private void Update()
    //{
    //    Vector3 dir = target.position - transform.position;
    //    transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

    //    if (Vector3.Distance(transform.position, target.position) <= 0.2f)
    //    {
    //        GetNextWaypoint();
    //    }
    //}

    //void GetNextWaypoint()
    //{
    //    if(wavpointIndex >= Waypoints.points.Length-1)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    wavpointIndex++;
    //    target = Waypoints.points[wavpointIndex];
    //}
}
