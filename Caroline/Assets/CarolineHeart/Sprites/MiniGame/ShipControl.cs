using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipControl : MonoBehaviour
{
    public float vel;
    public float minY, maxY;
    public GameObject bullet, rock;
    public Text rocksTxt;
    GameObject clone;
    private SpawnManagerMinigame spawnManagerMinigame;

    private void Awake()
    {
        spawnManagerMinigame = GameObject.Find("SpawnManagerRocks").GetComponent<SpawnManagerMinigame>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow) && this.transform.position.y < maxY)
        {
            transform.Translate(0,vel*Time.deltaTime,0);
        } 
        else if(Input.GetKey(KeyCode.DownArrow) && this.transform.position.y > minY)
        {
            transform.Translate(0,-vel*Time.deltaTime,0);
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            clone = Instantiate(bullet, transform.position, Quaternion.identity);
            Destroy(clone, 2);
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            spawnManagerMinigame.StartRockSpawn(rock, 0.1f, 50);
        }

        rocksTxt.text = "Points:" + MiniGameManager.rocksDestroyed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("pedra"))
        {
            MiniGameManager.rocksDestroyed -= 50;
            Destroy(other.gameObject);
        }
    }
}
