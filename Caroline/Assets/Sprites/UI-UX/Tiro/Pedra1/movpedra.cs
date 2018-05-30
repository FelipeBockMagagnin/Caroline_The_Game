using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movpedra : MonoBehaviour {	

	void Update () {
        transform.Translate(40 * Time.deltaTime, 0, 0);
    }
}
