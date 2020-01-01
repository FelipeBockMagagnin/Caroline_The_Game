using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksSpeed : MonoBehaviour {

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
	
}
