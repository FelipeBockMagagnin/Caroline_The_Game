using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMoonManager : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;

    public Transform spawnRight;
    public Transform spawnLeft;

    public float SpawnWaitTime;


    public void StartSpawnning()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        SpawnRandomEnemyInRandomSpot();
        yield return new WaitForSeconds(SpawnWaitTime);
        SpawnWaitTime -= Time.deltaTime/3;
        Debug.Log("SpawnWaitTime: " + SpawnWaitTime); 
        StartCoroutine(spawn());
    }

    private int lastRoll = 0;
    void SpawnRandomEnemyInRandomSpot()
    {
        int roll = Random.Range(1,7);
        if(roll == lastRoll)
        {
            if(roll < 5)
            {
                roll++;
            }
            else 
            {
                roll--;
            }
        }

        switch(roll)
        {
            case 1:
                Instantiate(enemy1, spawnRight.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(enemy2, spawnLeft.position, Quaternion.identity);
                break;
            case 3: 
                Instantiate(enemy1, spawnLeft.position, Quaternion.identity);
                break;
            case 4:
                Instantiate(enemy2, spawnRight.position, Quaternion.identity);
                break;
            case 5:
                Instantiate(enemy1, spawnLeft.position, Quaternion.identity);
                break;
            case 6:
                Instantiate(enemy1, spawnRight.position, Quaternion.identity);
                break;
            case 7:
                Instantiate(enemy1, spawnLeft.position, Quaternion.identity);
                break;
            case 8:
                Instantiate(enemy1, spawnRight.position, Quaternion.identity);
                break;
        }      
        lastRoll = roll;
    }

    public void StopSpawnning()
    {
        StopAllCoroutines();
    }
}
