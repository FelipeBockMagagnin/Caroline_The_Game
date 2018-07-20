using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_in_seconds : MonoBehaviour {

	public float Time;

	void Start () {
		Destroy(gameObject,Time);
	}
}	
	