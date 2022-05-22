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
            //Debug.Log("EnemRest: " + enemigosRestantes);
            if (enemigosRestantes <= 0 && enemies.Count <= 0) //Ha terminado la ronda 100%
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
        //Debug.Log("Spawning...");
        enemigosRestantes = GameManager.Instance.getRonda();
        for (int i = 0; i < GameManager.Instance.getRonda(); i++)
        {
            SpawnEnemy();
            enemigosRestantes--;
            yield return new WaitForSeconds(1f);
        }
        //GameManager.Instance.setRondaActiva(false);

        //Debug.Log("SPAWN: " + GameManager.Instance.getRondaActiva());
    }

    void SpawnEnemy()
    {
        if (GameManager.Instance.getRonda() <= 1)
        {
            enemies.Add(Instantiate(enemigoNormal, transform.position, transform.rotation));
            Debug.Log("Enemigos: " + enemies.Count);
        }
        else
        {

            int x = Random.Range(0, 2);
            if (x == 0)
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
