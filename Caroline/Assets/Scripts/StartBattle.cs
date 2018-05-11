using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour {


    public GameObject InimigosObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Menina"))
        {
            InimigosObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
