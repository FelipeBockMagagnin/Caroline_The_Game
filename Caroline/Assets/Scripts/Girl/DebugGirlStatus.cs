using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugGirlStatus : MonoBehaviour {

	public Girl scriptgirl;	
	public Text RockCont;
	public bool showVariables;

	void Update(){
        RockCont.text = "Ammunition: " + scriptgirl.ammunition;
	}
	
	// Update is called once per frame
	private void OnGUI()
    {
		if(showVariables)
        {
			GUI.Label (new Rect (0,0,500,500),
			"Input Vertical: " + scriptgirl.verticalInput + "\n" 		
			+ "PodeAndar: " + scriptgirl.canMove + "\n"
			+ "PodeUsarSpell: " + scriptgirl.canUseSpell + "\n"
			+ "Time: " + scriptgirl.time + "\n"
			+ "Face: " + scriptgirl.face + "\n"
			+ "No chão: " + scriptgirl.inGround + "\n"
			+ "pararparallax: " + scriptgirl.stopParallax + "\n"	
			+ "Força tiro: " + scriptgirl.shootingForce + "\n"
			+ "Forca tiro Absoluta: " + scriptgirl.absoluteShootingForce + "\n"
			+ "Munição: " + scriptgirl.ammunition + "\n"
			+ "Atirando" + scriptgirl.shooting + "\n");
		}		
	} 

}
