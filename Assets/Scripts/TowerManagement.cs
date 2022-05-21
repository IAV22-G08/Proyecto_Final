using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManagement : MonoBehaviour
{

    public GameObject archerTower;
    void Start()
    {
        
    }

    public void CrearTorre()
    {
        if (GameManager.Instance.SuficienteDinero(150))
        {
            Debug.Log("CreaTorre");
            GameObject selected = GameManager.Instance.getSelectedCasilla();
            if (selected != null && selected.transform.childCount == 0)
            {
                GameObject torre = GameObject.Instantiate(archerTower, selected.transform);
                GameManager.Instance.SumaDinero(-150);

                GameManager.Instance.addTower(torre);

                torre.transform.position = selected.transform.position;
                torre.transform.Translate(new Vector3(0, 0.5f, 0));
                //torre.transform.localScale = new Vector3(0.7f, 0.8f, 0.8f);
                GameManager.Instance.showRadius(true);
            }

            UIManager ui = GameManager.Instance.getUIManager();
            if (ui)
            {
                ui.ActualizaComporTorreText();
            }
        }
        
    }


    public void cambiarComportameintoTower()
    {
        GameObject torreSeleccionada = null;

        if (GameManager.Instance.getSelectedCasilla().transform.childCount > 0)
        {
            torreSeleccionada = GameManager.Instance.getSelectedCasilla().transform.GetChild(0).gameObject;

            Tower archerTower = torreSeleccionada.GetComponent<Tower>();
            if (archerTower)
            {
                archerTower.aQuienAtacar = (FILTRADO_ATAQUE) ( (int) (archerTower.aQuienAtacar + 1) % 3);
                GameManager.Instance._myUIManager.ActualizaComporTorreText();

            }
        }
         
    }


    public void MejorarTorreLigera()
    {
        if (!GameManager.Instance.SuficienteDinero(80)) return;



        GameObject torreSeleccionada = null;

        if (GameManager.Instance.getSelectedCasilla().transform.childCount > 0)
        {
            torreSeleccionada = GameManager.Instance.getSelectedCasilla().transform.GetChild(0).gameObject;

            Tower archerTower = torreSeleccionada.GetComponent<Tower>();
            if (archerTower && archerTower.tipoFlecha != TipoFlecha.LIGERA)
            {
                archerTower.tipoFlecha = TipoFlecha.LIGERA;
                GameManager.Instance.SumaDinero(-80);

                torreSeleccionada.GetComponent<LineRenderer>().material.color = new Color(188f/255f,0f,255f);

            }
        }
    }

    public void MejorarTorrePesasda()
    {
        if (!GameManager.Instance.SuficienteDinero(100)) return;



        GameObject torreSeleccionada = null;

        if (GameManager.Instance.getSelectedCasilla().transform.childCount > 0)
        {
            torreSeleccionada = GameManager.Instance.getSelectedCasilla().transform.GetChild(0).gameObject;

            Tower archerTower = torreSeleccionada.GetComponent<Tower>();
            if (archerTower && archerTower.tipoFlecha != TipoFlecha.PESADA)
            {
                archerTower.tipoFlecha = TipoFlecha.PESADA;
                GameManager.Instance.SumaDinero(-100);

                torreSeleccionada.GetComponent<LineRenderer>().material.color = Color.red;
            }
        }
    }

    public void DeleteTower()
    {
        GameObject selected = GameManager.Instance.getSelectedCasilla();
        if (selected != null)
        {
            GameManager.Instance.showRadius(false);

            GameObject torre;
            if (selected.transform.childCount > 0 && selected.transform.GetChild(0).GetComponent<Tower>())
            {
                torre = selected.transform.GetChild(0).gameObject;
                if (torre != null)
                {
                    GameManager.Instance.removeTower(torre);
                    Destroy(torre);
                    GameManager.Instance.getSelectedCasilla().GetComponent<MeshRenderer>().material.color = Color.green;
                    GameManager.Instance.SumaDinero(+80);

                }

            }

        }

        UIManager ui = GameManager.Instance.getUIManager();
        if (ui)
        {
            ui.ActualizaComporTorreText();
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Construir
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject selected = GameManager.Instance.getSelectedCasilla();
            if (selected != null && selected.transform.childCount == 0)
            {
                GameObject torre = GameObject.Instantiate(archerTower, selected.transform);
                GameManager.Instance.addTower(torre);

               torre.transform.position = selected.transform.position;
                torre.transform.Translate(new Vector3(0, 1, 0));
                //torre.transform.localScale = new Vector3(0.7f, 0.8f, 0.8f);
                GameManager.Instance.showRadius(true);
            }


        }//Destruir
        else if (Input.GetKeyDown(KeyCode.D))
        {
            GameObject selected = GameManager.Instance.getSelectedCasilla();
            if (selected != null)
            {
                GameManager.Instance.showRadius(false);
               
                GameObject torre;
                if (selected.transform.childCount > 0)
                {
                    torre = selected.transform.GetChild(0).gameObject;
                    if (torre != null)
                    {
                        GameManager.Instance.removeTower(torre);
                        Destroy(torre);
                    }
                        
                }
               

            }
        }

    }
}
