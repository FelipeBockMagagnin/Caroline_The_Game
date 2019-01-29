using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    [Range(0,0.05f)]
	public float velParallax;
	public Renderer quad;
    Girl Menina;

    Vector2 offset;

    private void Start()
    {
        Menina = GameObject.Find("Girl").GetComponent<Girl>();
    }

    void FixedUpdate () {
        Move_Parallax();    
        offset = new Vector2 (velParallax * Time.deltaTime * Menina.verticalInput, 0);    
	}

    void Move_Parallax(){
        //Somente se a menina puder andar acontece o parallax
        if (Menina.canMove == true && Menina.stopParallax == false)
        {
                quad.material.mainTextureOffset += offset;
        }
    }
}
