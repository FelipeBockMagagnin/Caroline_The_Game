using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Change_camera_atributes : MonoBehaviour {

public Animator cam;
public bool canShake = false;
            
	public void shake()
    {
		if(canShake)
        {
			cam.SetTrigger("shake");   
		}
	}

    public void NormalShake()
    {
        cam.SetTrigger("shake");
    }

	void OnTriggerEnter2D(Collider2D collider)
    {
		if(collider.CompareTag("descer")){
			cam.SetBool("descer",true);
            canShake = false;
		}
		
		if(collider.CompareTag("dialogo")){
			cam.SetBool("dialogo",true);
            canShake = false;
		}	
	}
	
	void OnTriggerExit2D(Collider2D collider)
    {
		if(collider.CompareTag("descer")){
			cam.SetBool("descer",false);
            canShake = false;
		}
		if(collider.CompareTag("dialogo")){
			cam.SetBool("dialogo",false);
            canShake = false;
		}
	}
}
