using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Change_camera_atributes : MonoBehaviour {

public Animator cam;

public Text alo;

	void OnTriggerEnter2D(Collider2D collider){

		if(collider.CompareTag("descer")){
			cam.SetBool("descer",true);
		}
		

	
	}
	void OnTriggerExit2D(Collider2D collider){
		if(collider.CompareTag("descer")){
			cam.SetBool("descer",false);
		}
	}
}
