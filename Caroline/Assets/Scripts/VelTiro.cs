using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelTiro : MonoBehaviour {

	void Update () {
		transform.Translate (100*Time.deltaTime, 0, 0);
	}
}


