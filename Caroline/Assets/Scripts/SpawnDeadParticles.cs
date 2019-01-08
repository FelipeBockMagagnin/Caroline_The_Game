using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDeadParticles : MonoBehaviour {

	public ParticleSystem particula;
	

	private void OnTriggerEnter2D(Collider2D other) {
		Instantiate(particula,transform.position,Quaternion.identity);	
	}
}
