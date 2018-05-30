using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirMenina : MonoBehaviour {

	public Transform    menina;         //guarda localização da menina
	public float        Somavetor;      //guarda vetor a somar na posição

	void Update () {
		Vector2 ola = transform.position;
		ola.y = transform.position.y;
		ola.x = menina.transform.position.x + 4.5f;
		transform.position = ola;
	}
}
