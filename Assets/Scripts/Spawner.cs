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

    private float timer = 2f;
    private int waveIndex = 0;

    private void Update()
    {
        if (timer <= 0f)
        {
            StartCoroutine(Spawn());
            timer = cooldown;
        }
        //Debug.Log("Timer: " + timer);
        timer -= Time.deltaTime;
    }

    IEnumerator Spawn()
    {
        waveIndex++;
        //Debug.Log("Spawning...");

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnEnemy()
    {
        if (waveIndex <= 1)
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
