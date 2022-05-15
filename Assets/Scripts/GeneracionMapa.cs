using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneracionMapa : MonoBehaviour
{
    public GameObject casillaMapa;
    public GameObject inicioPrefab;
    public GameObject exitPrefab;
    public GameObject spawnerPrefab;
    public GameObject waypoints;
    public GameObject waypointsList;

    [SerializeField]
    private int anchoMapa;
    [SerializeField]
    private int altoMapa;

    private List<GameObject> casillasCamino = new List<GameObject>();
    private List<GameObject> casillasMapa = new List<GameObject>();
    private List<GameObject> casillasBorde = new List<GameObject>();

    public List<GameObject> esquinas = new List<GameObject>();

    private bool x = false;
    private bool y = false;
    private GameObject casillaActual;
    private GameObject casillaIncio;
    private GameObject casillaExit;
    private int index;
    private int nextIndex;

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
    private void generaMapa()
    {
        for (int i = 0; i < anchoMapa; i++)
        {
            for (int j = 0; j < altoMapa; j++)
            {
                GameObject nuevaCasilla = Instantiate(casillaMapa);
                casillasMapa.Add(nuevaCasilla);
                nuevaCasilla.transform.position = new Vector3(i, 0, j);
            }
        }
        List<GameObject> casillasArriba = getBordeSuperior();
        List<GameObject> casillasAbajo = getBordeInferior();
        List<GameObject> casillasIzq = getBordeIzq();
        List<GameObject> casillasDer = getBordeDer();

        GameObject inicio;
        GameObject final;

        int rand1 = Random.Range(0, anchoMapa);
        int rand2 = Random.Range(0, anchoMapa);

        int tipoMapa = Random.Range(0, 2);
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
            int number = Random.Range(0, 2);
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
        int iterations = 0;

        //Cambia la malla del mapa para hacerla camino
        foreach (GameObject obj in casillasCamino)
        {
            if (obj == casillasCamino[0])
            {
                Vector3 pos = casillasCamino[0].transform.position;
                Destroy(casillasCamino[0]);
                casillaIncio = Instantiate(inicioPrefab);
                casillasMapa.Add(casillaIncio);
                casillaIncio.transform.position = pos;

                Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                Instantiate(spawnerPrefab, a, transform.rotation);
                Instantiate(waypoints, a, transform.rotation, waypointsList.transform);

                foreach(GameObject esq in esquinas)
                {
                    Instantiate(waypoints, esq.transform.position, transform.rotation, waypointsList.transform);
                }
                //obj.GetComponent<MeshFilter>().mesh = mesh;
                //obj.transform.Translate(new Vector3(0.5f, 0, 0));
                //obj.GetComponent<Renderer>().material = mat1;
            }
            else if (obj == casillasCamino[casillasCamino.Count - 1])
            {
                Vector3 pos = casillasCamino[casillasCamino.Count - 1].transform.position;
                Destroy(casillasCamino[casillasCamino.Count - 1]);
                casillaExit = Instantiate(exitPrefab);
                casillasMapa.Add(casillaExit);
                casillaExit.transform.position = pos;

                Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                Instantiate(waypoints, a, transform.rotation, waypointsList.transform);
                //obj.GetComponent<MeshFilter>().mesh = mesh;
                //obj.transform.Translate(new Vector3(0.5f, 0, 0));
                //obj.GetComponent<Renderer>().material = mat2;
            }
            else
            {
                obj.GetComponent<MeshFilter>().mesh = mesh;
                obj.transform.Translate(new Vector3(0.5f, 0, 0));
            }

        }

        casillasCamino[0] = casillaIncio;
        casillasCamino[casillasCamino.Count - 1] = casillaExit;
        casillasCamino[0].transform.Translate(0, -0.5f, 0);
        casillasCamino[casillasCamino.Count - 1].transform.Translate(0, -0.5f, 0);

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
    }
    // Start is called before the first frame update
    void Start()
    {
        generaMapa();
        waypointsList.GetComponent<Waypoints>().LeerWaypoints();
    }

    private void moveUp()
    {
        casillasCamino.Add(casillaActual);
        if (casillasCamino.Count > 2)
        {
            previo = casillasCamino[casillasCamino.Count - 2];
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index + anchoMapa;
            casillaActual = casillasMapa[nextIndex];
            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {

                Debug.Log(casillaActual.transform.position.z+" "+ previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " ");
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
        if (casillasCamino.Count > 2)
        {
            previo = casillasCamino[casillasCamino.Count - 2];
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index - anchoMapa;
            casillaActual = casillasMapa[nextIndex];

            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {
                Debug.Log(casillaActual.transform.position.z + " " + previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " "); esquinas.Add(anterior);
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
        if (casillasCamino.Count > 2)
        {
            previo = casillasCamino[casillasCamino.Count - 2];
            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index - 1;
            casillaActual = casillasMapa[nextIndex];
            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {
                Debug.Log(casillaActual.transform.position.z + " " + previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " "); esquinas.Add(anterior);
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
        if (casillasCamino.Count > 2)
        {
            previo = casillasCamino[casillasCamino.Count - 2];

            GameObject anterior = casillaActual;
            index = casillasMapa.IndexOf(casillaActual);
            nextIndex = index + 1;
            casillaActual = casillasMapa[nextIndex];
            if (casillaActual.transform.position.z != previo.transform.position.z && casillaActual.transform.position.x != previo.transform.position.x)
            {
                Debug.Log(casillaActual.transform.position.z + " " + previo.transform.position.z + " " + casillaActual.transform.position.x + " " + previo.transform.position.x + " "); esquinas.Add(anterior);
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