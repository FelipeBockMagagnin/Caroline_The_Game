using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_1_manager : MonoBehaviour {

	public Animator luz_Puzzle;
	public Animator porta_quebrar;
	[SerializeField] private ParticleSystem particula_puzzle_win;
	public GameObject Armadilha;
	[SerializeField] private Animator cam;

	public void Perdeu () {
		Armadilha.SetActive(true);
		cam.SetTrigger("shake");
	}	
	
	public void Ganhou () {
		luz_Puzzle.SetBool("On",true);
		StartCoroutine(Explodir_Porta(1f));	
	}    


	IEnumerator Explodir_Porta (float WaitTime){
		yield return new WaitForSeconds(WaitTime);
		porta_quebrar.SetBool("explodir",true);
		particula_puzzle_win.Play();
		cam.SetTrigger("shake");		
	}
	
}
