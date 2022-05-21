using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private LayerMask selectableLayer;
    [SerializeField]
    private LayerMask UILayer;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit rayHit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit,Mathf.Infinity, selectableLayer))
            {
                GameManager.Instance.setCasillaSeleccionada(rayHit.collider.gameObject);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameManager.Instance.deseleccionarCasilla();

        }
    }
}
