using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonsters : MonoBehaviour {


	public GameObject[] enemys;

	void Start(){
		for (int i = 0 ; i < enemys.Length; i ++)
        {
            enemys[i].SetActive(false);
		}
	}
	
	void OnTriggerEnter2D(Collider2D collision)
    {
		if(collision.CompareTag("Menina")){
			for (int i = 0 ; i < enemys.Length; i ++){
                enemys[i].SetActive(true);
			}
			Destroy(this.gameObject);
		}

	}
}
