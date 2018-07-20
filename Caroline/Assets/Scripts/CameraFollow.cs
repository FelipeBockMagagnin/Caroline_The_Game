using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform    OqueSeguir;     //guarda a posição do objeto a seguir
	public float        moveSpeed;      //speed que segue
	public Vector3      offset;         //posicionamento

	void LateUpdate(){
		Vector3 posicaoNormal = OqueSeguir.position + offset;
		posicaoNormal.y = transform.position.y;
		Vector3 SmoothPosition = Vector3.Lerp (transform.position, posicaoNormal, moveSpeed*Time.deltaTime);
		transform.position = SmoothPosition;
	}
}
