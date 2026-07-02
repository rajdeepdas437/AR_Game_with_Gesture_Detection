using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesToSpawn;
    private float spawnPosX;
    private float spawnPosZ;
    [SerializeField] float spawnCooldown=10f;
    [SerializeField] Transform player;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while(true)
        {
            spawnPosX = player.position.x + Random.Range(-10f, 10f);
            spawnPosZ = player.position.z + Random.Range(-10f, 10f);
            Vector3 spawnPos = new Vector3(spawnPosX, 2, spawnPosZ);
            Instantiate(enemiesToSpawn[0], spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnCooldown);
        }
    } 
}
