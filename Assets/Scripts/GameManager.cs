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
                Debug.Log("No hay game manager");
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
            }
            return _instance;
        }
    }



    //public GameObject particles;

    GameObject casillaSeleccionada;

    public void setCasillaSeleccionada(GameObject casilla)
    {

        GameObject.FindGameObjectsWithTag("Particulas")[0].GetComponentInChildren<ParticleSystem>().Play();

        //si hay alguna seleccionada devolver la anterior a la normlaidad 
        if (casillaSeleccionada != null)
        {
            casillaSeleccionada.GetComponent<MeshRenderer>().material.color = Color.green;
        }

        //remarcar la nueva
        casillaSeleccionada = casilla;
        casillaSeleccionada.GetComponent<MeshRenderer>().material.color = Color.grey;
        GameObject.FindGameObjectsWithTag("Particulas")[0].transform.position = casillaSeleccionada.transform.position;



    }


    public void deseleccionarCasilla()
    {
        GameObject.FindGameObjectsWithTag("Particulas")[0].GetComponentInChildren<ParticleSystem>().Stop();
        if (casillaSeleccionada != null)
        {
            casillaSeleccionada.GetComponent<MeshRenderer>().material.color = Color.green;

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
}
