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


    [SerializeField]
    GameObject particles;

    GameObject casillaSeleccionada;

    public void setCasillaSeleccionada(GameObject casilla)
    {
        //hay que hacer que la actual casilla seleccionada vuelva a ser una normal y luego poner esta como seleccionada
        if(casillaSeleccionada != null && casilla != null)
        {
            casillaSeleccionada.GetComponent<MeshRenderer>().enabled = true;
            //particles.transform.position = casillaSeleccionada.transform.position;
            
        }
        if(casilla != null)
        {
            //particles.SetActive(true);
            casillaSeleccionada = casilla;
            casillaSeleccionada.GetComponent<MeshRenderer>().enabled = false;
            //particles.transform.position = casillaSeleccionada.transform.position;

        }

    }


    public void deseleccionarCasilla()
    {
        if (casillaSeleccionada != null)
        {
            //particles.SetActive(false);

            casillaSeleccionada.GetComponent<MeshRenderer>().enabled = true;
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
