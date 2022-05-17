using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostrarHilo : MonoBehaviour
{
    public Graph grafo;

    bool smoothPath;
    public float radioNodo = .3f;
    public Color colorCamino;
    GameObject salida;
    GameObject spawner = null;

    EnemyMovement movimientoTeseo;
    List<Vertex> camino;

    void Awake()
    {
        camino = new List<Vertex>();
    }

    private void Start()
    {
    }

    public void MostrarCamino(List<Vertex> camino, Color color)
    {
        for (int x = 0; x < camino.Count; x++)
        {
            Vertex v = camino[x];
            Renderer r = v.GetComponent<Renderer>();
            if (ReferenceEquals(r, null))
                continue;
            r.material.color = color;
        }
    }

    void Update()
    {
        //KeyDown en vez de Key a secas ya que no queremos que se est� calculando el A* todo el rato mientras se pulse espacio
        //si no que s�lo una vez al principio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (spawner == null)
            {
                spawner = GameObject.FindGameObjectWithTag("Inicio");
                if (spawner == null)
                {
                    Debug.Log("No pilla Inicio");
                }
                else
                {
                    Debug.Log("Pilla Inicio");
                }

                if (Spawner.enemies[0] != null)
                {
                    movimientoTeseo = Spawner.enemies[0].GetComponent<EnemyMovement>();
                    if (movimientoTeseo != null) Debug.Log("Movimiento cogido");
                }

                salida = GameObject.FindGameObjectWithTag("Exit");
                if (salida == null)
                {
                    Debug.Log("No pilla Salida");
                }
                else Debug.Log("Pilla Salida");
            }
            Debug.Log("Espacio");
            camino = grafo.GetPathAstar(this.spawner, this.salida, grafo.ManhattanDist);
            Debug.Log(" AH: " + camino.Count);
            if (smoothPath)
            {
                Debug.Log("Entra en smooth");
                Line();
            }
            else
            {
                //Dibujar el hilo
                //MostrarCamino(camino, colorCamino);
            }

            //Actualizamos el camino en el script de MovimientoJugador
            if (movimientoTeseo != null) 
                movimientoTeseo.ActualizarCaminoSalida(camino);
        }
    }

    //public void OnDrawGizmos()
    //{
    //    if (!Application.isPlaying)
    //        return;

    //    if (ReferenceEquals(grafo, null))
    //        return;

    //    Vertex v;
    //    if (!ReferenceEquals(spawner, null))
    //    {
    //        Gizmos.color = Color.green; // Verde es el nodo inicial
    //        v = grafo.GetNearestVertex(spawner.transform.position);
    //        Gizmos.DrawSphere(v.transform.position, radioNodo);
    //    }
    //    if (!ReferenceEquals(salida, null))
    //    {
    //        Gizmos.color = Color.red; // Rojo es el color del nodo de destino
    //        v = grafo.GetNearestVertex(salida.transform.position);
    //        Gizmos.DrawSphere(v.transform.position, radioNodo);
    //    }
    //    int i;
    //    Gizmos.color = colorCamino;
    //    for (i = 0; i < camino.Count; i++)
    //    {
    //        v = camino[i];
    //        Gizmos.DrawSphere(v.transform.position, radioNodo);
    //    }
    //}

    void Line()
    {
        LineRenderer hilo = this.GetComponent<LineRenderer>();

        if (camino.Count > 0)
        {
            hilo.positionCount = camino.Count;
            for (int i = 1; i < camino.Count; i++)
            {
                Debug.Log(i);
                hilo.SetPosition(i - 1, camino[i - 1].transform.position);
                hilo.SetPosition(i, camino[i].transform.position);
                int x = hilo.positionCount;
                Debug.Log("Posiciones: " + x);
            }
        }
    }

    void BorrarLine()
    {
        LineRenderer hilo = this.GetComponent<LineRenderer>();
        hilo.positionCount = 0;

    }
}