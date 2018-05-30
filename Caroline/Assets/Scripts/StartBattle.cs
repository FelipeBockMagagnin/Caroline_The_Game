using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour {


    public GameObject  InimigosObject;
    public AudioSource SfxInicioBattle;


    //startar a batalha quando passar em cima
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Menina"))
        {
            SfxInicioBattle.Play();
            InimigosObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
