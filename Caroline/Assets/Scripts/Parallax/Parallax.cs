using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	public float velParallax;
	public Renderer quad;
    public GameObject Menina;

    Vector2 offset;


    void Start(){
        offset = new Vector2 (velParallax * Time.deltaTime, 0);
    }

	void Update () {

        //Somente se a menina puder andar acontece o parallax
        if (Menina.GetComponent<Menina>().PodeAndar == true && Menina.GetComponent<Menina>().pararParallar == false)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                quad.material.mainTextureOffset += offset;
                
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                quad.material.mainTextureOffset += -offset;
            }
        }
	}
}
