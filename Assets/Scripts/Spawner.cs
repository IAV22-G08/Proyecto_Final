using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static List<GameObject> enemies = new List<GameObject>();

    public GameObject enemigoNormal;
    public GameObject enemigoRapido;
    public GameObject enemigoPesado;
    public float cooldown = 100f;
    private int enemigosRestantes;
    private float timer = 2f;

    private void Update()
    {
        //if (timer <= 0f)
        //{
        //    StartCoroutine(Spawn());
        //    timer = cooldown;
        //}
        //Debug.Log("Timer: " + timer);
        //timer -= Time.deltaTime;

        Debug.Log(GameManager.Instance.getRondaActiva());
        if (GameManager.Instance.getRondaActiva() && enemigosRestantes <= 0)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        //if (GameManager.Instance.getRondaActiva())
        {
            GameManager.Instance.setRondaActiva(false);
            GameManager.Instance.SumaRonda(1);
            Debug.Log("Spawning...");
            enemigosRestantes = GameManager.Instance.getRonda();
            for (int i = 0; i < GameManager.Instance.getRonda(); i++)
            {
                SpawnEnemy();
                enemigosRestantes--;
                yield return new WaitForSeconds(1f);
            }


        }

    }

  

    void SpawnEnemy()
    {
        if (GameManager.Instance.getRonda() <= 1)
        {
            enemies.Add(Instantiate(enemigoNormal,transform.position,transform.rotation));
            Debug.Log("Enemigos: " + enemies.Count);
        }
        else
        {

            int x = Random.Range(0, 2);
            if(x == 0)
            {
                enemies.Add(Instantiate(enemigoRapido, transform.position, transform.rotation));
            }
            else
            {
                enemies.Add(Instantiate(enemigoPesado, transform.position, transform.rotation));

            }
            Debug.Log("Enemigos: " + enemies.Count);
        }
    }
}
