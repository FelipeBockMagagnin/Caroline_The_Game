using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

		public GameObject[] Tutoriais;


	void Start(){
		for (int i = 0 ; i < Tutoriais.Length; i ++){
			Tutoriais[i].SetActive(false);
		}
	}
	
	void OnTriggerEnter2D(Collider2D collision){
		if(collision.CompareTag("Menina")){
			for (int i = 0 ; i < Tutoriais.Length; i ++){
				Tutoriais[i].SetActive(true);
			}
		//	Destroy(this.gameObject);
		}
	}
	void OnTriggerExit2D(Collider2D collision){
		if(collision.CompareTag("Menina")){
			for (int i = 0 ; i < Tutoriais.Length; i ++){
			Tutoriais[i].SetActive(false);
			}
		}
	}

}
