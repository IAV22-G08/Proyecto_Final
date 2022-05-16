using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    // Start is called before the first frame update
    private float radioDestruccion;
    private Vector3 posIni;

    public float tiempoDestruccion;
    void Start()
    {
        posIni = transform.position;
        Invoke("Destruirse", tiempoDestruccion);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(posIni, this.transform.position) > radioDestruccion / 2)
        {
            Destroy(this.gameObject);

        }
    }

    private void Destruirse()
    {
        Destroy(this.gameObject);
    }

    public void setRadioDestruccion(float radio)
    {
        radioDestruccion = radio;
    }
}
