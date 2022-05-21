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
    void Start()
    {
        GameManager.Instance.setUIManger(this);
    }

    public void updateMoney(int money)
    {
        dinereteText.text = money.ToString();
    }

    public void DebugMode()
    {
        GameManager.Instance.SumaDinero(99999);
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
