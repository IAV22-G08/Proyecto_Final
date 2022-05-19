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
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Proyectil>() != null)
        {
            Debug.Log("Impacta");
            vidas--;
            if (Spawner.enemies.Count > 0 && vidas <= 0)
            {
                Debug.Log("Ha llegado al objetivo");
                Spawner.enemies.RemoveAt(0);
                Destroy(this.gameObject);
                Debug.Log("Enemigos restantes: " + Spawner.enemies.Count);
                EnemyMovement enemyMov = GetComponent<EnemyMovement>();
                if (enemyMov != null)
                {
                    enemyMov.setMuerto(true);
                }
            }
        }
       
    }
}
