using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMoonManager : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject rocks;

    public Transform spawnRight;
    public Transform spawnLeft;
    public Transform rockSpawn1;
    public Transform rockSpawn2;    

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
        int roll = Random.Range(1,11);
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
            case 9:
                Instantiate(rocks, rockSpawn1.position, Quaternion.identity);
                break;
            case 10:
                Instantiate(rocks, rockSpawn2.position, Quaternion.identity);
                break;
        }      
        lastRoll = roll;
    }

    public void StopSpawnning()
    {
        StopAllCoroutines();
    }
}
