using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button resetButton;
    public Button crearTorre;
    public Button cambiarCompoTorre;
    public Button torreRapida;
    public Button torrePesada;
    public Text textoComporTorre;
    public Text dinereteText;
    public Text vidasText;
    public Text rondasText;
    public Text enemigosText;

    public List<Text> enemigosQuePasaron;
    void Start()
    {
        Debug.Log("Start UIMANAGER");

        GameManager.Instance.setUIManger(this);
    }

    public void updateMoney(int money)
    {
        dinereteText.text = money.ToString();
    }

    public void updateVidas(int vidas)
    {
        vidasText.text = vidas.ToString();
    }

    public void updateEnemigos(int enem)
    {
        enemigosText.text = enem.ToString();
    }

    public void updateEnemigosPasaron(List<int> lista)
    {
        enemigosQuePasaron[0].text = lista[0].ToString();
        enemigosQuePasaron[1].text = lista[1].ToString();
        enemigosQuePasaron[2].text = lista[2].ToString();
    }
    public void updateRondas(int rondas)
    {
        rondasText.text = rondas.ToString();
    }

    public void DebugMode()
    {
        GameManager.Instance.SumaDinero(99999);
        GameManager.Instance.QuitaVidas(-1000);
    }

    public void SumaRondas()
    {
        GameManager.Instance.SumaRonda(5);
    }


    public void StartRound()
    {
        if (!GameManager.Instance.getRondaActiva())
            GameManager.Instance.setRondaActiva(true);
    }
    public void ActualizaComporTorreText()
    {
        GameObject torreSeleccionada = null;
        if (GameManager.Instance.getSelectedCasilla() && GameManager.Instance.getSelectedCasilla().transform.childCount > 0)
        {
            torreSeleccionada = GameManager.Instance.getSelectedCasilla().transform.GetChild(0).gameObject;
            Tower archerTower = torreSeleccionada.GetComponent<Tower>();
            if (archerTower)
            {
                textoComporTorre.text = archerTower.aQuienAtacar.ToString();
            }
            else
            {
                textoComporTorre.text = "Not Selected";
            }
        }
        else
        {
            textoComporTorre.text = "Not Selected";
        }




    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Reset()
    {
        SceneManager.LoadScene("EscenaFinal");
    }


}
