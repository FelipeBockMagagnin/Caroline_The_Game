using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_X : MonoBehaviour {

	public Transform Menina;

	Vector3 x;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Vector3 posicao = transform.position;
		posicao.x = Menina.position.x;
		transform.position = posicao;



	
	}
}
