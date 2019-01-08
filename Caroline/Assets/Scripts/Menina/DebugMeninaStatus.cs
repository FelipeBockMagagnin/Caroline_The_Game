using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMeninaStatus : MonoBehaviour {

	public Menina scriptmenina;	

	public Text PedraCont;

	public bool showVariables;

	// Use this for initialization
	void Start () {
	}

	void Update(){
		PedraCont.text = "Ammunition: " + scriptmenina.quantidadeMunicao;
	}
	
	// Update is called once per frame
	private void OnGUI() {
		if(showVariables){
			GUI.Label (new Rect (0,0,500,500),
			"Input Vertical: " + scriptmenina.inputVertical + "\n" 		
			+ "PodeAndar: " + scriptmenina.PodeAndar + "\n"
			+ "PodeUsarSpell: " + scriptmenina.podeUsarSpeel + "\n"
			+ "Time: " + scriptmenina.time + "\n"
			+ "Face: " + scriptmenina.face + "\n"
			+ "No chão: " + scriptmenina.nochao + "\n"
			+ "pararparallax: " + scriptmenina.pararParallar + "\n"	
			+ "Força tiro: " + scriptmenina.forcaTiro + "\n"
			+ "Forca tiro Absoluta: " + scriptmenina.forcaTiroAbsoluta + "\n"
			+ "Munição: " + scriptmenina.quantidadeMunicao + "\n"
			+ "Empurrando" + scriptmenina.empurrando + "\n"
			+ "Atirando" + scriptmenina.atirando + "\n");
		}		
	} 
	
	
}
