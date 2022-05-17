/*    
    Obra original:
        Copyright (c) 2018 Packt
        Unity 2018 Artificial Intelligence Cookbook - Second Edition, by Jorge Palacios
        https://github.com/PacktPublishing/Unity-2018-Artificial-Intelligence-Cookbook-Second-Edition
        MIT License

    Modificaciones:
        Copyright (C) 2020-2022 Federico Peinado
        http://www.federicopeinado.com

        Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
        Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
        Contacto: email@federicopeinado.com
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/// <summary>
/// Abstract class for graphs
/// </summary>
public abstract class Graph : MonoBehaviour
{
    protected Vertex[] vertices;
    protected List<List<Vertex>> neighbors;
    protected List<List<float>> costs;

    //protected Dictionary<int, int> instIdToId;

    //// this is for informed search like A*
    public delegate float Heuristic(Vertex a, Vertex b);

    // Used for getting path in frames
    public List<Vertex> path;
    int edgesVisitados;
    float timeActual;
    int longitud;
    //public bool isFinished;

    public virtual void Start()
    {
        Load();
        Debug.Log("1");
        edgesVisitados = 0;
        timeActual = 0.0f;
        longitud = 0;
    }

    public virtual void Load() { }

    public virtual int GetSize()
    {
        if (ReferenceEquals(vertices, null))
            return 0;
        return vertices.Length;
    }

    public virtual Vertex GetNearestVertex(Vector3 position)
    {
        return null;
    }


    public virtual Vertex[] GetNeighbours(Vertex v)
    {
        if (ReferenceEquals(neighbors, null) || neighbors.Count == 0)
            return new Vertex[0];
        if (v.id < 0 || v.id >= neighbors.Count)
            return new Vertex[0];
        return neighbors[v.id].ToArray();
    }

    private Edge[] GetNeighborEdge(Vertex v)
    {
        //evaluación perezosa
        if (ReferenceEquals(neighbors, null) || v.id < 0 || v.id >= neighbors.Count)
            return new Edge[0];

        int nEdges = neighbors[v.id].Count;//el número de vecinos que tiene ese vértice
        Edge[] edges = new Edge[nEdges];
        List<Vertex> nodosVecino = neighbors[v.id];
        List<float> costes = costs[v.id];

        for (int x = 0; x < nEdges; x++)
        {
            edges[x] = new Edge();
            edges[x].vertex = nodosVecino[x];
            edges[x].cost = costes[x];
        }

        Debug.Log("EDGES : " + edges.Length);
        Debug.Log("EDGES CONCTADO: " + edges[0].vertex.id);
        return edges;
    }

    //srcO = incio
    //dstO = Exit
    public List<Vertex> GetPathAstar(GameObject srcO, GameObject dstO, Heuristic h = null)
    {
        //Reseteo
        timeActual = Time.realtimeSinceStartup;
        edgesVisitados = 0;
        longitud = 0;

        Debug.Log("ASTAR");

        //Código defensivo
        if (!srcO || !dstO)
            return new List<Vertex>();

        //Guardar posiciones de los game objects en vertices
        Debug.Log("Pos Inicio x: " + srcO.transform.position.x + "  y: " + srcO.transform.position.z);
        Debug.Log("Pos Exit x: " + dstO.transform.position.x + "  y: " + dstO.transform.position.z);
        Vertex src = GetNearestVertex(srcO.transform.position);
        Vertex dst = GetNearestVertex(dstO.transform.position);
        Debug.Log("DST.ID : " + dst.id);
        Debug.Log("SRC.ID : " + src.id);
        //AQUI SE COMPROBARIAN LAS TORRETAS EN UNA LISTA STATIC
        //Vector3 posMinotauro = minotauro.transform.position;
        //posMinotauro.y = 0;
        //Vertex verticeMinotauro = GetNearestVertex(posMinotauro);
        //Edge[] aristaMinotauro = GetNeighborEdge(verticeMinotauro);
        //List<Vertex> verticesMinotauro = new List<Vertex>();
        //verticesMinotauro.Add(verticeMinotauro);

        //foreach (Edge e in aristaMinotauro)
        //{
        //    verticesMinotauro.Add(e.vertex);
        //}

        //Para llevar siempre el camino más prometedor delante
        BinaryHeap<Edge> priorityQueueAristas = new BinaryHeap<Edge>();

        Edge vertice, hijo;
        Edge[] aristas;

        float[] distancias = new float[vertices.Length];
        int[] verticeAnterior = new int[vertices.Length];
        Debug.Log("VERTICES: " + vertices.Length);

        vertice = new Edge(src, 0);
        priorityQueueAristas.Add(vertice);
        distancias[src.id] = 0;
        verticeAnterior[src.id] = src.id;

        //Inicialización del resto de vértices, con valores por defecto
        //para que, posteriormente, se actualicen
        for (int i = 0; i < vertices.Length; i++)
        {
            if (i != src.id)
            {
                verticeAnterior[i] = -1;
                distancias[i] = Mathf.Infinity;
            }
        }

        Debug.Log("COUNT : " + priorityQueueAristas.Count);
        //Bucle ppal
        while (priorityQueueAristas.Count > 0)
        {
            edgesVisitados++;

            //Obtenemos el nodo más prioritario
            vertice = priorityQueueAristas.Remove();
            int nodeId = vertice.vertex.id;
            Debug.Log("VERTICE.VERTEX.ID : " + vertice.vertex.id);

            //Si es el que se busca, devolvemos el camino construido
            if (ReferenceEquals(vertice.vertex, dst))
            {
                timeActual = Time.realtimeSinceStartup - timeActual;
                List<Vertex> lista = BuildPath(src.id, vertice.vertex.id, ref verticeAnterior);
                longitud = lista.Count;
                Debug.Log("longitud " + longitud);
                return lista;
                Debug.Log("longitudFUERA");
            }

            //Se guardan los nodos vecinos del nodo actual y se recorren
            aristas = GetNeighborEdge(vertice.vertex);
            foreach (Edge neigh in aristas)
            {
                Debug.Log("Entra foreach");
                int nID = neigh.vertex.id;
                Debug.Log("NID: " + nID);
                Debug.Log(verticeAnterior[nID]);
                //Si es != -1 es que ha sido visitado, por tanto se le salta
                //Si es la casilla del minotauro, tampoco se cuenta
                if (verticeAnterior[nID] == -1)
                {
                    Debug.Log("Posicion vertice : " + vertice.vertex.transform);
                    Debug.Log("Posicion neigh : " + neigh.vertex.id);
                    Debug.Log("Visita ID: " + nID);
                    //AQUI SE CALCULA EL COSTE DE LAS POSICIONES DE LAS TORRETAS
                    //if (vertice.vertex == verticeMinotauro)
                    //{
                    //    continue;
                    //}
                    ////Se calcula el coste del nodo
                    //float cost = distancias[nodeId] + neigh.cost;


                    //foreach (Edge mino in aristaMinotauro)
                    //{
                    //    if (neigh.vertex == mino.vertex)
                    //        cost *= 5;
                    //}

                    //Se le suma el coste estimado al coste del nodo
                    float cost = distancias[nodeId] + neigh.cost;
                    cost += h(vertice.vertex, neigh.vertex);

                    Debug.Log("COSTE: " + cost);
                    Debug.Log("DISTANCIAS: " + distancias[neigh.vertex.id]);
                    //Si el coste es menor que el que ya estaba almacenado,
                    //entonces es mejor solución
                    if (cost < distancias[neigh.vertex.id])
                    {
                        Debug.Log("ABC");
                        distancias[nID] = cost;
                        verticeAnterior[nID] = nodeId;
                        priorityQueueAristas.Remove(neigh);
                        hijo = new Edge(neigh.vertex, cost);
                        if (!priorityQueueAristas.Contains(hijo))
                        {
                            priorityQueueAristas.Add(hijo);
                        }
                    }
                    Debug.Log("TERMINA");
                }
            }
        }

        Debug.Log("Devuelve camino");
        Debug.Log("longitud " + longitud);
        return new List<Vertex>();
    }

    private List<Vertex> BuildPath(int srcId, int dstId, ref int[] prevList)
    {
        List<Vertex> path = new List<Vertex>();
        int prev = dstId;
        do
        {
            path.Add(vertices[prev]);
            prev = prevList[prev];
        } while (prev != srcId);
        return path;
    }

    // Heurística de distancia Manhattan
    public float ManhattanDist(Vertex a, Vertex b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        Debug.Log("B");
        float ac = Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
        Debug.Log("COSTE H: " + ac);
        return ac;
    }
}
