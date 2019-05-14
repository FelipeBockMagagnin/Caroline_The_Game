using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerMinigame : MonoBehaviour
{
    public float xPos, maxY, minY;

    // Start is called before the first frame update
    public void StartRockSpawn(GameObject _spawnObject, float _time, int _rounds)
    {
        StartCoroutine(spawnRock(_spawnObject, _time, _rounds));
    }

    IEnumerator spawnRock(GameObject _spawnObject, float _time, int _rounds)
    {
        for(int x = 0; x < _rounds; x++)
        {
            Instantiate(_spawnObject, new Vector3(xPos, Random.Range(minY, maxY), 0), Quaternion.identity);
            yield return new WaitForSeconds(_time);
        }
        Debug.Log("End burst of " + _spawnObject.name);
    }
}
