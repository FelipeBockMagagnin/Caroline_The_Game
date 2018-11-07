using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_particle_morte : MonoBehaviour {

	public ParticleSystem particula;
	

	private void OnTriggerEnter2D(Collider2D other) {
		Instantiate(particula,transform.position,Quaternion.identity);	
	}
}
