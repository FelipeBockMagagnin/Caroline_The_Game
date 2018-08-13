using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botões : MonoBehaviour {

	Animator anim_botão;
	public bool vaiPerder;
	public bool vaiGanhar;

	private Botões script;

	

	[SerializeField]Puzzle_1_manager puzzle_script;


	void Start(){
		anim_botão = GetComponent<Animator>();
		script = GetComponent<Botões>();

		
	}

	
	void OnTriggerStay2D(Collider2D collision){
		if(collision.CompareTag("Menina") && Input.GetKey(KeyCode.X)){
			anim_botão.SetBool("On",true);
			if(vaiPerder){
				puzzle_script.Perdeu();
				Destroy(script);
			}

			if(vaiGanhar){
				puzzle_script.Ganhou();
				Destroy(script);
			}
		}
	}	
}
