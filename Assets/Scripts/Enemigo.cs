using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TipoEnemigo { DEFAULT = 0, RAPIDO = 1, FUERTE = 2 }

public class Enemigo : MonoBehaviour
{
    public int vidas = 1;
    public TipoEnemigo tipoEnem = TipoEnemigo.DEFAULT;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("VElocidad enemigo: " + GetComponent<Rigidbody>().velocity);
    }


    public void recibeDanho(int danho)
    {

        //Debug.Log("Impacta");
        vidas -= danho;
        if (Spawner.enemies.Count > 0 && vidas <= 0)
        {
            Spawner.enemies.RemoveAt(0);
            EnemyMovement enemyMov = GetComponent<EnemyMovement>();
            if (enemyMov != null)
            {
                enemyMov.setMuerto(true);
            }
            GameManager.Instance.SumaDinero(((int)tipoEnem + 1) * 10);
            GameManager.Instance.enemigoMuerto();
            Destroy(this.gameObject);

        }


    }


    private void OnDestroy()
    {
        GameManager.Instance.removeFromTowers(this.gameObject);
    }
}
