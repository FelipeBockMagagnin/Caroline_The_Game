using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMoonManager : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;

    public Transform SpawnDir;
    public Transform SpawnEsq;

    public float SpawnWaitTime;


    public void StartSpawnning()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        SpawnRandomEnemyInRandomSpot();
        yield return new WaitForSeconds(SpawnWaitTime);
        StartCoroutine(spawn());
    }

    void SpawnRandomEnemyInRandomSpot()
    {
        //BASIC CODE TO TEST
        Instantiate(enemy1, SpawnDir.position, Quaternion.identity);
        Instantiate(enemy2, SpawnEsq.position, Quaternion.identity);
    }

    public void StopSpawnning()
    {
        StopAllCoroutines();
    }
}
