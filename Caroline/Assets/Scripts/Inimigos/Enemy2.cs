using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyFather {

    public float tiroSpeed;

	private void OnEnable() {
        escala = transform.localScale;
        escalaX = escala.x;
        anim = GetComponent<Animator>();
        Seguir(OqueSeguir);
    }

	void FixedUpdate(){
		transform.Translate(speed * Time.deltaTime, 0, 0);

        if(Input.GetKeyDown(KeyCode.T)){
            anim.SetBool("Atirar", true);
            if (menina.transform.position.x >= transform.position.x){
                speed = 0;
                escala = transform.localScale;
                escala.x = Mathf.Abs(escalaX);
                transform.localScale = escala;        
            } else {
                speed = 0;
                escala = transform.localScale;
                escala.x = -(Mathf.Abs(escalaX));
                transform.localScale = escala;       
            }
        }
        
	}   

    void tiro(){
        print("aaaa");
        if (menina.transform.position.x >= transform.position.x){
                speed = tiroSpeed;
                escala = transform.localScale;
                escala.x = Mathf.Abs(escalaX);
                transform.localScale = escala;        
            } else {
                speed = -tiroSpeed;
                escala = transform.localScale;
                escala.x = -(Mathf.Abs(escalaX));
                transform.localScale = escala;       
            }
    }


}
