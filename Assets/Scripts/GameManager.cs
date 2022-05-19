using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{



    private static GameManager _instance;
    public static List<GameObject> _towers;

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
        casillaSeleccionada = casilla;
        casillaSeleccionada.GetComponent<MeshRenderer>().material.color = Color.grey;
        GameObject.FindGameObjectsWithTag("Particulas")[0].transform.position = casillaSeleccionada.transform.position;
        showRadius(true);



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
    }



    void Start()
    {
        _towers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("TamTorres: " + _towers.Count);

    }


    public void showRadius( bool activar)
    {
        if (casillaSeleccionada.transform.childCount > 0)
        {
            casillaSeleccionada.transform.GetChild(0).GetComponent<LineRenderer>().enabled = activar;
            casillaSeleccionada.transform.GetChild(0).GetComponent<DrawRadius>().enabled = activar;
        }
       
    }

    public void addTower(GameObject t) { _towers.Add(t); } 
    public void removeTower(GameObject t) { _towers.Remove(t); } 
}
