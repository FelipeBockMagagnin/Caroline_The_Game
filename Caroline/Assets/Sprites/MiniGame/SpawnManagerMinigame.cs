using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerMinigame : MonoBehaviour
{
    public GameObject rock;    
    public float xPos, maxY, minY;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(spawnRock());
    }

    IEnumerator spawnRock()
    {
        Instantiate(rock, new Vector3(xPos, Random.Range(minY, maxY), 0), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(spawnRock());
    }
}
