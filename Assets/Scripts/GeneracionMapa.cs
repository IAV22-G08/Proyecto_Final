using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneracionMapa : Graph
{
    //Prefabs
    public GameObject casillaMapa;
    public GameObject floorPrefab;
    public GameObject inicioPrefab;
    public GameObject exitPrefab;
    public GameObject spawnerPrefab;
    public GameObject waypoints;
    public GameObject waypointsList;
    public GameObject objetivoPrefab;

    //DatosMapa
    [SerializeField]
    private int anchoMapa;
    [SerializeField]
    private int altoMapa;

    //Listas
    private List<GameObject> casillasCamino = new List<GameObject>();
    private List<GameObject> casillasCamino2 = new List<GameObject>();
    private List<GameObject> casillasMapa = new List<GameObject>();
    private List<GameObject> casillasBorde = new List<GameObject>();

    public GameObject[] decorations=new GameObject[3];

    public List<GameObject> esquinas = new List<GameObject>();
    public List<GameObject> esquinas2 = new List<GameObject>();

    GameObject[] vertexObjs;
    bool[,] mapVertices;
    float defaultCost = 1f;

    private bool x = false;
    private bool y = false;
    private GameObject casillaActual;
    private GameObject casillaIncio;
    private GameObject casillaExit;
    private GameObject casillaFloor;
    private int index;
    private int nextIndex;
    int casillasCaminables = 0;

    private GameObject previo;

    public Mesh mesh;
    public Material mat1, mat2;

    private List<GameObject> getBordeSuperior()
    {
        List<GameObject> casillasBorde = new List<GameObject>();

        for (int x = anchoMapa * (altoMapa - 1); x < anchoMapa * altoMapa; x++)
        {
            casillasBorde.Add(casillasMapa[x]);
        }

        return casillasBorde;
    }
    private List<GameObject> getBordeInferior()
    {
        List<GameObject> casillasBorde = new List<GameObject>();

        for (int x = 0; x < anchoMapa; x++)
        {
            casillasBorde.Add(casillasMapa[x]);
        }

        return casillasBorde;
    }
    private List<GameObject> getBordeIzq()
    {
        List<GameObject> casillasBorde = new List<GameObject>();
        int x = 0;
        while (x < anchoMapa * altoMapa)
        {
            casillasBorde.Add(casillasMapa[x]);
            x = x + anchoMapa;
        }
        return casillasBorde;
    }
    private List<GameObject> getBordeDer()
    {
        List<GameObject> casillasBorde = new List<GameObject>();
        int x = anchoMapa - 1;
        while (x < anchoMapa * altoMapa)
        {
            casillasBorde.Add(casillasMapa[x]);
            x = x + anchoMapa;
        }
        return casillasBorde;
    }

    public override void Load()
    {
        generaMapa();
    }

    //MAPA ----------------------------------------------------------------------------
    private void generaMapa()
    {
        int id = 0;

        vertices = new Vertex[altoMapa * anchoMapa];
        neighbors = new List<List<Vertex>>(altoMapa * anchoMapa);
        costs = new List<List<float>>(altoMapa * anchoMapa);
        vertexObjs = new GameObject[altoMapa * anchoMapa];
        mapVertices = new bool[altoMapa, anchoMapa];

        for (int i = 0; i < anchoMapa; i++)
        {
            for (int j = 0; j < altoMapa; j++)
            {
                //Debug.Log("Casilla x: " + i + "  y: " + j);
                id = GridToId(j, i);
                vertexObjs[id] = Instantiate(casillaMapa);
                int rnad = UnityEngine.Random.Range(0, 3);
                int rObs = UnityEngine.Random.Range(0, 10);
                if (rObs < 2)
                {

                    if (rnad == 0)
                    {
                        Instantiate(decorations[0], vertexObjs[id].transform);
                    }
                    else if (rnad == 1)
                    {
                        Instantiate(decorations[1], vertexObjs[id].transform);

                    }
                    else
                    {
                        Instantiate(decorations[2], vertexObjs[id].transform);

                    }
                }
                
                casillasMapa.Add(vertexObjs[id]);
                vertexObjs[id].transform.position = new Vector3(i, -0.5f, j);
                Vertex v = vertexObjs[id].AddComponent<Vertex>();
                v.id = id;
                vertices[id] = v;
                neighbors.Add(new List<Vertex>());
                costs.Add(new List<float>());
            }
        }

        List<GameObject> casillasArriba = getBordeSuperior();
        List<GameObject> casillasAbajo = getBordeInferior();
        List<GameObject> casillasIzq = getBordeIzq();
        List<GameObject> casillasDer = getBordeDer();

        GameObject inicio;
        GameObject final;

        int rand1 = UnityEngine.Random.Range(0, anchoMapa);
        int rand2 = UnityEngine.Random.Range(0, anchoMapa);

        int tipoMapa = UnityEngine.Random.Range(0, 2);
        if (tipoMapa == 0)
        {
            inicio = casillasArriba[rand1];
            final = casillasAbajo[rand2];
        }
        else
        {
            inicio = casillasDer[rand1];
            final = casillasIzq[rand2];
        }



        casillaActual = inicio;
        //Si empieza desde la izquierda, se mueve hacia la derecha, sino hacia abajo
        if (tipoMapa == 0)
        {
            moveDown();

        }
        else moveLeft();

        while (!x || !y)
        {
            int number = UnityEngine.Random.Range(0, 2);
            if (!x && number == 0)
            {
                if (casillaActual.transform.position.z > final.transform.position.z)
                {
                    moveLeft();
                }
                else if (casillaActual.transform.position.z < final.transform.position.z)
                {
                    moveRight();
                }
                else
                {
                    x = true;
                }
            }
            else if (!y && number == 1)
            {
                if (casillaActual.transform.position.x > final.transform.position.x)
                {
                    moveDown();
                }
                else if (casillaActual.transform.position.x < final.transform.position.x)
                {
                    moveUp();
                }
                else
                {
                    y = true;
                }
            }
        }
        casillasCamino.Add(final);

        x = false;
        y = false;
        casillaActual = inicio;

        if (tipoMapa == 0)
        {
            moveDown2();

        }
        else moveLeft2();

        while (!x)
        {
            if (casillaActual.transform.position.z > final.transform.position.z)
            {
                moveLeft2();
            }
            else if (casillaActual.transform.position.z < final.transform.position.z)
            {
                moveRight2();
            }
            else
            {
                x = true;
            }
        }
        while (!y)
        {
            if (casillaActual.transform.position.x > final.transform.position.x)
            {
                moveDown2();
            }
            else if (casillaActual.transform.position.x < final.transform.position.x)
            {
                moveUp2();
            }
            else
            {
                y = true;
            }
        }
        casillasCamino2.Add(final);

        //Cambia la malla del mapa para hacerla camino
        int index = 0;
        foreach (GameObject obj in casillasCamino)
        {
            if (obj == casillasCamino[0])
            {
                mapVertices[(int)casillasCamino[index].transform.position.x, (int)casillasCamino[index].transform.position.z] = true;
                casillasCaminables++;

                Vector3 pos = casillasCamino[index].transform.position;
                Destroy(casillasCamino[index]);
                //Debug.Log("GENERA INICIO x: " + pos.x + "   y: " + pos.z);
                int b = GridToId((int)pos.z, (int)pos.x);
                //Debug.Log("AHA: " + b);
                vertexObjs[b] = Instantiate(inicioPrefab);
                casillaIncio = vertexObjs[b];
                vertexObjs[b].transform.position = pos;
                Vertex v = vertexObjs[b].AddComponent<Vertex>();
                v.id = b;
                vertices[b] = v;
                //Debug.Log("CASILLA INICIO pos: " + vertices[b].transform.position);
                casillasMapa.Add(casillaIncio);
                casillaIncio.transform.position = pos;

                Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                Instantiate(spawnerPrefab, a, transform.rotation);
                //Instantiate(waypoints, a, transform.rotation, waypointsList.transform);

                //foreach (GameObject esq in esquinas)
                //{
                //    Instantiate(waypoints, esq.transform.position, transform.rotation, waypointsList.transform);
                //}
                //obj.GetComponent<MeshFilter>().mesh = mesh;
                //obj.transform.Translate(new Vector3(0.5f, 0, 0));
                //obj.GetComponent<Renderer>().material = mat1;
            }
            else if (obj == casillasCamino[casillasCamino.Count - 1])
            {
                mapVertices[(int)casillasCamino[index].transform.position.x, (int)casillasCamino[index].transform.position.z] = true;
                casillasCaminables++;

                Vector3 pos = casillasCamino[index].transform.position;
                Destroy(casillasCamino[index]);
                //Debug.Log("GENERA EXIT x: " + pos.x + "   y: " + pos.z);
                int b = GridToId((int)pos.z, (int)pos.x);
                //Debug.Log("AHA: " + b);
                vertexObjs[b] = Instantiate(exitPrefab);
                casillaExit = vertexObjs[b];
                vertexObjs[b].transform.position = pos;
                Vertex v = vertexObjs[b].AddComponent<Vertex>();
                v.id = b;
                vertices[b] = v;
                //Debug.Log("CASILLA EXIT pos: " + vertices[b].transform.position);
                casillasMapa.Add(casillaExit);
                casillaExit.transform.position = pos;

                Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                Instantiate(objetivoPrefab, a, transform.rotation);
                //Instantiate(waypoints, a, transform.rotation, waypointsList.transform);
                //obj.GetComponent<MeshFilter>().mesh = mesh;
                //obj.transform.Translate(new Vector3(0.5f, 0, 0));
                //obj.GetComponent<Renderer>().material = mat2;
            }
            else
            {
                mapVertices[(int)casillasCamino[index].transform.position.x, (int)casillasCamino[index].transform.position.z] = true;
                casillasCaminables++;

                Vector3 pos = casillasCamino[index].transform.position;
                Destroy(casillasCamino[index]);
                //Debug.Log("GENERA SUELO x: " + pos.x + "   y: " + pos.z);
                int b = GridToId((int)pos.z, (int)pos.x);
                //Debug.Log("AHA: " + b);
                vertexObjs[b] = Instantiate(floorPrefab);
                casillaFloor = vertexObjs[b];
                vertexObjs[b].transform.position = pos;
                Vertex v = vertexObjs[b].AddComponent<Vertex>();
                v.id = b;
                vertices[b] = v;
                //Debug.Log("CASILLA SUELO pos: " + vertices[b].transform.position);
                casillasMapa.Add(casillaFloor);
                casillaFloor.transform.position = pos;

                //obj.GetComponent<MeshFilter>().mesh = mesh;
                //obj.transform.Translate(new Vector3(0.5f, 0, 0));
            }
            index++;
        }

        index = 0;
        foreach (GameObject obj in casillasCamino2)
        {
            if (obj == casillasCamino2[0])
            {
                if (!mapVertices[(int)casillasCamino2[index].transform.position.x, (int)casillasCamino2[index].transform.position.z])
                {
                    mapVertices[(int)casillasCamino2[index].transform.position.x, (int)casillasCamino2[index].transform.position.z] = true;

                    Vector3 pos = casillasCamino2[index].transform.position;
                    Destroy(casillasCamino2[index]);
                    casillaIncio = Instantiate(inicioPrefab);
                    casillasMapa.Add(casillaIncio);
                    casillaIncio.transform.position = pos;

                    Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                    //Instantiate(waypoints, a, transform.rotation, waypointsList.transform);
                }

                //foreach (GameObject esq in esquinas2)
                //{
                //    Instantiate(waypoints, esq.transform.position, transform.rotation, waypointsList.transform);
                //}
                //obj.GetComponent<MeshFilter>().mesh = mesh;
                //obj.transform.Translate(new Vector3(0.5f, 0, 0));
                //obj.GetComponent<Renderer>().material = mat1;
            }
            else if (obj == casillasCamino2[casillasCamino2.Count - 1])
            {
                if (!mapVertices[(int)casillasCamino2[index].transform.position.x, (int)casillasCamino2[index].transform.position.z])
                {
                    mapVertices[(int)casillasCamino2[index].transform.position.x, (int)casillasCamino2[index].transform.position.z] = true;

                    //Vector3 pos = casillasCamino2[index].transform.position;
                    //Destroy(casillasCamino2[index]);
                    //casillaExit = Instantiate(exitPrefab);
                    //casillasMapa.Add(casillaExit);
                    //casillaExit.transform.position = pos;

                    //Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                    //Instantiate(waypoints, a, transform.rotation, waypointsList.transform);
                    //obj.GetComponent<MeshFilter>().mesh = mesh;
                    //obj.transform.Translate(new Vector3(0.5f, 0, 0));
                    //obj.GetComponent<Renderer>().material = mat2;
                }
            }
            else
            {
                if (!mapVertices[(int)casillasCamino2[index].transform.position.x, (int)casillasCamino2[index].transform.position.z])
                {
                    mapVertices[(int)casillasCamino2[index].transform.position.x, (int)casillasCamino2[index].transform.position.z] = true;
                    casillasCaminables++;

                    Vector3 pos = casillasCamino2[index].transform.position;
                    Destroy(casillasCamino2[index]);
                    //Debug.Log("GENERA SUELO x: " + pos.x + "   y: " + pos.z);
                    int b = GridToId((int)pos.z, (int)pos.x);
                    //Debug.Log("AHA: " + b);
                    vertexObjs[b] = Instantiate(floorPrefab);
                    casillaFloor = vertexObjs[b];
                    vertexObjs[b].transform.position = pos;
                    Vertex v = vertexObjs[b].AddComponent<Vertex>();
                    v.id = b;
                    vertices[b] = v;
                    //Debug.Log("CASILLA SUELO pos: " + vertices[b].transform.position);
                    casillasMapa.Add(casillaFloor);
                    casillaFloor.transform.position = pos;

                    //obj.GetComponent<MeshFilter>().mesh = mesh;
                    //obj.transform.Translate(new Vector3(0.5f, 0, 0));
                }
            }
            index++;
        }

        //Debug.Log("CASILLAS CAMINABLES : " + casillasCaminables);
        casillasCamino[0] = casillaIncio;
        casillasCamino[casillasCamino.Count - 1] = casillaExit;

        //casillasCamino2[0] = casillaIncio;
        //casillasCamino2[casillasCamino2.Count - 1] = casillaExit;
        //casillasCamino2[0].transform.Translate(0, -0.5f, 0);
        //casillasCamino2[casillasCamino2.Count - 1].transform.Translate(0, -0.5f, 0);

        //Crea y pinta los bordes de negro
        for (int l = -1; l < altoMapa + 1; l++)
        {
            if (l == -1 || l == altoMapa)
            {
                for (int k = -1; k < anchoMapa + 1; k++)
                {
                    GameObject nuevoBorde = Instantiate(casillaMapa);
                    nuevoBorde.transform.position = new Vector3(l, 0, k);
                    nuevoBorde.GetComponent<Renderer>().material.color = Color.black;

                }
            }
            else
            {
                GameObject nuevoBorde = Instantiate(casillaMapa);
                nuevoBorde.transform.position = new Vector3(l, 0, -1);
                nuevoBorde.GetComponent<Renderer>().material.color = Color.black;
                GameObject nuevoBorde2 = Instantiate(casillaMapa);
                nuevoBorde2.transform.position = new Vector3(l, 0, anchoMapa);
                nuevoBorde2.GetComponent<Renderer>().material.color = Color.black;
            }
        }

        for (int i = 0; i < anchoMapa; i++)
        {
            for (int j = 0; j < altoMapa; j++)
            {
                //Debug.Log("Vertice i: " + i + "   j: " + j);
                SetNeighbours(j, i);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Load();
        waypointsList.GetComponent<Waypoints>().LeerWaypoints();
    }

    private void moveUp()
    {
        casillasCamino.Add(casillaActual);
        if (casillasCamino.Count >= 2)
        {
            previo = casillasCamino[casillasCamino.Count - 2];
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index + anchoMapa;
            casillaActual = casillasMapa[nextIndex];
            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {

                //Debug.Log(casillaActual.transform.position.z + " " + previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " ");
                esquinas.Add(anterior);
            }
        }
        else
        {
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index + anchoMapa;
            casillaActual = casillasMapa[nextIndex];
        }

    }
    private void moveDown()
    {
        casillasCamino.Add(casillaActual);
        if (casillasCamino.Count >= 2)
        {
            previo = casillasCamino[casillasCamino.Count - 2];
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index - anchoMapa;
            casillaActual = casillasMapa[nextIndex];

            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {
                //Debug.Log(casillaActual.transform.position.z + " " + previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " "); esquinas.Add(anterior);
            }
        }
        else
        {
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index - anchoMapa;
            casillaActual = casillasMapa[nextIndex];
        }
    }
    private void moveLeft()
    {
        casillasCamino.Add(casillaActual);
        if (casillasCamino.Count >= 2)
        {
            previo = casillasCamino[casillasCamino.Count - 2];
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index - 1;
            casillaActual = casillasMapa[nextIndex];
            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {
                //Debug.Log(casillaActual.transform.position.z + " " + previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " "); esquinas.Add(anterior);
            }
        }
        else
        {
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index - 1;
            casillaActual = casillasMapa[nextIndex];
        }

    }
    private void moveRight()
    {
        casillasCamino.Add(casillaActual);
        if (casillasCamino.Count >= 2)
        {
            previo = casillasCamino[casillasCamino.Count - 2];

            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index + 1;
            casillaActual = casillasMapa[nextIndex];
            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {
                //Debug.Log(casillaActual.transform.position.z + " " + previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " "); esquinas.Add(anterior);
            }
        }
        else
        {
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index + 1;
            casillaActual = casillasMapa[nextIndex];
        }

    }
    public List<GameObject> getEsquinas()
    {
        return esquinas;
    }
    public List<GameObject> getEsquinas2()
    {
        return esquinas2;
    }

    private void moveUp2()
    {
        casillasCamino2.Add(casillaActual);
        if (casillasCamino2.Count >= 2)
        {
            previo = casillasCamino2[casillasCamino2.Count - 2];
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index + anchoMapa;
            casillaActual = casillasMapa[nextIndex];
            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {

                //Debug.Log(casillaActual.transform.position.z + " " + previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " ");
                esquinas2.Add(anterior);
            }
        }
        else
        {
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index + anchoMapa;
            casillaActual = casillasMapa[nextIndex];
        }

    }
    private void moveDown2()
    {
        casillasCamino2.Add(casillaActual);
        if (casillasCamino2.Count >= 2)
        {
            previo = casillasCamino2[casillasCamino2.Count - 2];
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index - anchoMapa;
            casillaActual = casillasMapa[nextIndex];

            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {
                //Debug.Log(casillaActual.transform.position.z + " " + previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " "); esquinas2.Add(anterior);
            }
        }
        else
        {
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index - anchoMapa;
            casillaActual = casillasMapa[nextIndex];
        }
    }
    private void moveLeft2()
    {
        casillasCamino2.Add(casillaActual);
        if (casillasCamino2.Count >= 2)
        {
            previo = casillasCamino2[casillasCamino2.Count - 2];
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index - 1;
            casillaActual = casillasMapa[nextIndex];
            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {
                //Debug.Log(casillaActual.transform.position.z + " " + previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " "); esquinas2.Add(anterior);
            }
        }
        else
        {
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index - 1;
            casillaActual = casillasMapa[nextIndex];
        }

    }
    private void moveRight2()
    {
        casillasCamino2.Add(casillaActual);
        if (casillasCamino2.Count >= 2)
        {
            previo = casillasCamino2[casillasCamino2.Count - 2];

            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index + 1;
            casillaActual = casillasMapa[nextIndex];
            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {
                //Debug.Log(casillaActual.transform.position.z + " " + previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " "); esquinas2.Add(anterior);
            }
        }
        else
        {
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index + 1;
            casillaActual = casillasMapa[nextIndex];
        }
    }

    // -------------------------------- GRAPH ----------------------------------------

    private int GridToId(int x, int y)
    {
        //Debug.Log("x: " + x + "   y: " + y);
        return Math.Max(altoMapa, anchoMapa) * y + x;
        //De abajo derecha a arriba izquierda
    }

    protected void SetNeighbours(int x, int y, bool get8 = false)
    {
        int col = x;
        int row = y;
        int i, j;
        int vertexId = GridToId(x, y);
        //Debug.Log("ID: " + vertexId);
        neighbors[vertexId] = new List<Vertex>();
        costs[vertexId] = new List<float>();
        Vector2[] pos = new Vector2[0];
        if (get8)
        {
            pos = new Vector2[8];
            int c = 0;
            for (i = row - 1; i <= row + 1; i++)
            {
                for (j = col - 1; j <= col; j++)
                {
                    pos[c] = new Vector2(j, i);
                    c++;
                }
            }
        }
        else
        {
            pos = new Vector2[4];
            pos[0] = new Vector2(col, row - 1);
            pos[1] = new Vector2(col - 1, row);
            pos[2] = new Vector2(col + 1, row);
            pos[3] = new Vector2(col, row + 1);
        }
        foreach (Vector2 p in pos)
        {
            i = (int)p.y;
            j = (int)p.x;
            if (i < 0 || j < 0)
                continue;
            if (i >= anchoMapa || j >= altoMapa)
                continue;
            if (i == row && j == col)
                continue;
            if (!mapVertices[i, j])
                continue;
            //Debug.Log("Vertice EDGE i: " + i + "   j: " + j);
            int id = GridToId(j, i);
            neighbors[vertexId].Add(vertices[id]);
            costs[vertexId].Add(defaultCost);
        }
    }

    public override Vertex GetNearestVertex(Vector3 position)
    {
        //Debug.Log("NV x: " + position.x + "   y: " + position.z);
        int col = (int)(position.x);
        int row = (int)(position.z);
        Vector2 p = new Vector2(col, row);
        List<Vector2> explored = new List<Vector2>();
        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(p);
        do
        {
            p = queue.Dequeue();
            col = (int)p.x;
            row = (int)p.y;
            int id = GridToId(row, col);
            if (mapVertices[col, row])
            {
                //Debug.Log("ID NearestVertex: " + id);
                //Debug.Log("Position NearestVertex: " + vertices[id].transform.position);
                return vertices[id];
            }

            if (!explored.Contains(p))
            {
                explored.Add(p);
                int i, j;
                for (i = row - 1; i <= row + 1; i++)
                {
                    for (j = col - 1; j <= col + 1; j++)
                    {
                        if (i < 0 || j < 0)
                            continue;
                        if (j >= anchoMapa || i >= altoMapa)
                            continue;
                        if (i == row && j == col)
                            continue;
                        queue.Enqueue(new Vector2(j, i));
                    }
                }
            }
        } while (queue.Count != 0);
        return null;
    }
}



//INTENTO DE GENERAR CAMINOS ESPECIALES

/*int previo = -1;
int random=0;
int randomAnt = 2;
while (random < 15)
{
    random++;
    int randomR = Random.Range(0, 4);
    if ((randomR == 0 || randomR == 1) && (randomAnt == 2 || randomAnt == 3) && (previo == 0 || previo == 1) && previo != randomR)
    {
        if (randomR == 0)
        {
            moveLeft();
        }
        else moveRight();
    }
    else if ((randomR == 2 || randomR == 3) && (randomAnt == 0 || randomAnt == 1) && (previo == 2 || previo == 3) && previo != randomR)
    {
        if (randomR == 2)
        {
            moveDown();
        }
        else moveUp();
    }
    else if (randomR==2&&(randomAnt==0||randomAnt==1))
    {
        int options = Random.Range(0, 2);
        if (options == 0&&randomAnt==0)
        {
            if (!(casillasMapa.IndexOf(casillaActual) - 1 < 0) && !(casillasMapa.IndexOf(casillaActual) - 1 % anchoMapa == 0))
                moveLeft();
        }
        else if (options == 0 && randomAnt == 1)
        {
            if (!(casillasMapa.IndexOf(casillaActual) + 1 > anchoMapa * altoMapa - 1) && !(casillasMapa.IndexOf(casillaActual) + 1 % anchoMapa == 0))
                moveRight();
        }
        else if (options == 1)
        {
            if (!(casillasMapa.IndexOf(casillaActual) + anchoMapa > anchoMapa * altoMapa - 1))
            {
                moveUp();
            }
        }
    }
    else if (randomR == 1 && (randomAnt == 2 || randomAnt == 3))
    {
        int options = Random.Range(0, 2);
        if (options == 0 && randomAnt == 2)
        {
            if (!(casillasMapa.IndexOf(casillaActual) - anchoMapa < 0))
                moveDown();
        }
        else if (options == 0 && randomAnt == 3)
        {
            if (!(casillasMapa.IndexOf(casillaActual) + anchoMapa > anchoMapa * altoMapa - 1))
            {
                moveUp();
            }
        }
        else if (options == 1)
        {
            if (!(casillasMapa.IndexOf(casillaActual) - 1 < 0) && !(casillasMapa.IndexOf(casillaActual) - 1 % anchoMapa == 0))
                moveLeft();
        }
    }

    else if (randomR == 0 && (randomAnt == 2 || randomAnt == 3))
    {
        int options = Random.Range(0, 2);
        if (options == 0 && randomAnt == 2)
        {
            if (!(casillasMapa.IndexOf(casillaActual) - anchoMapa < 0))
                moveDown();
        }
        else if (options == 0 && randomAnt == 3)
        {
            if (!(casillasMapa.IndexOf(casillaActual) + anchoMapa > anchoMapa * altoMapa - 1))
            {
                moveUp();
            }
        }
        else if (options == 1)
        {
            if (!(casillasMapa.IndexOf(casillaActual) + 1 > anchoMapa * altoMapa - 1) && !(casillasMapa.IndexOf(casillaActual) + 1 % anchoMapa == 0))
                moveRight();
        }
    }
    else if (randomR == 3 && (randomAnt == 0 || randomAnt == 1))
    {
        int options = Random.Range(0, 2);
        if (options == 0 && randomAnt == 0)
        {
            if (!(casillasMapa.IndexOf(casillaActual) - 1 < 0) && !(casillasMapa.IndexOf(casillaActual) - 1 % anchoMapa == 0))
                moveLeft();
        }
        else if (options == 0 && randomAnt == 1)
        {
            if (!(casillasMapa.IndexOf(casillaActual) + 1 > anchoMapa * altoMapa - 1) && !(casillasMapa.IndexOf(casillaActual) + 1 % anchoMapa == 0))
                moveRight();
        }
        else if (options == 1)
        {
            if (!(casillasMapa.IndexOf(casillaActual) - anchoMapa < 0))
                moveDown();
        }
    }
    else if (randomR == 3)
    {
        if (!(casillasMapa.IndexOf(casillaActual) + anchoMapa > anchoMapa * altoMapa - 1))
            moveUp();
    }
    else if (randomR == 2)
    {
        if (!(casillasMapa.IndexOf(casillaActual) - anchoMapa < 0))
            moveDown();
    }
    else if (randomR == 1)
    {
        if (!(casillasMapa.IndexOf(casillaActual) + 1 > anchoMapa * altoMapa - 1)&&!(casillasMapa.IndexOf(casillaActual)+1%anchoMapa==0))
            moveRight();
    }
    else if (randomR == 0)
    {
        if (!(casillasMapa.IndexOf(casillaActual) - 1 < 0)&&!(casillasMapa.IndexOf(casillaActual) - 1 % anchoMapa == 0))
            moveLeft();
    }
    previo = randomAnt;
    randomAnt = randomR;
}*/