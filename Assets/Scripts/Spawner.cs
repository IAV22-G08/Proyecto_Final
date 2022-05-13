using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform enemyPrefab2;
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
        Debug.Log("Timer: " + timer);
        timer -= Time.deltaTime;
    }

    IEnumerator Spawn()
    {
        waveIndex++;
        Debug.Log("Spawning...");

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnEnemy()
    {
        if (waveIndex <= 5)
            Instantiate(enemyPrefab, transform.position, transform.rotation);
        else
            Instantiate(enemyPrefab2, transform.position, transform.rotation);
    }
}
