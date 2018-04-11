using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour {


	public int Vidainimigo = 2;



	void OnTriggerEnter2D(Collider2D tiro){
		if(tiro.CompareTag("Tiro")){
			Vidainimigo--;
			Destroy (tiro.gameObject);
			if(Vidainimigo == 0){
				Destroy (gameObject);
			}


	}

}

}
