using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Change_camera_atributes : MonoBehaviour {

public Animator cam;
public bool podeShake = true;

//public Text alo;

	public void shake(){
		if(podeShake){
			cam.SetTrigger("shake");   
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.CompareTag("descer")){
			cam.SetBool("descer",true);
			podeShake = false;
		}
		
		if(collider.CompareTag("dialogo")){
			cam.SetBool("dialogo",true);
			podeShake = false;
		}	
	}
	
	void OnTriggerExit2D(Collider2D collider){
		if(collider.CompareTag("descer")){
			cam.SetBool("descer",false);
			podeShake = true;
		}
		if(collider.CompareTag("dialogo")){
			cam.SetBool("dialogo",false);
			podeShake = true;
		}
	}
}
