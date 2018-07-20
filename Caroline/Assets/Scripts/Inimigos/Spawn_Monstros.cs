using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Monstros : MonoBehaviour {


	public GameObject[] inimigos;
	public AudioSource somSpawn;

	void Start(){
		for (int i = 0 ; i < inimigos.Length; i ++){
			inimigos[i].SetActive(false);
		}
	}
	
	void OnTriggerEnter2D(Collider2D collision){
		if(collision.CompareTag("Menina")){
			somSpawn.Play();
			for (int i = 0 ; i < inimigos.Length; i ++){
				inimigos[i].SetActive(true);
			}
			Destroy(this.gameObject);
		}

	}
}
