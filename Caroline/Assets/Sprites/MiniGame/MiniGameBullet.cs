using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiniGameBullet : MonoBehaviour
{
    public float speed;
    public float timeToDestroy;
    

    private void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }

    void Update () {
		transform.Translate(speed*Time.deltaTime,0,0);
        
	}

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("pedra"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            MiniGameManager.rocksDestroyed += 1;
        }
    }
}
