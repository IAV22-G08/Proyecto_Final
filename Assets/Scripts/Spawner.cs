using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{

    public List<GameObject> enemigos;
    public float cooldown = 100f;
    private int enemigosRestantes;
    //private float timer = 2f;
    private bool finGen = true;

    
    private void Update()
    {
        //if (timer <= 0f)
        //{
        //    StartCoroutine(Spawn());
        //    timer = cooldown;
        //}
        //Debug.Log("Timer: " + timer);
        //timer -= Time.deltaTime;
        if (GameManager.Instance.getRondaActiva())
        {
            if (enemigosRestantes <= 0 && finGen)
            {
                finGen = false;
                //Debug.Log("UPDATE: " + GameManager.Instance.getRondaActiva());
                StartCoroutine(Spawn());
                //Spawn();
            }
            Debug.Log("EnemRest: " + enemigosRestantes);
            Debug.Log("EnemCount: " + GameManager._enemies.Count);
            if (enemigosRestantes <= 0 && GameManager._enemies.Count <= 0) //Ha terminado la ronda 100%
            {
                Debug.Log(GameManager.Instance.getRondaActiva());
                GameManager.Instance.setRondaActiva(false);
                finGen = true;
            }
        }
    }

    IEnumerator Spawn()
    {
        GameManager.Instance.SumaRonda(1);



        enemigosRestantes = GameManager.Instance.getRonda();

        int torreDebil = 0;
        if (GameManager.Instance.getRonda() % 5 == 0)
        {
            torreDebil = checkWeakTowers();
            if (torreDebil == 0) enemigosRestantes = (int)(enemigosRestantes * 2f);
        }
        //Debug.Log("Spawning...");
        //enemigos restantes tamb es el número de la ronda en la que nos encontramos
        int numEnemigos = enemigosRestantes;
        for (int i = 0; i < numEnemigos; i++)
        {
            if (GameManager.Instance.getRonda() % 5 != 0) SpawnEnemy();
            else SpawnSpecialRound((TipoFlecha)torreDebil);

            enemigosRestantes--;
            yield return new WaitForSeconds(0.5f);
        }
        //GameManager.Instance.setRondaActiva(false);

        //Debug.Log("SPAWN: " + GameManager.Instance.getRondaActiva());
    }

    void SpawnEnemy()
    {
        GameObject spawned = null;
        if (GameManager.Instance.getRonda() <= 4)
        {
            GameManager._enemies.Add(spawned = Instantiate(enemigos[(int)TipoEnemigo.DEFAULT], transform.position, transform.rotation));
            Debug.Log("Enemigos: " + GameManager._enemies.Count);
        }

        else
        {

            int x = Random.Range(0, 10);
            if (x < 3)//30% de spawn
            {
                GameManager._enemies.Add(spawned = Instantiate(enemigos[(int)TipoEnemigo.RAPIDO], transform.position, transform.rotation));
            }
            else if (x < 6)//30% de spawn
            {
                GameManager._enemies.Add(spawned = Instantiate(enemigos[(int)TipoEnemigo.FUERTE], transform.position, transform.rotation));
            }
            else//40% de spawn
            {
                GameManager._enemies.Add(spawned = Instantiate(enemigos[(int)TipoEnemigo.DEFAULT], transform.position, transform.rotation));
            }
            Debug.Log("Enemigos: " + GameManager._enemies.Count);
        }


        spawned.GetComponent<Enemigo>().vidas = spawned.GetComponent<Enemigo>().vidas * ((GameManager.Instance.getRonda() / 10) + 1);
    }


    int checkWeakTowers()
    {
        List<int> torres = new List<int>(3);

        for (int i = 0; i < 3; i++)
        {
            torres.Add(0);
        }


        for (int i = 0; i < GameManager._towers.Count; i++)
        {
            Tower t = GameManager._towers[i].GetComponent<Tower>();
            if (t)
            {
                //Con esto sacamos el número de torres de cada tipo
                torres[(int)t.tipoFlecha]++;

            }
        }

        int min = int.MaxValue;
        int indiceMin = 0;
        for (int i = 0; i < 3; i++)
        {
            if (torres[i] < min)
            {
                min = torres[i];
                indiceMin = i;
            }

        }

        return indiceMin;
    }
    void SpawnSpecialRound(TipoFlecha torreDebil)
    {
        GameObject spawned = null;
        int pEnemRapido = 25;
        int pEnemFuerte = 25;
        if (torreDebil == TipoFlecha.LIGERA)
        {
            pEnemRapido *= 2;
        }
        else if(torreDebil == TipoFlecha.PESADA){
            pEnemFuerte *= 2;
        }

        int x = Random.Range(0, 101);
        if (x < pEnemRapido)//30% de spawn
        {
            GameManager._enemies.Add(spawned = Instantiate(enemigos[(int)TipoEnemigo.RAPIDO], transform.position, transform.rotation));
        }
        else if (x < pEnemRapido + pEnemFuerte)//30% de spawn
        {
            GameManager._enemies.Add(spawned = Instantiate(enemigos[(int)TipoEnemigo.FUERTE], transform.position, transform.rotation));
        }
        else//40% de spawn
        {
            GameManager._enemies.Add(spawned = Instantiate(enemigos[(int)TipoEnemigo.DEFAULT], transform.position, transform.rotation));
        }
        Debug.Log("Enemigos: " + GameManager._enemies.Count);

        spawned.GetComponent<Enemigo>().vidas = spawned.GetComponent<Enemigo>().vidas * ((GameManager.Instance.getRonda() / 10) + 1);

    }
}
