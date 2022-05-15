using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{



    private static GameManager _instance;
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

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void showRadius( bool activar)
    {
        Debug.Log("MuestraRadio");
        if (casillaSeleccionada.transform.childCount > 0)
        {
            Debug.Log("MuestraRadioDentro");
            casillaSeleccionada.GetComponent<LineRenderer>().enabled = activar;
            casillaSeleccionada.GetComponent<DrawRadius>().enabled = activar;
        }
       
    }
}
