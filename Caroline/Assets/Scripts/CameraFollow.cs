using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform OqueSeguir;
	public float moveSpeed = 0.125f;
	public Vector3 offset;

	void FixedUpdate(){
		Vector3 posicaoNormal = OqueSeguir.position + offset;
		posicaoNormal.y = transform.position.y;
		Vector3 SmoothPosition = Vector3.Lerp (transform.position, posicaoNormal, moveSpeed*Time.deltaTime);
		transform.position = SmoothPosition;
	}
}
