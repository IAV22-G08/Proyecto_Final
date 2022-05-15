using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTower : MonoBehaviour
{

    public GameObject archerTower;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Construir
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject selected = GameManager.Instance.getSelectedCasilla();
            if (selected != null)
            {
                GameObject torre = GameObject.Instantiate(archerTower, selected.transform);
                torre.transform.position = selected.transform.position;
                torre.transform.Translate(new Vector3(0, 1, 0));
                torre.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                GameManager.Instance.showRadius(true);
            }


        }//Destruir
        else if (Input.GetKeyDown(KeyCode.D))
        {
            GameObject selected = GameManager.Instance.getSelectedCasilla();
            if (selected != null)
            {
                GameManager.Instance.showRadius(false);
                Destroy(selected.transform.GetChild(0).gameObject);

            }
        }
    }
}
