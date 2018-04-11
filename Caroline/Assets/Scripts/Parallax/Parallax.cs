using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	public float velParallax;
	public Renderer quad;

	void Update () {
		Vector2 offset = new Vector2 (velParallax * Time.deltaTime, 0);

		if (Input.GetKey (KeyCode.RightArrow)) {
			quad.material.mainTextureOffset += offset;
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			quad.material.mainTextureOffset += -offset;
		}
	}
}
