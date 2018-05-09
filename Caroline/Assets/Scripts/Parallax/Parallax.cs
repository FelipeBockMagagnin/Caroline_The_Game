using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	public float velParallax;
	public Renderer quad;

    public GameObject Menina;

	void Update () {
        //acessa a var que indica se a menina pode movimentar-se
        bool podeandar = Menina.GetComponent<Menina>().PodeAndar;

		Vector2 offset = new Vector2 (velParallax * Time.deltaTime, 0);



        //Somente se a menina puder andar acontece o parallax
        if (podeandar == true)
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
