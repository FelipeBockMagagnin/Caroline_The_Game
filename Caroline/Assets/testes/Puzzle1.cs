using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1 : MonoBehaviour {

	public GameObject Frase;
	public bool fim;
	public GameObject porta;

	void OnTriggerEnter2D(Collider2D collision){
		if(collision.CompareTag("Menina")){
			if(fim){
				Destroy(porta);
			}
		Frase.SetActive(true);
		}
	}

	void OnTriggerExit2D(Collider2D collision){
		if(collision.CompareTag("Menina")){
		Frase.SetActive(false);
		}
	}
}
