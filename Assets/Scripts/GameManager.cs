using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{



    private static GameManager _instance;
    public static List<GameObject> _towers;
    public UIManager _myUIManager;
    private int dinerete;
    private int vidas;
    private int round;
    private int enemigosMuertos;
    private bool rondaActiva;
    List<int> enemigosQuePasaron;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
                
            }
            return _instance;
        }
    }
    


    //public GameObject particles;

    GameObject casillaSeleccionada;
    public GameObject getSelectedCasilla() { return casillaSeleccionada; }
    public void setCasillaSeleccionada(GameObject casilla)
    {
        GameObject.FindGameObjectsWithTag("Particulas")[0].GetComponentInChildren<ParticleSystem>().Play();

        //si hay alguna seleccionada devolver la anterior a la normlaidad 
        if (casillaSeleccionada != null)
        {
            showRadius(false);
            casillaSeleccionada.GetComponent<MeshRenderer>().material.color = Color.green;
        }

        //remarcar la nueva
        _myUIManager = GameObject.FindWithTag("Canvas").GetComponent<UIManager>();

        casillaSeleccionada = casilla;
        casillaSeleccionada.GetComponent<MeshRenderer>().material.color = Color.grey;
        GameObject.FindGameObjectsWithTag("Particulas")[0].transform.position = casillaSeleccionada.transform.position;
        showRadius(true);
        _myUIManager.ActualizaComporTorreText();


    }

    public UIManager getUIManager()
    {
        return _myUIManager;
    }

    public void deseleccionarCasilla()
    {
        GameObject.FindGameObjectsWithTag("Particulas")[0].GetComponentInChildren<ParticleSystem>().Stop();
        if (casillaSeleccionada != null)
        {
            casillaSeleccionada.GetComponent<MeshRenderer>().material.color = Color.green;
            showRadius(false);

        }
        casillaSeleccionada = null;
        _myUIManager.ActualizaComporTorreText();

    }



    void Awake()
    {
        _towers = new List<GameObject>();
        Debug.Log("Start GameManager");
        dinerete = 200;
        vidas = 100;
        round = 0;
        enemigosMuertos = 0;
        rondaActiva = false;
        enemigosQuePasaron = new List<int>(3);
        for(int x = 0;  x < 3; x++)
        {
            enemigosQuePasaron.Add(0);
        }
    }


    public void pasaEnem(int indice)
    {
        enemigosQuePasaron[indice]++;
        _myUIManager.updateEnemigosPasaron(enemigosQuePasaron);
    }

    

    public bool getRondaActiva()
    {
        return rondaActiva;
    }

    public void setRondaActiva(bool activa)
    {
        rondaActiva = activa;
        Debug.Log("Se activa la ronda " + rondaActiva);
    }
    public void setUIManger(UIManager ui)
    {
        _myUIManager = ui;
        _myUIManager.updateMoney(dinerete);
        _myUIManager.updateVidas(vidas);
        _myUIManager.updateRondas(round);

        Debug.Log("Dinero inicial: " + dinerete);
    }
    // Update is called once per frame
    void Update()
    {
      //_myUIManager = GameObject.FindWithTag("Canvas").GetComponent<UIManager>();


    }

    public bool SuficienteDinero(int dineroGastar)
    {
        return dinerete >= dineroGastar;
    }
    public void SumaDinero(int dinereteAnhadir)
    {
        dinerete += dinereteAnhadir;
        _myUIManager.updateMoney(dinerete);

    }

    public void enemigoMuerto()
    {
        enemigosMuertos++;
        _myUIManager.updateEnemigos(enemigosMuertos);
    }

    public void SumaRonda(int rondaMas)
    {
        round += rondaMas;
        _myUIManager.updateRondas(round);
    }

    public int getRonda()
    {
        return round;

    }
    public void QuitaVidas(int vidas2)
    {
        vidas -= vidas2;
        _myUIManager.updateVidas(vidas);
        if (vidas <= 0)
        {
            SceneManager.LoadScene("EscenaFinal");
        }
    }
    public void showRadius( bool activar)
    {
        if (casillaSeleccionada.transform.childCount > 0)
        {
            LineRenderer lr = casillaSeleccionada.transform.GetChild(0).GetComponent<LineRenderer>();
            DrawRadius dr = casillaSeleccionada.transform.GetChild(0).GetComponent<DrawRadius>();


            if(lr)lr.enabled = activar;
            if(dr)dr.enabled = activar;
        }
       
    }

    public void removeFromTowers(GameObject gO)
    {
        for(int x = 0; x < _towers.Count; x++)
        {
           if(_towers[x]) _towers[x].GetComponent<Tower>().removeEnemyFromList(gO);
        }
    }
    public void addTower(GameObject t) { _towers.Add(t); } 

    
    public void removeTower(GameObject t) { _towers.Remove(t); } 
}
