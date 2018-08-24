using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	public float velParallax;
	public Renderer quad;
    [SerializeField] Menina Menina;

    Vector2 offset;

	void FixedUpdate () {
        Move_Parallax();    
        offset = new Vector2 (velParallax * Time.deltaTime * Menina.inputVertical, 0);    
	}

    void Move_Parallax(){
        //Somente se a menina puder andar acontece o parallax
        if (Menina.PodeAndar == true && Menina.pararParallar == false)
        {
                quad.material.mainTextureOffset += offset;
        }
    }
}
